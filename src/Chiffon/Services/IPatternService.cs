namespace Chiffon.Services
{
    using System.Collections.Generic;

    using Chiffon.Entities;

    public partial interface IPatternService
    {
        IEnumerable<Pattern> GetPatternViews(DesignerKey designerKey, string categoryKey, string reference);

        PreviewPagedList ListPreviews(DesignerKey designerKey, int pageIndex, int pageSize);

        PreviewPagedList ListPreviews(DesignerKey designerKey, string categoryKey, int pageIndex, int pageSize);
    }
}

#if CONTRACTS_FULL // Contract Class and Object Invariants.

namespace Chiffon.Services
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;

    using Chiffon.Entities;

    [ContractClass(typeof(IPatternServiceContract))]
    public partial interface IPatternService { }

    [ContractClassFor(typeof(IPatternService))]
    internal abstract class IPatternServiceContract : IPatternService
    {
        IEnumerable<Pattern> IPatternService.GetPatternViews(DesignerKey designerKey, string categoryKey, string reference)
        {
            Contract.Ensures(Contract.Result<IEnumerable<Pattern>>() != null);

            return Enumerable.Empty<Pattern>();
        }

        PreviewPagedList IPatternService.ListPreviews(DesignerKey designerKey, int pageIndex, int pageSize)
        {
            return default(PreviewPagedList);
        }

        PreviewPagedList IPatternService.ListPreviews(DesignerKey designerKey, string categoryKey, int pageIndex, int pageSize)
        {
            return default(PreviewPagedList);
        }
    }
}

#endif
