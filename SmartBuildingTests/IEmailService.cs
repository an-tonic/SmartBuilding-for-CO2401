namespace SmartBuildingTests
{
    public interface IEmailService
    {
        public void SendMail(string emailAddress, string subject, string message);
    }
}
