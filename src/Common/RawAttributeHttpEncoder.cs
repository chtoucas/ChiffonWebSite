namespace Chiffon.Common
{
    using System.IO;
    using System.Web.Util;

    public class RawAttributeHttpEncoder : HttpEncoder
    {
        protected override void HtmlAttributeEncode(string value, TextWriter output)
        {
            output.Write(value);
        }
    }
}