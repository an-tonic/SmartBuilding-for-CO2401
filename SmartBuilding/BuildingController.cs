namespace SmartBuilding
{
    public class BuildingController
    {

        string buildingID;
        string currentState;

        public BuildingController(string buildingID)
        {
            this.buildingID = buildingID;
        }

        public string GetBuildingID()
        {
            return buildingID;
        }


    }
}