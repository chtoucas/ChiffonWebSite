namespace Chiffon.Infrastructure
{
    using System;
    using System.IO;
    using Chiffon.Crosscuttings;

    public class PatternFileSystem
    {
        readonly ChiffonConfig _config;

        public PatternFileSystem(ChiffonConfig config)
        {
            _config = config;
        }

        public void Delete(PatternFile file)
        {
            throw new NotImplementedException();
        }

        public string GetPath(PatternFile file)
        {
            return Path.Combine(_config.PatternDirectory, file.RelativePath);
        }

        public void Save(PatternFile file)
        {
            throw new NotImplementedException();
        }
    }
}
