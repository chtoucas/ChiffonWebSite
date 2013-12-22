namespace Chiffon.Infrastructure.Addressing
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Chiffon.Entities;

    public interface ISiteMap
    {
        ChiffonLanguage Language { get; }

        Uri BaseUri { get; }

        Uri Home();
        Uri About();
        Uri Contact();
        Uri Newsletter();

        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Login")]
        Uri Login();
        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Login")]
        Uri Login(Uri returnUrl);
        Uri Register();
        Uri Register(Uri returnUrl);

        Uri Designer(DesignerKey designerKey, int pageIndex);
        Uri DesignerCategory(DesignerKey designerKey, string categoryKey, int pageIndex);
        Uri DesignerPattern(DesignerKey designerKey, string categoryKey, string reference, int pageIndex);

        Uri MakeAbsoluteUri(string relativeUri);
        Uri MakeAbsoluteUri(Uri relativeUri);
    }
}
