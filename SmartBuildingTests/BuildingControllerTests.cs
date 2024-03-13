using NSubstitute;
using NUnit.Framework;
using SmartBuilding;

namespace SmartBuildingTests
{
    [TestFixture]
    public class BuildingControllerTests
    {

        [Test]
        //L1R1_L1R2
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
        //L1R3
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
        //L1R4
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
        //L1R5_L1R6
        public void L1R5_L1R6_Constructor_SetCurrentStateToOutOfHours()
        {

            //Arrange
            
            string correctState = "out of hours";
            var controller = new BuildingController(buildingID: "testID");

            //Act
            string returnedState = controller.GetCurrentState();

            //Assert
            Assert.AreEqual(correctState, returnedState);
        }


        [TestCase("out of hours")]
        [TestCase("closed")]
        [TestCase("open")]
        //L1R7
        public void L1R7_CheckValidCurrentState_ReturnTrue(string testState)
        {
            //Arrange
            var controller = new BuildingController(buildingID: "testID");

            //Act
            bool returnedState = controller.SetCurrentState(testState);

            //Assert
            Assert.IsTrue(returnedState);
        }    

        [TestCase("out of hours")]
        [TestCase("closed")]
        [TestCase("open")]
        //L1R7
        public void L1R7_SetCurrentStateToValidState_ReturnTrue(string testState)
        {
            //Arrange
            var controller = new BuildingController(buildingID: "testID");

            //Act
            controller.SetCurrentState(testState);
            string returnedState = controller.GetCurrentState();

            //Assert
            Assert.AreEqual(returnedState, testState);
        }

        [TestCase("invalid")]
        [TestCase("")]
        //L1R7
        public void L1R7_CheckInvalidCurrentState_ReturnFalse(string testState)
        {
            //Arrange
            var controller = new BuildingController(buildingID: "testID");

            //Act
            bool returnedState = controller.SetCurrentState(testState);

            //Assert
            Assert.IsFalse(returnedState);
        }


        [TestCase("out of hours")]
        [TestCase("closed")]
        [TestCase("open")]
        //L2R1
        public void L2R1_SetStateToFireAlarmFromAnyState_ReturnTrue(string testState)
        {
            //Arrange
            var controller = new BuildingController(buildingID: "testID", testState);

            //Act
            bool stateSetSucsessfully =  controller.SetCurrentState("fire alarm");
            
            //Assert
            Assert.IsTrue(stateSetSucsessfully);
        }

        [TestCase("out of hours")]
        [TestCase("closed")]
        [TestCase("open")]
        //L2R1
        public void L2R1_SetStateToFireDrillFromAnyState_ReturnTrue(string testState)
        {
            //Arrange
            var controller = new BuildingController(buildingID: "testID", testState);

            //Act
            bool stateSetSucsessfully = controller.SetCurrentState("fire drill");

            //Assert
            Assert.IsTrue(stateSetSucsessfully);
        }

        [Test]
        //L2R1
        public void L2R1_InitialStateOpenSetInvalidState_ReturnFalse()
        {
            //Arrange
            var controller = new BuildingController(buildingID: "testID");
            controller.SetCurrentState("closed");

            //Act
            bool stateSetSucsessfully = controller.SetCurrentState("open");

            //Assert
            Assert.IsFalse(stateSetSucsessfully);
        }

        [Test]
        //L2R1
        public void L2R1_InitialStateClosedSetInvalidState_ReturnFalse()
        {
            //Arrange
            var controller = new BuildingController(buildingID: "testID");
            controller.SetCurrentState("open");

            //Act
            bool stateSetSucsessfully = controller.SetCurrentState("closed");

            //Assert
            Assert.IsFalse(stateSetSucsessfully);
        }

        [TestCase("out of hours")]
        [TestCase("closed")]
        [TestCase("open")]
        //L2R1
        public void L2R1_SetStateToCorrectPreviousState_ReturnTrue(string testState)
        {
            //Arrange
            var controller = new BuildingController(buildingID: "testID", startState: testState);
            controller.SetCurrentState("fire drill");

            //Act
            bool stateIsPrevious = controller.SetCurrentState(testState);

            //Assert
            Assert.IsTrue(stateIsPrevious);
        }

        [TestCase("out of hours")]
        [TestCase("closed")]
        //L2R1
        public void L2R1_SetStateToIncorrectPreviousState_ReturnFalse(string testState)
        {
            //Arrange
            var controller = new BuildingController(buildingID: "testID", startState: testState);
            controller.SetCurrentState("fire drill");

            //Act
            bool state = controller.SetCurrentState("open");

            //Assert
            Assert.IsFalse(state);
        }

        [Test]
        //L2R1
        public void L2R1_SetStateToFireAlarmAndThenFireDrill_ReturnFalse()
        {
            //Arrange
            var controller = new BuildingController(buildingID: "testID");
            controller.SetCurrentState("fire alarm");

            //Act
            bool state = controller.SetCurrentState("fire drill");

            //Assert
            Assert.IsFalse(state);
        }

