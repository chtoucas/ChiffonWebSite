namespace Chiffon.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using Chiffon.Crosscuttings;
    using Narvalo.Collections;
    using Narvalo.Fx;

    public class PatternFileSystem
    {
        static Dictionary<PatternSize, string> SizeNames_ = new Dictionary<PatternSize, string>() {
            { new PatternSize(200, 160), "apercu"}
        };

        readonly ChiffonConfig _config;

        public PatternFileSystem(ChiffonConfig config)
        {
            _config = config;
        }

        public string GetPath(PatternFile pattern)
        {
            return GetAbsolutePath_(pattern.Directory, GetFilename_(pattern.Reference));
        }

        public Maybe<string> GetPath(PatternFile pattern, PatternSize size)
        {
            return SizeNames_
                .MayGetValue(size)
                .Map(_ => GetFilename_(pattern.Reference, _))
                .Map(_ => GetAbsolutePath_(pattern.Directory, _));
        }

        #region > Méthodes privées <

        string GetAbsolutePath_(string directory, string fileName)
        {
            return Path.Combine(_config.PatternDirectory, directory, fileName);
        }

        string GetFilename_(string reference)
        {
            return String.Format(CultureInfo.InvariantCulture, "motif-{0}.jpg", reference);
        }

        string GetFilename_(string reference, string sizeName)
        {
            return String.Format(CultureInfo.InvariantCulture, "motif-{0}_{1}.jpg", reference, sizeName);
        }

        #endregion
    }
}
