namespace Narvalo.Web {
    using System;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class TooltipAttribute : Attribute {
        public static readonly TooltipAttribute Default;

        public TooltipAttribute() { }

        public TooltipAttribute(string tooltip) {
            TooltipValue = tooltip;
        }

        public virtual string Tooltip { get { return TooltipValue; } }

        protected string TooltipValue { get; set; }
    }
}