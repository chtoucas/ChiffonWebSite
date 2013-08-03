namespace Chiffon.Application
{
    using Chiffon.Infrastructure;
    using Narvalo.Fx;

    public interface IPatternService
    {
        Maybe<PatternFile> FindPatternFile(string reference, string designerUrlKey);
    }
}
