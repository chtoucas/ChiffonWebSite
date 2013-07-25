namespace Chiffon.Web {
    using System;
    using System.Globalization;
    using System.Web.Mvc;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class BodyIdAttribute : ActionFilterAttribute
    {
        public string Id { get; set; }

        public override void OnActionExecuted(ActionExecutedContext filterContext) {
            string id;

            if (String.IsNullOrEmpty(Id)) {
                var action = filterContext.ActionDescriptor.ActionName.ToLowerInvariant();
                var controller = filterContext.Controller.GetType().Name
                    .Replace("Controller", string.Empty).ToLowerInvariant();
                id = String.Format(CultureInfo.InvariantCulture, "_{0}_{1}", controller, action);
            }
            else {
                id = Id;
            }

            filterContext.Controller.ViewData["id"] = id;
        }
    }
}
