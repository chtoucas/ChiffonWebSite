namespace Chiffon.ViewModels
{
    using System.Diagnostics.CodeAnalysis;

    public sealed class RegisterSuccessViewModel
    {
        [SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
        public string NextUrl { get; set; }
    }
}