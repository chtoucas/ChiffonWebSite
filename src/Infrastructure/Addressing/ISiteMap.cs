namespace Chiffon.Infrastructure.Addressing
{
    using System;
    using Chiffon.Entities;

    public interface ISiteMap
    {
        Uri Home();
        Uri About();
        Uri Contact();
        Uri Newsletter();

        Uri Register();
        Uri LogOn();
        Uri LogOn(Uri targetUrl);

        Uri Designer(DesignerKey designer);
        Uri DesignerCategory(DesignerKey designer, string category);
        Uri DesignerPattern(DesignerKey designer, string reference);
    }
}
