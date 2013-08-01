namespace Chiffon.WebSite.Handlers
{
    using System;
    using System.ComponentModel.DataAnnotations;

    [Serializable]
    public class PatternPreviewQuery
    {
        public static readonly string HeightKey = "height";
        public static readonly string IdKey = "id";
        public static readonly string WidthKey = "width";

        int _height;
        int _id;
        int _width;

        [Required(ErrorMessage = "Height is required.")]
        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }

        [Required(ErrorMessage = "Id is required.")]
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        [Required(ErrorMessage = "Height is required.")]
        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }
    }
}
