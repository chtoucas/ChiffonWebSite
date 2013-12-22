namespace Chiffon.Services
{
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;

    public class RegisterMemberQuery
    {
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool NewsletterChecked { get; set; }

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase",
            Justification = "Email addresses are case-insensitive and better lloking lowercase.")]
        public void Normalize()
        {
            CompanyName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(CompanyName);
            Email = Email.ToLowerInvariant();
            FirstName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(FirstName);
            LastName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(LastName);
        }
    }
}
