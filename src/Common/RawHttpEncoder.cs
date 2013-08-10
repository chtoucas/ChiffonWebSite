namespace Chiffon.Common
{
    using System.IO;
    using System.Web.Util;

    public class RawHttpEncoder : HttpEncoder
    {
        protected override void HtmlAttributeEncode(string value, TextWriter output)
        {
            output.Write(value);
        }

        protected override void HtmlEncode(string value, TextWriter output)
        {
            output.Write(value);
        }
    }
}