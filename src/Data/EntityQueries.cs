namespace Chiffon.Data
{
    using Chiffon.Data.SqlServer.EntityQueries;
    using Chiffon.Entities;
    using Chiffon.Infrastructure;
    using Narvalo.Fx;

    public class EntityQueries : IEntityQueries
    {
        readonly ChiffonConfig _config;

        public EntityQueries(ChiffonConfig config)
        {
            _config = config;
        }

        protected string ConnectionString { get { return _config.SqlConnectionString; } }

        public Maybe<Pattern> MayGetPattern(DesignerKey designerKey, string reference)
        {
            var query = new MayGetPatternQuery(ConnectionString) {
                DesignerKey = designerKey,
                Reference = reference,
            };
            return query.Execute();
        }
    }
}
