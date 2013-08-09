namespace Chiffon.ViewModels
{
    using System;
    using System.Globalization;
    using Chiffon.Common;
    using Chiffon.Entities;

    public class PatternPreviewViewModel
    {
        public DesignerKey DesignerKey { get; set; }
        public string DesignerName { get; set; }
        public string Reference { get; set; }

        public string CssClass { get { return CssUtility.Designer(DesignerKey); } }

        public string Description
        {
            get
            {
                return String.Format(CultureInfo.CurrentCulture, 
                    "Motif textile, référence n°{0}, par {1}", Reference, DesignerName);
            }
        }

    }
}
