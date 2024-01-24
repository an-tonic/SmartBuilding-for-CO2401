using NUnit.Framework;
using SmartBuilding;


namespace SmartBuildingTests
{
    [TestFixture]
    public class BuildingControllerTests
    {

        [Test]
        public void L1R1_L1R2_Constructor_SetBuildingID()
        {

            //Arrenge
            string newBuildingID = "test";
            var controller = new BuildingController(newBuildingID);
            

            //Act
            string returnedBuildingID = controller.GetBuildingID();

            //Assert
            Assert.AreEqual(newBuildingID, returnedBuildingID);
        }

        [Test]
        public void L1R3_Constructor_SetsBuildingIDToLowercase()
        {

            //Arrenge
            string shouldReturnID = "test";
            var controller = new BuildingController("TEST");


            //Act
            string returnedBuildingID = controller.GetBuildingID();

            //Assert
            Assert.AreEqual(shouldReturnID, returnedBuildingID);
        }

        [Test]
        public void L1R4_SetBuildingID_BuildingIDToLowercase()
        {

            //Arrenge
            string shouldReturnID = "test1";
            var controller = new BuildingController("TEST");


            //Act
            controller.SetBuildingID("TEST1");
            string returnedBuildingID = controller.GetBuildingID();

            //Assert
            Assert.AreEqual(shouldReturnID, returnedBuildingID);
        }

        [Test]
        public void L1R5_L1R6_Constructor_SetCurrentStateToOutOfHours()
        {

            //Arrenge
            
            string newCurrentState = "out_of_hours";
            var controller = new BuildingController(state:newCurrentState);


            //Act
            string returnedState = controller.GetCurrentState();

            //Assert
            Assert.AreEqual(newCurrentState, returnedState);
        }



        [TestCase("out_of_hours")]
        [TestCase("closed")]
        [TestCase("open")]
        [TestCase("fire_drill")]
        [TestCase("fire_alarm")]
        public void L1R7_CheckValidCurrentState_ReturnTrue(string testState)
        {

            //Arrenge
            var controller = new BuildingController();


            //Act
            bool returnedState = controller.SetCurrentState(testState);

            //Assert
            Assert.IsTrue(returnedState);
        }


        [TestCase("invalid")]
        [TestCase("")]

        public void L1R7_CheckInvalidCurrentState_ReturnFalse(string testState)
        {

            //Arrenge
            var controller = new BuildingController();


            //Act
            bool returnedState = controller.SetCurrentState(testState);

            //Assert
            Assert.IsFalse(returnedState);
        }


        [TestCase("out_of_hours")]
        [TestCase("closed")]
        [TestCase("open")]
        [TestCase("fire_drill")]
        [TestCase("fire_alarm")]
        public void L1R7_SetCurrentStateToValidState_ReturnTrue(string testState)
        {

            //Arrenge
            var controller = new BuildingController();


            //Act
            controller.SetCurrentState(testState);
            string returnedState = controller.GetCurrentState();

            //Assert
            Assert.AreEqual(returnedState, testState);
        }


      

    }
}