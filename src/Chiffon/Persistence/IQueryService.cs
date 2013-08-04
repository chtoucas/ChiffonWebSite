namespace Chiffon.Persistence
{
    using Narvalo.Fx;

    public interface IQueryService
    {
        Maybe<QueryResults.MayGetPattern> MayGetPattern(string reference, string designerKey, bool publicOnly);
    }
}
