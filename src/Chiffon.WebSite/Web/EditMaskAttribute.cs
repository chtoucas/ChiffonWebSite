namespace Narvalo.Web {
    using System;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class EditMaskAttribute : Attribute {
        public static readonly EditMaskAttribute Default;

        public EditMaskAttribute() { }

        public EditMaskAttribute(string mask) {
            EditMaskValue = mask;
        }

        public virtual string EditMask { get { return EditMaskValue; } }

        protected string EditMaskValue { get; set; }
    }
}