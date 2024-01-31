
using System;


namespace SmartBuilding
{
    class Program
    {
        static void Main(string[] args)
        {

            try
            {
                var controller = new BuildingController(buildingID: "", startState: "fire_drill");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //Assert
                //Assert.Throws<ApplicationException>(() => { });
            }

        }
    }
}
