namespace EventTracker.BusinessServices
{
    public interface IUserServices
    {
        int Authenticate(string userName, string password);
    }
}