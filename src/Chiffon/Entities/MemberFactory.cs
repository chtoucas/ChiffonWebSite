namespace Chiffon.Entities
{
    using System.Diagnostics.Contracts;

    public static class MemberFactory
    {
        public static Member NewMember(string email, string firstName, string lastName)
        {
            Contract.Ensures(Contract.Result<Member>() != null);

            return new Member {
                Email = email,
                FirstName = firstName,
                LastName = lastName,
            };
        }
    }
}