        [Test]
        //L2R1
        public void L2R1_SetStateToFireDrillAndThenFireAlarm_ReturnFalse()
        {
            //Arrange
            var controller = new BuildingController(buildingID: "testID");
            controller.SetCurrentState("fire drill");

            //Act
            bool state = controller.SetCurrentState("fire alarm");

            //Assert
            Assert.IsFalse(state);
        }

        [TestCase("out of hours")]
        [TestCase("closed")]
        [TestCase("open")]
        [TestCase("fire drill")]
        [TestCase("fire alarm")]
        //L2R2
        public void L2R2_SetCurrentStateToSameState_ReturnTrue(string testState)
        {
            //Arrange
            var controller = new BuildingController(buildingID: "testID");
            
            //Act
            controller.SetCurrentState(testState);
            bool stateSetSucsessfully = controller.SetCurrentState(testState);

            //Assert
            Assert.IsTrue(stateSetSucsessfully);
        }

        [TestCase("out of hours")]
        [TestCase("closed")]
        [TestCase("open")]
        [TestCase("fire drill")]
        [TestCase("fire alarm")]
        //L2R2
        public void L2R2_SetCurrentStateToSameState_StateStaysTheSame(string testState)
        {
            //Arrange
            var controller = new BuildingController(buildingID: "testID");

            //Act
            controller.SetCurrentState(testState);
            controller.SetCurrentState(testState);
            bool stateSetSucsessfully = controller.GetCurrentState() == testState;

            //Assert
            Assert.IsTrue(stateSetSucsessfully);
        }

        [TestCase("out of hoUrs")]
        [TestCase("clOsed")]
        [TestCase("opeN")]
        //L2R3
        public void L2R3_SetCurrentStateToUpperCase_SetsToLowerCase(string testState)
        {
            //Arrange
            var controller = new BuildingController(buildingID: "testID", startState: testState);

            //Act
            string setState = controller.GetCurrentState();

            //Assert

            Assert.AreEqual(testState.ToLower(), setState);

        }
        [TestCase("fire drill")]
        [TestCase("fire alarm")]
        //L2R3
        public void L2R3_SetCurrentStateToWrongState_ThrowException(string testState)
        {
            string correctMessage = "Argument Exception: BuildingController can only be initialised to the following states 'open', 'closed', 'out of hours'";

            //Arrange & Act & Assert
            Assert.Throws(Is.TypeOf<ArgumentException>()
                        .And.Message.EqualTo(correctMessage),
                        () => new BuildingController(buildingID: "testID", startState: testState)
            );

        }

        [TestCase("test1", "test2", "test3")]
        [TestCase("Lights,OK,OK,FAULT,OK,OK,OK,OK,FAULT,OK,OK,",
                "Doors,OK,OK,OK,OK,OK,OK,OK,OK,OK,OK,",
                "FireAlarm,OK,OK,FAULT,OK,OK,OK,OK,FAULT,OK,OK,")]
        //L3R3
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

            var controller = new BuildingController("testID", lightManager, fireAlarmManager, doorManager, webService, emailService );
            //Act
            var report = controller.GetCurrentReport();

            //Asset
            Assert.AreEqual (testString1 + testString2 + testString3, report);
        }

        [Test]
        //L3R4
        public void L3R4_SetStatusToOpen_OpenAllDoorsMethodIsCalled()
        {
            //Arrange
            ILightManager lightManager = Substitute.For<ILightManager>();
            IDoorManager doorManager = Substitute.For<IDoorManager>();
            IFireAlarmManager fireAlarmManager = Substitute.For<IFireAlarmManager>();
            IWebService webService = Substitute.For<IWebService>();
            IEmailService emailService = Substitute.For<IEmailService>();

            var controller = new BuildingController("testID", lightManager, fireAlarmManager, doorManager, webService, emailService);
            //Act
            controller.SetCurrentState("open");

            //Assert
            doorManager.Received().OpenAllDoors();
        }

