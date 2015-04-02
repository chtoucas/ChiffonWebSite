namespace Chiffon
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web;

    public class Global : HttpApplication
    {
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
            Justification = "ASP.Net requiert que cette méthode ne soit pas statique.")]
        protected void Application_Start()
        {
            ApplicationLifecycle.Start();
        }
    }
}