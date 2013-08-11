namespace Chiffon.Common
{
    using System.Web.Mvc;
    using Narvalo;

    public sealed class MetaRobotsAttribute : ActionFilterAttribute
    {
        readonly string _value;

        public MetaRobotsAttribute(string value)
        {
            Requires.NotNullOrEmpty(value, "value");

            _value = value;
        }

        public string Value { get { return _value; } }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewData["Robots"] = "index, follow";
        }
    }
}
