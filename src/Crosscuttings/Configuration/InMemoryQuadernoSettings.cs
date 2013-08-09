namespace Chiffon.CrossCuttings.Configuration
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Linq;

    public class InMemoryQuadernoSettings
    {
        readonly QuadernoSection _section;

        //string[] _xmlArchives;
        //string _xmlPath;

        public InMemoryQuadernoSettings(QuadernoSection section)
        {
            Requires.NotNull(section, "section");

            _section = section;
        }

        //public string[] XmlArchives
        //{
        //    get { return _xmlArchives; }
        //    set { _xmlArchives = value; }
        //}

        //public string XmlPath
        //{
        //    get { return _xmlPath; }
        //    set { _xmlPath = value; }
        //}

        public void Initialize()
        {
            InitializeXmlStore_();
        }

        #region Membres privés

        string GetAbsolutePath_(string directory, string path)
        {
            return Path.IsPathRooted(path)
                 ? path
                 : Path.Combine(directory, path);
        }

        string GetDefaultDirectory_()
        {
            return Path.Combine(Environment.CurrentDirectory, "App_Data");
        }

        void InitializeXmlStore_()
        {
            var directory = _section.XmlStore.Directory ?? GetDefaultDirectory_();

            _xmlPath = GetAbsolutePath_(directory, _section.XmlStore.Path);

            var archives = _section.XmlStore.Archives;

            int capacity = archives.Count;
            _xmlArchives = new string[capacity];
            if (capacity > 0) {
                var list = new ArrayList(capacity);
                foreach (var item in archives.OrderBy(_ => _.EndDate)) {
                    list.Add(GetAbsolutePath_(directory, item.Path));
                }
                list.CopyTo(_xmlArchives);
            }
        }

        #endregion
    }
}
