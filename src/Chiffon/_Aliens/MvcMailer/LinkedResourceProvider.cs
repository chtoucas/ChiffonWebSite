namespace Mvc.Mailer
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Mail;
    using System.Net.Mime;

    /// <summary>
    /// This class is a utility class for instantiating LinkedResource objects
    /// </summary>
    public class LinkedResourceProvider : ILinkedResourceProvider
    {
        public IList<LinkedResource> GetAll(Dictionary<string, string> resources)
        {
            return resources
                .Select(resource => Get(resource.Key, resource.Value))
                .ToList();
        }

        public LinkedResource Get(string contentId, string filePath)
        {
            return new LinkedResource(filePath, GetContentType(filePath)) { ContentId = contentId };
        }

        public ContentType GetContentType(string fileName)
        {
            var ext = System.IO.Path.GetExtension(fileName).ToLower();

            var regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null) {
                return new ContentType(regKey.GetValue("Content Type").ToString());
            }
            return null;
        }
    }
}