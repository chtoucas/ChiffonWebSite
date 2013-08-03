namespace Chiffon.Domain
{
    using System.Collections.Generic;
    using Narvalo.Fx;

    public interface IDesignerRepository
    {
        IEnumerable<Designer> GetAll();
        Maybe<Designer> MayGetDesigner(DesignerId designerId);
    }
}
