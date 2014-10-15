namespace Chiffon.Domain
{
    public static class MemberFactory
    {
        public static Member NewMember(string email, string firstName, string lastName)
        {
            return new Member {
                Email = email,
                FirstName = firstName,
                LastName = lastName,
            };
        }
    }
}
