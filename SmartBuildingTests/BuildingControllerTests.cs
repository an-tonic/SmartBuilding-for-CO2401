using NUnit.Framework;
using SmartBuilding;


namespace SmartBuildingTests
{
    [TestFixture]
    public class BuildingControllerTests
    {



        [Test]
        public void L1R1_L1R2_BuildingControllerConstructor_SetBuildingID()
        {

            //Arrenge
            string newBuildingID = "test";
            var controller = new BuildingController(newBuildingID);
            

            //Act
            string returnedBuildingID = controller.GetBuildingID();

            //Assert
            Assert.AreEqual(newBuildingID, returnedBuildingID);
        }

       


        //naming convention for your test method names: MethodBeingTested_TestScenario_ExpectedOutput
        //E.g. SetCurrentState_InvalidState_ReturnsFalse

   


    }
}