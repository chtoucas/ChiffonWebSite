namespace Chiffon.Mail
{
    using System.IO;
    using System.Text;
    using System.Web.Mvc;
    using Narvalo;

    /// <summary>
    /// Mimics the ViewResult with an important difference - the view is only stored 
    /// in the Output property instead of written to a browser stream!
    /// </summary>
    public class StringResult : ViewResult
    {
        public StringResult(string viewName)
        {
            ViewName = viewName;
        }

        /// <summary>
        /// Contains the output after compiling the view and putting in the values
        /// </summary>
        public string Output { get; private set; }

        public void ExecuteResult(ControllerContext context, string mailerName)
        {
            //remember the controller name
            var controllerName = context.RouteData.Values["controller"];

            //temporarily change it to the mailer name so that FindView works
            context.RouteData.Values["controller"] = mailerName;
            try {
                ExecuteResult(context);
            }
            finally {
                //restore the controller name
                context.RouteData.Values["controller"] = controllerName;
            }
        }

        public override void ExecuteResult(ControllerContext context)
        {
            Requires.NotNull(context, "context");
            Requires.NotNullOrEmpty(ViewName, "ViewName");

            if (View == null) {
                var result = FindView(context);
                View = result.View;
            }

            var sb = new StringBuilder();
            var sw = new StringWriter(sb);

            var viewContext = new ViewContext(context, View, ViewData, TempData, sw);
            View.Render(viewContext, sw);

            Output = sb.ToString();
        }
    }
}