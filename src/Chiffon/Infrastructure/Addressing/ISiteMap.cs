namespace Chiffon.Infrastructure.Addressing
{
    using System;

    public interface ISiteMap
    {
        Uri Home();

        Uri LogOn();

        Uri LogOn(LogOnOptions options);

        Uri MakeAbsoluteUri(Uri targetUrl);
    }
}
