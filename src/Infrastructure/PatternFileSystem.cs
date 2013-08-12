namespace Chiffon.Infrastructure
{
    using System;
    using System.IO;
    using Chiffon.Infrastructure;

    public class PatternFileSystem
    {
        readonly ChiffonConfig _config;

        public PatternFileSystem(ChiffonConfig config)
        {
            _config = config;
        }

        public void Delete(PatternImage image)
        {
            throw new NotImplementedException();
        }

        public string GetPath(PatternImage image)
        {
            return Path.Combine(_config.PatternDirectory, image.RelativePath);
        }

        public void Save(PatternImage image)
        {
            throw new NotImplementedException();
        }
    }
}
