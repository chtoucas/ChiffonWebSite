﻿namespace Chiffon.ViewModels
{
    using System.Diagnostics.CodeAnalysis;
    using System.ComponentModel.DataAnnotations;

    // FIXME: messages d'erreur en anglais.
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Le champs \"Email\" est obligatoire.")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Le champs \"Email\" doit comporter au moins 5 caractères.")]
        [EmailAddress(ErrorMessage = "Vous devez utiliser une adresse email valide.")]
        //[DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Le champs \"Prénom\" est obligatoire.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Le champs \"Prénom\" doit comporter au moins 2 caractères.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Le champs \"Nom\" est obligatoire.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Le champs \"Nom\" doit comporter au moins 2 caractères.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Le champs \"Agence\" est obligatoire.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Le champs \"Agence\" doit comporter au moins 2 caractères.")]
        public string CompanyName { get; set; }

        // FIXME
        public string Newsletter { get; set; }

        public string Message { get; set; }

        [SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
        public string ReturnUrl { get; set; }
    }
}