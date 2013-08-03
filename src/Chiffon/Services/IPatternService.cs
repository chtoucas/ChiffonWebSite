namespace Chiffon.Services
{
    using Chiffon.Infrastructure;
    using Narvalo.Fx;

    public interface IPatternService
    {
        Maybe<PatternService_MayFindPatternFileResult> MayFindPatternFile(
            string reference, string designerUrlKey, bool publicOnly);
    }

    public class PatternService_MayFindPatternFileResult
    {
        public string Directory { get; set; }
        public bool IsPublic { get; set; }
        public string Reference { get; set; }
    }
}
