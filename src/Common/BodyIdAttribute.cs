namespace Chiffon.Common
{
    using System.Web.Mvc;
    using Narvalo;

    public sealed class BodyIdAttribute : ActionFilterAttribute
    {
        readonly string _value;

        public BodyIdAttribute(string value)
        {
            Requires.NotNullOrEmpty(value, "value");

            _value = value;
        }

        public string Value { get { return _value; } }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            filterContext.Controller.ViewData["BodyId"] = Value;
        }
    }
}
