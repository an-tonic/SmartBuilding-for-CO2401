using SmartBuildingTests;


namespace SmartBuilding
{
    public class BuildingController
    {

        private string buildingID;
        private string currentState;
        private string? previousState;
        private IWebService? webService;
        private IEmailService? emailService;
        private ILightManager? lightManager;
        private IDoorManager? doorManager;
        private IFireAlarmManager? fireAlarmManager;

        string[] validInitialStates = { "fire drill", "open", "out of hours", "closed", "fire alarm" };
        string[] validStates = { "open", "out of hours", "closed" };
    

        public BuildingController(string buildingID)
        {
            this.buildingID = buildingID.ToLower();
            this.currentState = "out of hours";
        }

        

        public BuildingController(string buildingID, string startState)
        {
            this.buildingID = buildingID.ToLower();
            startState = startState.ToLower();

            if (validStates.Contains(startState))
            {
                this.currentState = startState;
            }
            else
            {
                throw new ArgumentException("Argument Exception: BuildingController can only be initialised to the following states 'open', 'closed', 'out of hours'");
            }

        }

        public BuildingController(string id,  ILightManager iLightManager, IFireAlarmManager iFireAlarmManager, IDoorManager iDoorManager, IWebService iWebService, IEmailService iEmailService)
        {
            buildingID = id.ToLower();
            currentState = "out of hours";
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

            if (!validInitialStates.Contains(state))
            {
                return false;
            }
            
            //set fire alarm
            if (state == validInitialStates[4])
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
                previousState = currentState;
                currentState = state;
                return true;
            }
            //set fire drill
            if(state == validInitialStates[0])
            {   
                previousState = currentState;
                currentState = state;
                return true;
            }

            //returning to previous state from drill
            if(currentState == "fire drill" && state == previousState)
            {
                currentState = state;
                return true;
            }
            //returning to previous state from alarm
            if (currentState == "fire alarm" && state == previousState)
            {
                currentState = state;
                return true;
            }

            if (Math.Abs(Array.IndexOf(validInitialStates, state) - Array.IndexOf(validInitialStates, currentState)) <= 1)
            {
                if (doorManager != null && lightManager != null)
                {

                    if (state == validInitialStates[1] && !doorManager.OpenAllDoors())
                    {
                        return false;
                    }
                    if (state == validInitialStates[3])
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