        [Test]
        //L3R4
        public void L3R4_SetStatusToOpenAndOpenAllDoorsReturnsFalse_SetStatusReturnsFalse()
        {
            //Arrange
            ILightManager lightManager = Substitute.For<ILightManager>();
            IDoorManager doorManager = Substitute.For<IDoorManager>();
            doorManager.OpenAllDoors().Returns(false);
            IFireAlarmManager fireAlarmManager = Substitute.For<IFireAlarmManager>();
            IWebService webService = Substitute.For<IWebService>();
            IEmailService emailService = Substitute.For<IEmailService>();

            var controller = new BuildingController("testID", lightManager, fireAlarmManager, doorManager, webService, emailService);
            //Act
            var result = controller.SetCurrentState("open");

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        //L3R4
        public void L3R4_SetStatusToOpenAndOpenAllDoorsReturnsFalse_CurrentStatusStaysTheSame()
        {
            //Arrange
            ILightManager lightManager = Substitute.For<ILightManager>();
            IDoorManager doorManager = Substitute.For<IDoorManager>();
            doorManager.OpenAllDoors().Returns(false);

            IFireAlarmManager fireAlarmManager = Substitute.For<IFireAlarmManager>();
            IWebService webService = Substitute.For<IWebService>();
            IEmailService emailService = Substitute.For<IEmailService>();

            var controller = new BuildingController("testID", lightManager, fireAlarmManager, doorManager, webService, emailService);
            //Act
            controller.SetCurrentState("open");
            var result = controller.GetCurrentState();

            //Assert
            Assert.AreEqual(result, "out of hours");
        }
        [Test]
        //L3R5
        public void L3R5_SetStatusToOpenAndOpenAllDoorsReturnsTrue_CurrentStatusMovesToOpen()
        {
            //Arrange
            ILightManager lightManager = Substitute.For<ILightManager>();
            IDoorManager doorManager = Substitute.For<IDoorManager>();
            doorManager.OpenAllDoors().Returns(true);
            IFireAlarmManager fireAlarmManager = Substitute.For<IFireAlarmManager>();
            IWebService webService = Substitute.For<IWebService>();
            IEmailService emailService = Substitute.For<IEmailService>();

            var controller = new BuildingController("testID", lightManager, fireAlarmManager, doorManager, webService, emailService);
            //Act
            controller.SetCurrentState("open");
            var result = controller.GetCurrentState();

            //Assert
            Assert.AreEqual(result, "open");
        }

        [Test]
        //L4R1
        public void L4R1_SetStatusToClosed_OpenAllDoorsAndSetAllLIghtMethodsAreCalled()
        {
            //Arrange
            ILightManager lightManager = Substitute.For<ILightManager>();
            IDoorManager doorManager = Substitute.For<IDoorManager>();
            IFireAlarmManager fireAlarmManager = Substitute.For<IFireAlarmManager>();
            IWebService webService = Substitute.For<IWebService>();
            IEmailService emailService = Substitute.For<IEmailService>();

            var controller = new BuildingController("testID", lightManager, fireAlarmManager, doorManager, webService, emailService);
            //Act
            controller.SetCurrentState("closed");

            //Assert
            doorManager.Received().LockAllDoors();
            lightManager.Received().SetAllLights(false);

        }

        [Test]
        //L4R2
        public void L4R2_CurrentStateMovesToFireAlarm_OpenAllDoorsAndSetAllLightAndFireAlarmMethodsAreCalled()
        {
            //Arrange
            ILightManager lightManager = Substitute.For<ILightManager>();
            IDoorManager doorManager = Substitute.For<IDoorManager>();
            IFireAlarmManager fireAlarmManager = Substitute.For<IFireAlarmManager>();
            IWebService webService = Substitute.For<IWebService>();
            IEmailService emailService = Substitute.For<IEmailService>();

            var controller = new BuildingController("testID", lightManager, fireAlarmManager, doorManager, webService, emailService);
            //Act
            controller.SetCurrentState("fire alarm");

            //Assert
            doorManager.Received().LockAllDoors();
            lightManager.Received().SetAllLights(true);
            fireAlarmManager.Received().SetAlarm(true);
            webService.Received().LogFireAlarm("fire alarm");
        }

        [TestCase("Lights,FAULT,", "Doors,OK", "FireAlarm,OK,", "Lights,")]
        [TestCase("Lights,OK,", "Doors,FAULT", "FireAlarm,OK,", "Doors,")]
        [TestCase("Lights,OK,", "Doors,OK", "FireAlarm,FAULT,", "FireAlarm,")]
        [TestCase("Lights,FAULT,", "Doors,FAULT", "FireAlarm,OK,", "Lights,Doors,")]
        [TestCase("Lights,OK,", "Doors,FAULT", "FireAlarm,FAULT,", "Doors,FireAlarm,")]
        //L4R3
        public void L4R3_GetStatusReportDetectsAFault_TheLogEngineerRequiredMethodIsCalled(string testString1, string testString2, string testString3, string result)
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
            var controller = new BuildingController("testID", lightManager, fireAlarmManager, doorManager, webService, emailService);
            
            //Act
            controller.GetCurrentReport();

            //Assert
            webService.Received().LogEngineerRequired(result);
        }

        [Test]
        //L4R4
        public void L4R4_WebServiceLogFireAlarmThrowsError_EmailSent()
        {
            //Arrange
            ILightManager lightManager = Substitute.For<ILightManager>();
            IDoorManager doorManager = Substitute.For<IDoorManager>();
            IFireAlarmManager fireAlarmManager = Substitute.For<IFireAlarmManager>();
            IWebService webService = Substitute.For<IWebService>();
            IEmailService emailService = Substitute.For<IEmailService>();
            webService.When(x => x.LogFireAlarm(Arg.Any<string>()))
                      .Do(x => { throw new Exception("fake exception"); });

            var controller = new BuildingController("testID", lightManager, fireAlarmManager, doorManager, webService, emailService);
            //Act
            controller.SetCurrentState("fire alarm");

            //Assert
            emailService.Received().SendMail("smartbuilding@uclan.ac.uk", "failed to log alarm", "fake exception");
        }

    }

}