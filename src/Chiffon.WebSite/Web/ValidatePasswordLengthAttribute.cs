namespace Narvalo.Web {
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Web.Security;

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class ValidatePasswordLengthAttribute : ValidationAttribute {
        private const string _DefaultErrorMessage = "'{0}' doit comporter au moins {1} caractères.";
        
        private readonly int _minCharacters = Membership.Provider.MinRequiredPasswordLength;

        public ValidatePasswordLengthAttribute()
            : base(_DefaultErrorMessage) {
        }

        public override string FormatErrorMessage(string name) {
            return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString, name, _minCharacters);
        }

        public override bool IsValid(object value) {
            string valueAsString = value as string;
            return valueAsString != null && valueAsString.Length >= _minCharacters;
        }
    }
}
