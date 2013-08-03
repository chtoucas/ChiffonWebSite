namespace Chiffon.Application
{
    using System.Linq;
    using Chiffon.Domain;
    using Chiffon.Infrastructure;
    using Narvalo;
    using Narvalo.Collections;
    using Narvalo.Fx;

    public class PatternServiceImpl : IPatternService
    {
        readonly IPatternRepository _patternRepository;
        readonly IDesignerRepository _designerRepository;

        public PatternServiceImpl(
            IPatternRepository patternRepository,
            IDesignerRepository designerRepository)
            : base()
        {
            Requires.NotNull(patternRepository, "patternRepository");
            Requires.NotNull(designerRepository, "designerRepository");

            _patternRepository = patternRepository;
            _designerRepository = designerRepository;
        }

        public Maybe<PatternFile> FindPatternFile(string reference, string designerUrlKey)
        {
            // select
            //  D.pattern_directory as directory
            //  , P.is_public       as is_public
            //  , P.reference       as reference
            // from Patterns as P
            //  inner join Designers as D on P.designer_id = D.id
            // where P.reference = reference
            //  and D.urlKey = designer_key

            var q = from p in _patternRepository.GetAll()
                    join d in _designerRepository.GetAll() on p.DesignerId equals d.DesignerId
                    where p.Reference == reference
                        && d.UrlKey == designerUrlKey
                    select new PatternFile { 
                        Directory = d.PatternDirectory, 
                        IsPublic = p.IsPublic,
                        Reference = p.Reference 
                    };

            return q.SingleOrNone();
        }
    }
}
