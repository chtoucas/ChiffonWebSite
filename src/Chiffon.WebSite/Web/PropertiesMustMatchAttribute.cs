namespace Narvalo.Web {
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public sealed class PropertiesMustMatchAttribute : ValidationAttribute {
        private const string _DefaultErrorMessage = "'{0}' et '{1}' sont différents.";

        private readonly object _typeId = new object();

        public PropertiesMustMatchAttribute(string originalProperty, string confirmProperty)
            : base(_DefaultErrorMessage) {
            OriginalProperty = originalProperty;
            ConfirmProperty = confirmProperty;
        }

        public string ConfirmProperty { get; private set; }

        public string OriginalProperty { get; private set; }

        public override object TypeId {
            get {
                return _typeId;
            }
        }

        public override string FormatErrorMessage(string name) {
            return String.Format(
                CultureInfo.CurrentUICulture, 
                ErrorMessageString,
                OriginalProperty, 
                ConfirmProperty);
        }

        public override bool IsValid(object value) {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(value);
            object originalValue = properties.Find(OriginalProperty, true /* ignoreCase */).GetValue(value);
            object confirmValue = properties.Find(ConfirmProperty, true /* ignoreCase */).GetValue(value);
            return Object.Equals(originalValue, confirmValue);
        }
    }
}
