namespace Chiffon.Data
{
    using Chiffon.Entities;
    using Narvalo.Fx;

    public interface IEntityQueries
    {
        Maybe<Pattern> MayGetPattern(DesignerKey designerKey, string reference);
    }
}