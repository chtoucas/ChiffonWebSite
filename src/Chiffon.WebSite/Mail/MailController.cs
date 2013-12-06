namespace Chiffon.Mail
{
    using System;
    using System.Net.Mail;
    using System.Net.Mime;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Narvalo;

    /// <summary>
    /// The Base class for Mailers. Your mailer should subclass MailerBase:
    /// </summary>
    public class MailController : ControllerBase
    {
        /// <summary>
        /// The parameterless constructor
        /// </summary>
        public MailController()
        {
            if (HttpContext.Current != null) {
                CurrentHttpContext = new HttpContextWrapper(HttpContext.Current);
            }
        }

        /// <summary>
        /// The Path to the MasterPage or Layout
        /// e.g. Razor: myMailer.MasterName = "_MyLayout.cshtml"
        /// </summary>
        public string MasterName { get; set; }

        /// <summary>
        /// The MailerName determines the folder that contains the views for this mailer
        /// </summary>
        protected string MailerName { get { return GetType().Name; } }

        public HttpContextBase CurrentHttpContext { get; set; }

        /// <summary>
        /// Nothing to Execute at this point, left blank
        /// </summary>
        protected override void ExecuteCore()
        {
        }

        /// <summary>
        /// Populates the mailMessage with content rendered from the view using the provided masterName
        /// </summary>
        /// <param name="message">a non null System.Net.Mail.MailMessage reference</param>
        /// <param name="viewName">The name of the view file, e.g. WelcomeMessage </param>
        /// <param name="masterName">The name of the master file, e.g. Layout </param>
        /// <param name="linkedResources">Key: linked resource id or CID, Value:Path to the resource</param>
        public void PopulateBody(MailMessage message, string viewName, string masterName = null)
        {
            Requires.NotNull(message, "message");

            masterName = masterName ?? MasterName;

            var textExists = TextViewExists(viewName, masterName);

            //if Text exists, it always goes to the body
            if (textExists) {
                PopulateTextBody(message, viewName, masterName);
            }

            // if html exists
            if (!HtmlViewExists(viewName, masterName)) return;

            if (textExists) {
                PopulateHtmlPart(message, viewName, masterName);
            }
            else {
                PopulateHtmlBody(message, viewName, masterName);
            }
        }

        /// <summary>
        /// Converts a view to its text view name
        /// </summary>
        /// <param name="viewName">e.g. Welcome</param>
        /// <returns>e.g. Welcome.text</returns>
        protected static string TextViewName(string viewName)
        {
            return viewName + ".text";
        }

        /// <summary>
        /// Returns a text master name or null if blank string passed
        /// </summary>
        /// <param name="masterName">e.g. Layout</param>
        /// <returns>e.g. Layout.text </returns>
        protected static string TextMasterName(string masterName)
        {
            return string.IsNullOrEmpty(masterName) ? null : masterName + ".text";
        }

        /// <summary>
        /// This method generates the EmailBody from the given viewName, masterName
        /// </summary>
        /// <param name="viewName">@example: "WelcomeMessage" </param>
        /// <param name="masterName">@example: "_MyLayout.cshtml" if nothing is set, then the MasterName property will be used instead</param>
        /// <returns>the raw html content of the email view and its master page</returns>
        protected string EmailBody(string viewName, string masterName = null)
        {
            masterName = masterName ?? MasterName;

            var result = new StringResult(viewName) {
                ViewData = ViewData,
                MasterName = masterName ?? MasterName
            };

            if (ControllerContext == null) {
                CreateControllerContext_();
            }

            result.ExecuteResult(ControllerContext, MailerName);
            return result.Output;
        }

        /// <summary>
        /// Populates the mailMessage.Body with a text/plain content
        /// </summary>
        /// <returns>The string containing the body</returns>
        protected string PopulateTextBody(MailMessage message, string viewName, string masterName)
        {
            message.Body = EmailBody(TextViewName(viewName), TextMasterName(masterName));
            message.IsBodyHtml = false;

            return message.Body;
        }

        /// <summary>
        /// Populates the mailMessage.Body with a text/html content and sets the IsBodyHtml to true
        /// </summary>
        /// <returns>The string containing the Html body</returns>
        protected string PopulateHtmlBody(MailMessage message, string viewName, string masterName)
        {
            message.Body = EmailBody(viewName, masterName);
            message.IsBodyHtml = true;

            return message.Body;
        }

        /// <summary>
        /// Populates a text/plain AlternateView inside the mailMessage
        /// </summary>
        protected AlternateView PopulateTextPart(MailMessage mailMessage, string viewName, string masterName)
        {
            return PopulatePart(mailMessage, TextViewName(viewName), "text/plain", TextMasterName(masterName));
        }

        /// <summary>
        /// Returns true if text view exists
        /// </summary>
        /// <param name="viewName">e.g. "Welcome" will look for "Welcome.text"</param>
        /// <param name="masterName">e.g. "Layout" will Look for "Layout.text"</param>
        protected bool TextViewExists(string viewName, string masterName)
        {
            return ViewExists(TextViewName(viewName), TextMasterName(masterName));
        }

        /// <summary>
        /// Returns true if html view exists
        /// </summary>
        protected bool HtmlViewExists(string viewName, string masterName)
        {
            return ViewExists(viewName, masterName);
        }

        /// <summary>
        /// Returns true if both text and html views are present
        /// </summary>
        protected bool IsMultiPart(string viewName, string masterName)
        {
            return TextViewExists(viewName, masterName) && HtmlViewExists(viewName, masterName);
        }

        /// <summary>
        /// Populates a text/html AlternateView inside the mailMessage
        /// </summary>
        protected AlternateView PopulateHtmlPart(MailMessage message, string viewName, string masterName)
        {
            return PopulatePart(message, viewName, "text/html", masterName);
        }

        /// <summary>
        /// Populates an AlternateView inside the mailMessage
        /// </summary>
        /// <param name="mime">e.g. text/plain, text/html etc.</param>
        protected AlternateView PopulatePart(MailMessage message, string viewName, string mime, string masterName = null)
        {
            masterName = masterName ?? MasterName;
            if (ViewExists(viewName, masterName)) {
                var part = EmailBody(viewName, masterName);
                var alternateView = AlternateView.CreateAlternateViewFromString(part, new ContentType(mime));
                message.AlternateViews.Add(alternateView);
                return alternateView;
            }

            return null;
        }

        /// <summary>
        /// Determines if a View exists given its name and masterName
        /// </summary>
        protected bool ViewExists(string viewName, string masterName)
        {
            if (ControllerContext == null) {
                CreateControllerContext_();
            }

            var controllerName = ControllerContext.RouteData.Values["controller"];
            ControllerContext.RouteData.Values["controller"] = MailerName;

            try {
                return ViewEngines.Engines.FindView(ControllerContext, viewName, masterName).View != null;
            }
            finally {
                ControllerContext.RouteData.Values["controller"] = controllerName;
            }
        }

        void CreateControllerContext_()
        {
            if (CurrentHttpContext == null) {
                throw new InvalidOperationException("CurrentHttpContext cannot be null");
            }
            var routeData = RouteTable.Routes.GetRouteData(CurrentHttpContext);
            ControllerContext = new ControllerContext(CurrentHttpContext, routeData, this);
        }
    }
}