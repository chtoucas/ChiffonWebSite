namespace Chiffon.Services
{
    public interface IMemberService
    {
        // TODO: devra renvoyer un UserInfo.
        string LogOn(string emailAddress, string password);
    }
}
