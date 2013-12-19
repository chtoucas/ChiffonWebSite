namespace Chiffon.ViewModels
{
    using System.Diagnostics.CodeAnalysis;

    public class LoginViewModel
    {
        [SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
        public string ReturnUrl { get; set; }
    }
}