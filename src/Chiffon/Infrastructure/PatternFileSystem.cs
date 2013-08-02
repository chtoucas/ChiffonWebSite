namespace Chiffon.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using Chiffon.Crosscuttings;
    using Chiffon.Entities;
    //using Narvalo.Collections;
    using Narvalo.Fx;

    public class PatternFileSystem
    {
        public const string MimeType = "image/jpeg";

        static Dictionary<PatternSize, string> SizeNames_ = new Dictionary<PatternSize, string>() {
            { new PatternSize(200, 160), "apercu"}
        };

        readonly ChiffonConfig _config;

        public PatternFileSystem(ChiffonConfig config)
        {
            _config = config;
        }

        public string GetPath(Pattern pattern, Member member)
        {
            return String.Format(CultureInfo.InvariantCulture, "motif-{0}.jpg", pattern.Id);
        }

        public Maybe<string> GetPath(Pattern pattern, Member member, PatternSize size)
        {
            throw new NotImplementedException();
            //var sizeName = SizeNames_.MayGetValue(size);
            //return String.Format(CultureInfo.InvariantCulture, "motif-{0}_{1}.jpg", pattern.Id, sizeName.Value);
        }

        string GetFilePath_(string directory, string fileName)
        {
            return Path.Combine(_config.PatternDirectory, directory, fileName);
        }
    }
}
