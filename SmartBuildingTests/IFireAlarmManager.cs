namespace SmartBuildingTests
{
    public interface IFireAlarmManager : IManager
    {
        public void SetAlarm(bool isActive);
    }
}
