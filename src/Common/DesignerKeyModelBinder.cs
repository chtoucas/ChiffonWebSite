namespace Chiffon.Common
{
    using System.Web.Mvc;
    using Chiffon.Entities;

    public class DesignerKeyModelBinder : IModelBinder
    {
        #region IModelBinder

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var value = controllerContext.RouteData.Values["designerKey"];
            if (value == null) {
                return null;
            }

            return DesignerKey.Parse((string)value);
        }

        #endregion
    }
}