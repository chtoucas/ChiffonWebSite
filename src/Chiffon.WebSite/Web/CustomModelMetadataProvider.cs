namespace Narvalo.Web {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    public class CustomModelMetadataProvider : DataAnnotationsModelMetadataProvider {
        protected override ModelMetadata CreateMetadata(
            IEnumerable<Attribute> attributes,
            Type containerType,
            Func<object> modelAccessor,
            Type modelType,
            string propertyName) {

            var data = base.CreateMetadata(attributes,
                containerType,
                modelAccessor,
                modelType,
                propertyName);

            var watermark = attributes.SingleOrDefault(a => typeof(PlaceholderAttribute) == a.GetType());
            if (watermark != null) {
                data.Watermark = ((PlaceholderAttribute)watermark).Watermark;
            }
            var editMask = attributes.SingleOrDefault(a => typeof(EditMaskAttribute) == a.GetType());
            if (editMask != null) {
                data.AdditionalValues.Add("EditMask", ((EditMaskAttribute)editMask).EditMask);
            }

            return data;
        }

    }
}
