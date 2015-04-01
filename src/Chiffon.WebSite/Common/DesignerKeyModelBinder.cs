namespace Chiffon.Common
{
    using System.Web.Mvc;
    using Chiffon.Entities;
    using Narvalo;

    public sealed class DesignerKeyModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            Require.NotNull(controllerContext, "controllerContext");

            var value = controllerContext.RouteData.Values["designerKey"];
            if (value == null) {
                return null;
            }

            return DesignerKey.Parse((string)value);
        }
    }
}