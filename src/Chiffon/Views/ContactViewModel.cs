namespace Chiffon.Views
{
    using System.ComponentModel.DataAnnotations;

    public sealed class ContactViewModel
    {
        [Required(
            ErrorMessageResourceType = typeof(Strings_Views),
            ErrorMessageResourceName = Strings_Names.Email_Required)]
        [StringLength(200, MinimumLength = 5,
            ErrorMessageResourceType = typeof(Strings_Views),
            ErrorMessageResourceName = Strings_Names.Email_StringLength)]
        [EmailAddress(
            ErrorMessageResourceType = typeof(Strings_Views),
            ErrorMessageResourceName = Strings_Names.Email_DataType)]
        public string Email { get; set; }

        [Required(
            ErrorMessageResourceType = typeof(Strings_Views),
            ErrorMessageResourceName = Strings_Names.Message_Required)]
        [StringLength(3000, MinimumLength = 10,
            ErrorMessageResourceType = typeof(Strings_Views),
            ErrorMessageResourceName = Strings_Names.Message_StringLength)]
        public string Message { get; set; }

        [Required(
            ErrorMessageResourceType = typeof(Strings_Views),
            ErrorMessageResourceName = Strings_Names.Name_Required)]
        [StringLength(200, MinimumLength = 2,
            ErrorMessageResourceType = typeof(Strings_Views),
            ErrorMessageResourceName = Strings_Names.Name_StringLength)]
        public string Name { get; set; }
    }
}