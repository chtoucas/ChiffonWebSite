namespace Chiffon.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using Chiffon.ViewModels.Resources;

    public class ContactViewModel
    {
        [Required(
            ErrorMessageResourceType = typeof(SR),
            ErrorMessageResourceName = SRNames.Email_Required)]
        [StringLength(200, MinimumLength = 5,
            ErrorMessageResourceType = typeof(SR),
            ErrorMessageResourceName = SRNames.Email_StringLength)]
        [EmailAddress(
            ErrorMessageResourceType = typeof(SR),
            ErrorMessageResourceName = SRNames.Email_DataType)]
        public string Email { get; set; }

        [Required(
            ErrorMessageResourceType = typeof(SR),
            ErrorMessageResourceName = SRNames.Message_Required)]
        [StringLength(3000, MinimumLength = 10,
            ErrorMessageResourceType = typeof(SR),
            ErrorMessageResourceName = SRNames.Message_StringLength)]
        public string Message { get; set; }

        [Required(
            ErrorMessageResourceType = typeof(SR),
            ErrorMessageResourceName = SRNames.Name_Required)]
        [StringLength(200, MinimumLength = 2,
            ErrorMessageResourceType = typeof(SR),
            ErrorMessageResourceName = SRNames.Name_StringLength)]
        public string Name { get; set; }
    }
}