namespace Chiffon.Services
{
    using System.Collections.Generic;
    using Chiffon.Entities;

    public interface IDataContext
    {
        IEnumerable<Designer> Designers { get; }

        IEnumerable<Pattern> Patterns { get; }
     }
}
