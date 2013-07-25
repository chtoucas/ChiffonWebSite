namespace Narvalo.Web {
    using System;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class PlaceholderAttribute : Attribute {
        public static readonly PlaceholderAttribute Default;

        public PlaceholderAttribute() { }

        public PlaceholderAttribute(string watermark) {
            WatermarkValue = watermark;
        }

        public virtual string Watermark { get { return WatermarkValue; } }

        protected string WatermarkValue { get; set; }
    }
}