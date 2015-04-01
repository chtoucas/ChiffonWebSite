namespace Chiffon.Internal
{
    using Chiffon.Infrastructure.Persistence;
    using Chiffon.Services;

    using Narvalo;

    internal static class Mapper
    {
        /// <summary />
        public static NewMemberParameters Map(RegisterMemberRequest request, string encryptedPassword)
        {
            Require.NotNull(request, "query");
            Require.NotNullOrEmpty(encryptedPassword, "encryptedPassword");

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
