namespace Chiffon.Entities
{
    using System.Collections.Generic;

    public interface IDataContext
    {
        IEnumerable<Designer> Designers { get; }
        IEnumerable<Pattern> Patterns { get; }
     }
}
