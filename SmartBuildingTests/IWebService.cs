namespace SmartBuildingTests
{
    public interface IWebService
    {
        public void LogFireAlarm(string logDetails);

        public void LogStateChange(string logDetails);

        public void LogEngineerRequired(string logDetails);
    }
}
