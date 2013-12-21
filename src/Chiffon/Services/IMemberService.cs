namespace Chiffon.Services
{
    using Narvalo.Fx;

    public interface IMemberService
    {
        Maybe<MemberInfo> MayLogOn(string emailAddress, string password);
    }
}
