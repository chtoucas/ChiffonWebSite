namespace Chiffon.Infrastructure
{
    using System;
    using System.IO;
    using Chiffon.Infrastructure;
    using Narvalo;

    public class PatternFileSystem
    {
        readonly ChiffonConfig _config;

        public PatternFileSystem(ChiffonConfig config)
        {
            _config = config;
        }

        //public void Delete(PatternImage image)
        //{
        //    throw new NotImplementedException();
        //}

        public string GetPath(PatternImage image)
        {
            Requires.NotNull(image, "image");

            return Path.Combine(_config.PatternDirectory, image.RelativePath);
        }

        //public void Save(PatternImage image)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
