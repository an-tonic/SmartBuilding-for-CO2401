namespace SmartBuilding
{
    public class BuildingController
    {

        string buildingID;
        string currentState;
        string[] validStates = { "open", "closed", "out_of_hours", "fire_drill", "fire_alarm" };


        public BuildingController(string buildingID = "", string state = "")
        {
            this.buildingID = buildingID.ToLower();
            this.currentState = state.ToLower();
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
            if (validStates.Contains(state))
            {
                this.currentState = state;
                return true;
            }
            else
            {
                return false;
            }
            
        }
    }
}