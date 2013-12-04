namespace Chiffon.Common
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using Chiffon.Infrastructure;

    public static class HtmlHelperExtensions
    {
        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Login")]
        public static IHtmlString AccountLoginContent(this HtmlHelper @this, ChiffonLanguage language)
        {
            return @this.Partial(ViewUtility.Localize(ViewName.Account.LoginContent, language));
        }

        public static IHtmlString AccountPostRegisterEmail(this HtmlHelper @this, ChiffonLanguage language)
        {
            return @this.Partial(ViewUtility.Localize(ViewName.Account.PostRegisterEmail, language));
        }

        public static IHtmlString AccountRegisterHelp(this HtmlHelper @this, ChiffonLanguage language)
        {
            return @this.Partial(ViewUtility.Localize(ViewName.Account.RegisterHelp, language));
        }

        public static IHtmlString AccountRegisterWarning(this HtmlHelper @this, ChiffonLanguage language)
        {
            return @this.Partial(ViewUtility.Localize(ViewName.Account.RegisterWarning, language));
        }

        public static IHtmlString HomeAboutContent(this HtmlHelper @this, ChiffonLanguage language)
        {
            return @this.Partial(ViewUtility.Localize(ViewName.Home.AboutContent, language));
        }

        public static IHtmlString HomeContactTitle(this HtmlHelper @this, ChiffonLanguage language)
        {
            return @this.Partial(ViewUtility.Localize(ViewName.Home.ContactTitle, language));
        }

        public static IHtmlString LayoutTitle(this HtmlHelper @this, ChiffonLanguage language)
        {
            return @this.Partial(ViewUtility.Localize(ViewName.Shared.LayoutTitle, language));
        }

        public static IHtmlString LayoutAuthorsRights(this HtmlHelper @this, ChiffonLanguage language)
        {
            return @this.Partial(ViewUtility.Localize(ViewName.Shared.LayoutAuthorsRights, language));
        }
    }
}