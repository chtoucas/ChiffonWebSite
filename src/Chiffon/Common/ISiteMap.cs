namespace Chiffon.Common
{
    using System;

    using Chiffon.Entities;

    public interface ISiteMap
    {
        ChiffonLanguage Language { get; }

        Uri Home();

        Uri About();

        Uri Contact();

        Uri Newsletter();

        Uri Login();

        Uri Login(Uri returnUrl);

        Uri Register();

        Uri Register(Uri returnUrl);

        Uri Designer(DesignerKey designerKey, int pageIndex);

        Uri DesignerCategory(DesignerKey designerKey, string categoryKey, int pageIndex);

        Uri DesignerPattern(DesignerKey designerKey, string categoryKey, string reference, int pageIndex);
    }
}
