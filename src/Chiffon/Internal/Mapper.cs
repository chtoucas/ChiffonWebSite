namespace Chiffon.Internal
{
    using Chiffon.Persistence;
    using Chiffon.Services;
    using Narvalo;

    static class Mapper
    {
        /// <summary />
        public static NewMemberParameters Map(RegisterMemberRequest request, string encryptedPassword)
        {
            Requires.NotNull(request, "query");
            Requires.NotNullOrEmpty(encryptedPassword, "encryptedPassword");

            return new NewMemberParameters {
                CompanyName = request.CompanyName,
                Email = request.Email,
                EncryptedPassword = encryptedPassword,
                FirstName = request.FirstName,
                LastName = request.LastName,
                NewsletterChecked = request.NewsletterChecked,
            };
        }
    }
}
