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

    public class RegisterMemberQuery
    {
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool NewsletterChecked { get; set; }
    }
}
