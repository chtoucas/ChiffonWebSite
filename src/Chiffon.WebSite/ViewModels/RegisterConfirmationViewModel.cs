namespace Chiffon.ViewModels
{
    using System.Diagnostics.CodeAnalysis;

    public class RegisterConfirmationViewModel
    {
        [SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
        public string NextUrl { get; set; }
    }
}