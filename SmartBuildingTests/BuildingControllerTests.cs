using NSubstitute;
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

            //Arrange
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

            //Arrange
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

            //Arrange
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

            //Arrange
            
            string correctState = "out_of_hours";
            var controller = new BuildingController();


            //Act
            string returnedState = controller.GetCurrentState();

            //Assert
            Assert.AreEqual(correctState, returnedState);
        }



        [TestCase("out_of_hours")]
        [TestCase("closed")]
        public void L1R7_CheckValidCurrentState_ReturnTrue(string testState)
        {

            //Arrange
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

            //Arrange
            var controller = new BuildingController();


            //Act
            bool returnedState = controller.SetCurrentState(testState);

            //Assert
            Assert.IsFalse(returnedState);
        }


        [TestCase("out_of_hours")]
        [TestCase("closed")]
        public void L1R7_SetCurrentStateToValidState_ReturnTrue(string testState)
        {

            //Arrange
            var controller = new BuildingController();


            //Act
            controller.SetCurrentState(testState);
            string returnedState = controller.GetCurrentState();

            //Assert
            Assert.AreEqual(returnedState, testState);
        }

        [TestCase("out_of_hours")]
        [TestCase("closed")]
        [TestCase("open")]
        [TestCase("fire_drill")]
    
        public void L2R1_SetStateToFireAlarmFromAnyState_ReturnTrue(string testState)
        {

            //Arrange
            var controller = new BuildingController(testState);


            //Act
            bool stateSetSucsessfully =  controller.SetCurrentState("fire_alarm");
            

            //Assert
            Assert.IsTrue(stateSetSucsessfully);


        }
        
        [TestCase("open")]
        [TestCase("fire_drill")]
        
        public void L2R1_SetInvalidState_ReturnFalse(string testState)
        {
            //Arrange
            var controller = new BuildingController();
            controller.SetCurrentState("closed");

            //Act
            bool stateSetSucsessfully = controller.SetCurrentState(testState);

            //Assert
            Assert.IsFalse(stateSetSucsessfully);

        }


        [TestCase("out_of_hours")]
        [TestCase("closed")]
        [TestCase("open")]
        [TestCase("fire_drill")]
        [TestCase("fire_alarm")]
        public void L2R2_SetCurrentStateToSameState_ReturnTrue(string testState)
        {
            //Arrange
            var controller = new BuildingController();
            //Needs to happen othervise would not be able to initially set fire_alarm
            if(testState == "fire_drill")
            {
                controller.SetCurrentState("open");
            }

            //Act
            controller.SetCurrentState(testState);
            bool stateSetSucsessfully = controller.SetCurrentState(testState);

            //Assert
            Assert.IsTrue(stateSetSucsessfully);
        }

        [TestCase("out_of_hours")]
        [TestCase("closed")]
        [TestCase("open")]
        [TestCase("fire_drill")]
        [TestCase("fire_alarm")]
        public void L2R2_SetCurrentStateToSameState_StateStaysTheSame(string testState)
        {
            //Arrange
            var controller = new BuildingController();
            //Needs to happen othervise would not be able to initially set fire_alarm
            if (testState == "fire_drill")
            {
                controller.SetCurrentState("open");
            }

            //Act
            controller.SetCurrentState(testState);
            controller.SetCurrentState(testState);
            bool stateSetSucsessfully = controller.GetCurrentState() == testState;

            //Assert
            Assert.IsTrue(stateSetSucsessfully);
        }

        [TestCase("oUt_of_hours")]
        [TestCase("clOsed")]
        [TestCase("opeN")]
        public void L2R3_SetCurrentStateToUpperCase_SetsToLowerCase(string testState)
        {
            //Arrange
            var controller = new BuildingController(buildingID: "", startState: testState);

            //Act
            string setState = controller.GetCurrentState();

            //Assert

            Assert.AreEqual(testState.ToLower(), setState);

        }
        [TestCase("fire_drill")]
        [TestCase("fire_alarm")]
        public void L2R3_SetCurrentStateToWrongState_ThrowException(string testState)
        {
            string correctMessage = "Argument Exception: BuildingController can only be initialised to the following states 'open', 'closed', 'out of hours'";

            //Arrange & Act & Assert
            Assert.Throws(Is.TypeOf<ArgumentException>()
                        .And.Message.EqualTo(correctMessage),
                        () => new BuildingController(buildingID: "", startState: testState)
            );

        }

        [TestCase("test1", "test2", "test3")]
        [TestCase("Lights,OK,OK,FAULT,OK,OK,OK,OK,FAULT,OK,OK,",
                "Doors,OK,OK,OK,OK,OK,OK,OK,OK,OK,OK,",
                "FireAlarm,OK,OK,FAULT,OK,OK,OK,OK,FAULT,OK,OK,")]
        public void L3R3_GetStatusReport_ReturnsCorrectString(string testString1, string testString2, string testString3 )
        {
            //Arrange
            ILightManager lightManager = Substitute.For<ILightManager>();
            lightManager.GetStatus().Returns(testString1);
            IDoorManager doorManager = Substitute.For<IDoorManager>();
            doorManager.GetStatus().Returns(testString2);
            IFireAlarmManager fireAlarmManager = Substitute.For<IFireAlarmManager>();
            fireAlarmManager.GetStatus().Returns(testString3);

            IWebService webService = Substitute.For<IWebService>();
            IEmailService emailService = Substitute.For<IEmailService>();

            var controller = new BuildingController("", "out_of_hours", lightManager, fireAlarmManager, doorManager, webService, emailService );
            //Act
            var report = controller.GetCurrentReport();

            //Asset

            Assert.AreEqual (testString1 + testString2 + testString3, report);
        }




    }



    
}