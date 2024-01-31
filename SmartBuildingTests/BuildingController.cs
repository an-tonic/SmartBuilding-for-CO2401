using System.Collections.Immutable;

namespace SmartBuilding
{
    public class BuildingController
    {

        string buildingID;
        string currentState;
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

            if (validInitialStates.Contains(startState)){
                this.currentState = startState;
            } else
            {
                throw new ArgumentException("Argument Exception: BuildingController can only be initialised to the following states 'open', 'closed', 'out of hours'");
            }
            
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
                //fire alarm can be set from any state
                if(state == validStates[4])
                {
                    currentState = state;
                    return true;
                }
                else if (Math.Abs(Array.IndexOf(validStates, state) - Array.IndexOf(validStates, currentState)) <= 1)
                {
                    currentState = state;
                    return true;
                }
                else
                {
                    return false;
                }
                
            }
            
            return false;
            
            
        }
    }
}