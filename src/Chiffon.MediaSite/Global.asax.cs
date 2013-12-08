namespace Chiffon
{
    using System;
    using System.Web;

    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            AppActivator.Start();
        }

        protected void Application_End(object sender, EventArgs e)
        {
            AppActivator.End();
        }
    }
}