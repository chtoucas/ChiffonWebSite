namespace Chiffon.Application
{
    using System.Linq;
    using Chiffon.Entities;
    using Narvalo;
    using Narvalo.Fx;

    public class PatternServiceImpl : IPatternService
    {
        readonly IPatternRepository _patternRepository;
        readonly IMemberRepository _memberRepository;

        public PatternServiceImpl(
            IPatternRepository patternRepository,
            IMemberRepository memberRepository)
            : base()
        {
            Requires.NotNull(patternRepository, "patternRepository");
            Requires.NotNull(memberRepository, "memberRepository");

            _patternRepository = patternRepository;
            _memberRepository = memberRepository;
        }

        public Maybe<FindPatternDto> FindPattern(string id, string memberKey)
        {
            var q = from m in _memberRepository.GetAll()
                    let p = _patternRepository.GetPattern(id)
                    where m.UrlKey == memberKey && m.MemberId == p.MemberId
                    select new FindPatternDto {
                        Member = m,
                        Pattern = p
                    };

            return Maybe.Create(q.SingleOrDefault());
        }
    }
}
