namespace Chiffon.Persistence.InMemory
{
    using Chiffon.Entities;

    public class PatternRepositoryImpl : IPatternRepository
    {
        #region IPatternRepository

        public Pattern GetPattern(string id)
        {
            return new Pattern {
                Id = id,
                IsPublic = true
            };
        }

        #endregion
    }
}
