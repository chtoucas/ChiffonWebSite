namespace Chiffon.WebSite.Handlers
{
    using System;
    using System.ComponentModel.DataAnnotations;

    [Serializable]
    public class PatternImage
    {
        int _id;

        [Required(ErrorMessage = "Id is required.")]
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
    }
}