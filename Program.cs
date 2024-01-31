
using System;


namespace SmartBuildingTests
{
    class Program
    {
        static void Main(string[] args)
        {

            var controller = new BuildingController();


            //Act
            controller.SetCurrentState("fire_drill");

        }
    }
}
