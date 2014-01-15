namespace Chiffon
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Web;

    public class Global : HttpApplication
    {
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        protected void Application_Start()
        {
            AppActivator.Start();
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        protected void Application_End(object sender, EventArgs e)
        {
            AppActivator.End();
        }
    }
}