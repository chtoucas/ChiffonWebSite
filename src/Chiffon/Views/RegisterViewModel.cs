namespace Chiffon.Views
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;

    // Attribut DataType pour Email
    // Attribut Display
    // LabelFor
    [MetadataType(typeof(UIMetadata_))]
    public sealed class RegisterViewModel
    {
        private string _companyName;
        private string _email;
        private string _firstName;
        private string _lastName;

        [Required(
            ErrorMessageResourceType = typeof(Strings_Views),
            ErrorMessageResourceName = Strings_Names.CompanyName_Required)]
        [StringLength(100, MinimumLength = 2,
            ErrorMessageResourceType = typeof(Strings_Views),
            ErrorMessageResourceName = Strings_Names.CompanyName_StringLength)]
        public string CompanyName
        {
            get { return _companyName; }
            set { _companyName = NormalizeName_(value); }
        }

        [Required(
            ErrorMessageResourceType = typeof(Strings_Views),
            ErrorMessageResourceName = Strings_Names.Email_Required)]
        [StringLength(200, MinimumLength = 5,
            ErrorMessageResourceType = typeof(Strings_Views),
            ErrorMessageResourceName = Strings_Names.Email_StringLength)]
        [EmailAddress(
            ErrorMessageResourceType = typeof(Strings_Views),
            ErrorMessageResourceName = Strings_Names.Email_DataType)]
        public string Email
        {
            get
            {
                return _email;
            }

            set
            {
                if (!String.IsNullOrWhiteSpace(value))
                {
                    _email = value.ToLowerInvariant();
                }
            }
        }

        [Required(
            ErrorMessageResourceType = typeof(Strings_Views),
            ErrorMessageResourceName = Strings_Names.FirstName_Required)]
        [StringLength(50, MinimumLength = 2,
            ErrorMessageResourceType = typeof(Strings_Views),
            ErrorMessageResourceName = Strings_Names.FirstName_StringLength)]
        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = NormalizeName_(value); }
        }

        [Required(
            ErrorMessageResourceType = typeof(Strings_Views),
            ErrorMessageResourceName = Strings_Names.LastName_Required)]
        [StringLength(50, MinimumLength = 2,
            ErrorMessageResourceType = typeof(Strings_Views),
            ErrorMessageResourceName = Strings_Names.LastName_StringLength)]
        public string LastName
        {
            get { return _lastName; }
            set { _lastName = NormalizeName_(value); }
        }

        // NB: On n'impose pas de contrainte sur ce champs car il sera traité manuellement.
        public string Newsletter { get; set; }

        // NB: On n'impose pas de contrainte sur ce champs car il sera traité manuellement.
        [SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
        public string ReturnUrl { get; set; }

        private static string NormalizeName_(string value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            // NB: La méthode ToTitleCase() ne touche pas les mots entièrement en majuscule,
            // on passe donc d'abord ces derniers en minuscules.

            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.ToLowerInvariant());
        }

        private class UIMetadata_
        {
        }
    }
}