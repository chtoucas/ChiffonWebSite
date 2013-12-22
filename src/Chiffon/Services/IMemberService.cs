namespace Chiffon.Services
{
    using System;
    using Chiffon.Entities;
    using Narvalo.Fx;

    public interface IMemberService
    {
        event EventHandler<MemberCreatedEventArgs> MemberCreated;

        Maybe<Member> MayLogOn(string email, string password);
        Outcome<Member> RegisterMember(RegisterMemberQuery query);
    }
}
