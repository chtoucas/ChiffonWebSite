namespace Chiffon.Application
{
    using Chiffon.Entities;
    using Narvalo.Fx;

    public interface IPatternService
    {
        Maybe<FindPatternDto> FindPattern(string id, string memberKey);
    }

    public class FindPatternDto
    {
        public Pattern Pattern { get; set; }
        public Member Member { get; set; }
    }
}
