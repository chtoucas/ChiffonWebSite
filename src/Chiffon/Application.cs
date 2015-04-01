namespace Chiffon
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Web;

    public class Application : HttpApplication
    {
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        protected void Application_Start()
        {
            ApplicationActivator.Start();
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        protected void Application_End(object sender, EventArgs e)
        {
            ApplicationActivator.End();
        }
    }
}