namespace Chiffon
{
    using System.Web;

    public class Global : HttpApplication
    {
        protected void Application_Start()
        {
            ApplicationLifecycle.Start();
        }
    }
}