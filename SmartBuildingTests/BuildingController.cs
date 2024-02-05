using SmartBuildingTests;


namespace SmartBuilding
{
    public class BuildingController
    {

        private string buildingID;
        private string currentState;
        private IWebService webService;
        private IEmailService emailService;
        private ILightManager lightManager;
        private IDoorManager doorManager;
        private IFireAlarmManager fireAlarmManager;

        string[] validStates = { "fire_drill", "open", "out_of_hours", "closed", "fire_alarm" };
        string[] validInitialStates = { "open", "out_of_hours", "closed" };

        public BuildingController()
        {
            this.buildingID = "";
            this.currentState = "out_of_hours";
        }

        public BuildingController(string buildingID)
        {
            this.buildingID = buildingID.ToLower();
            this.currentState = "out_of_hours";
        }

        public BuildingController(string buildingID, string startState)
        {
            this.buildingID = buildingID.ToLower();
            startState = startState.ToLower();

            if (validInitialStates.Contains(startState))
            {
                this.currentState = startState;
            }
            else
            {
                throw new ArgumentException("Argument Exception: BuildingController can only be initialised to the following states 'open', 'closed', 'out of hours'");
            }

        }

        public BuildingController(string id, string startState, ILightManager iLightManager, IFireAlarmManager iFireAlarmManager, IDoorManager iDoorManager, IWebService iWebService, IEmailService iEmailService)
        {
            buildingID = id;
            currentState = startState;
            lightManager = iLightManager;
            doorManager = iDoorManager;
            fireAlarmManager = iFireAlarmManager;
            webService = iWebService;
            emailService = iEmailService;
        }

        public string GetBuildingID()
        {
            return buildingID;
        }

        public string GetCurrentState()
        {
            return currentState;
        }

        public void SetBuildingID(string buildingID)
        {
            this.buildingID = buildingID.ToLower();
        }

        public bool SetCurrentState(string state)
        {

            if (!validStates.Contains(state))
            {
                return false;
            }

            //fire alarm can be set from any state
            if (state == validStates[4])
            {

                lightManager?.SetAllLights(true);
                doorManager?.LockAllDoors();
                fireAlarmManager?.SetAlarm(true);
                try
                {
                    webService?.LogFireAlarm("fire alarm");
                }
                catch (Exception e)
                {
                    emailService?.SendMail("smartbuilding@uclan.ac.uk", "failed to log alarm", e.Message);
                }

                currentState = state;
                return true;
            }


            if (Math.Abs(Array.IndexOf(validStates, state) - Array.IndexOf(validStates, currentState)) <= 1)
            {
                if (doorManager != null && lightManager != null)
                {

                    if (state == validStates[1] && !doorManager.OpenAllDoors())
                    {
                        return false;
                    }
                    if (state == validStates[3])
                    {
                        lightManager.SetAllLights(false);
                        doorManager.LockAllDoors();
                        return true;
                    }
                }

                currentState = state;

                return true;
            }

            return false;
        }



        public string GetCurrentReport()
        {
            if (doorManager != null && lightManager != null && fireAlarmManager != null && webService != null)
            {
                string faultyLights = lightManager.GetStatus().Contains("FAULT") ? "Lights," : "";
                string faultyDoors = doorManager.GetStatus().Contains("FAULT") ? "Doors," : "";
                string faultyFireAlarm = fireAlarmManager.GetStatus().Contains("FAULT") ? "FireAlarm," : "";
                webService.LogEngineerRequired(faultyLights + faultyDoors + faultyFireAlarm);
                return lightManager.GetStatus() + doorManager.GetStatus() + fireAlarmManager.GetStatus();
            }
            return "";
        }

    }
}