namespace Chiffon.ViewModels
{
    using System.Diagnostics.CodeAnalysis;
    using System.ComponentModel.DataAnnotations;

    // FIXME: messages d'erreur en anglais.
    public class ContactViewModel
    {
        [Required(ErrorMessage = "Le champs \"Email\" est obligatoire.")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Le champs \"Email\" doit comporter au moins 5 caractères.")]
        [EmailAddress(ErrorMessage = "Vous devez utiliser une adresse email valide.")]
        //[DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Le champs \"Nom\" est obligatoire.")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Le champs \"Nom\" doit comporter au moins 2 caractères.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Le champs \"Message\" est obligatoire.")]
        [StringLength(3000, MinimumLength = 10, ErrorMessage = "Le champs \"Message\" doit comporter au moins 10 caractères.")]
        public string Message { get; set; }
    }
}