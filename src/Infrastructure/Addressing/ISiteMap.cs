namespace Chiffon.Infrastructure.Addressing
{
    using System;
    using Chiffon.Entities;

    public interface ISiteMap
    {
        Uri BaseUri { get; }

        Uri Home();
        Uri About();
        Uri Contact();
        Uri Newsletter();

        Uri Login();
        Uri Register();
        Uri LogOn();
        Uri LogOn(Uri targetUrl);

        Uri Designer(DesignerKey designer);
        Uri DesignerCategory(DesignerKey designer, string category);
        Uri DesignerPattern(DesignerKey designer, string reference);

        Uri MakeAbsoluteUri(string relativeUri);
        Uri MakeAbsoluteUri(Uri relativeUri);
    }
}
