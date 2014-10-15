namespace Chiffon.Mail
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Net.Mail;
    using System.Net.Mime;
    using System.Text;
    using System.Web.Mvc;
    using Narvalo;

    public class MailMessageResult : ViewResult
    {
        readonly MailMessage _message;

        public MailMessageResult(MailMessage message)
        {
            Requires.NotNull(message, "message");

            _message = message;
        }

        public MailMessage Message { get { return _message; } }

        public override void ExecuteResult(ControllerContext context)
        {
            Requires.NotNull(context, "context");

            var txtView = FindTextView(context).View;
            var htmlView = FindView(context).View;

            if (txtView == null && htmlView == null) {
                throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture,
                    "You must provide a view for this email. Views should be named {0}.txt.cshtml or {0}.cshtml depending on the format you wish to render.",
                    ViewName));
            }

            if (txtView != null) {
                var body = RenderView_(context, txtView);
                var altView = AlternateView.CreateAlternateViewFromString(body, Message.BodyEncoding, MediaTypeNames.Text.Plain);
                Message.AlternateViews.Add(altView);
            }

            if (htmlView != null) {
                var body = RenderView_(context, htmlView);
                var altView = AlternateView.CreateAlternateViewFromString(body, Message.BodyEncoding, MediaTypeNames.Text.Html);
                Message.AlternateViews.Add(altView);
            }
        }

        protected ViewEngineResult FindTextView(ControllerContext context)
        {
            var viewName = String.Format(CultureInfo.InvariantCulture, "{0}.txt", ViewName);
            var masterName = String.Format(CultureInfo.InvariantCulture, "{0}.txt", MasterName);

            return ViewEngines.Engines.FindView(context, viewName, masterName);
        }

        protected override ViewEngineResult FindView(ControllerContext context)
        {
            return ViewEngines.Engines.FindView(context, ViewName, MasterName);
        }

        string RenderView_(ControllerContext context, IView view)
        {
            var sb = new StringBuilder();

            using (var sw = new StringWriter(sb)) {
                var viewContext = new ViewContext(context, view, ViewData, TempData, sw);
                view.Render(viewContext, sw);

                return sb.ToString();
            }
        }
    }
}