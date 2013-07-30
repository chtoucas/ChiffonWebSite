namespace Chiffon.CrossCuttings.Configuration
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Linq;
    using Narvalo.Configuration;

    public class QuadernoSettings
    {
        string[] _xmlArchives;
        string _xmlPath;

        public string[] XmlArchives
        {
            get { return _xmlArchives; }
            set { _xmlArchives = value; }
        }

        public string XmlPath
        {
            get { return _xmlPath; }
            set { _xmlPath = value; }
        }

        public static QuadernoSettings Load()
        {
            return Load(LoadSection_());
        }

        public static QuadernoSettings Load(QuadernoSection section)
        {
            Requires.NotNull(section, "section");

            var directory = section.XmlStore.Directory ?? GetDefaultDirectory_();

            var xmlPath = GetAbsolutePath_(directory, section.XmlStore.Path);

            var archives = section.XmlStore.Archives;
            int capacity = archives.Count;
            var xmlArchives = new string[capacity];
            if (capacity > 0) {
                var list = new ArrayList(capacity);
                foreach (var item in archives.OrderBy(_ => _.EndDate)) {
                    list.Add(GetAbsolutePath_(directory, item.Path));
                }
                list.CopyTo(xmlArchives);
            }

            return new QuadernoSettings {
                XmlArchives = xmlArchives,
                XmlPath = xmlPath,
            };
        }

        #region Membres privés

        static QuadernoSection LoadSection_()
        {
            return ConfigurationSectionManager.GetSection<QuadernoSection>(QuadernoSection.DefaultName);
        }

        static string GetAbsolutePath_(string directory, string path)
        {
            return Path.IsPathRooted(path)
                 ? path
                 : Path.Combine(directory, path);
        }

        static string GetDefaultDirectory_()
        {
            return Path.Combine(Environment.CurrentDirectory, "App_Data");
        }

        #endregion
    }
}
