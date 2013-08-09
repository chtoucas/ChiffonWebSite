namespace Chiffon.Infrastructure.Addressing
{
    using System;

    public interface ISiteMap
    {
        Uri LogOn();

        Uri LogOn(Uri targetUrl);

        Uri MakeAbsoluteUri(Uri targetUrl);
    }
}
