namespace Chiffon.WebSite.Handlers
{
    using Chiffon.Entities;

    public class PatternPreview
    {
        int _height;
        string _id;
        int _width;
        string _memberKey;

        public string MemberKey
        {
            get { return _memberKey; }
            set { _memberKey = value; }
        }

        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }

        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }
    }
}
