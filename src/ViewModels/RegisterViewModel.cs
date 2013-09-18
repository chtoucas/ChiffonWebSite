namespace Chiffon.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    // FIXME: messages d'erreur en anglais.
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Le champs \"Email\" est obligatoire.")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Le champs \"Email\" doit comporter au moins 5 caractères.")]
        [EmailAddress(ErrorMessage = "Vous devez utiliser une adresse email valide.")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Le champs \"Prénom\" est obligatoire.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Le champs \"Prénom\" doit comporter au moins 2 caractères.")]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "Le champs \"Nom\" est obligatoire.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Le champs \"Nom\" doit comporter au moins 2 caractères.")]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "Le champs \"Agence\" est obligatoire.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Le champs \"Agence\" doit comporter au moins 2 caractères.")]
        public string CompanyName { get; set; }

        public string Message { get; set; }
        public string ReturnUrl { get; set; }
    }
}