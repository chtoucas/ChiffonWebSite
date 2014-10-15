namespace Chiffon.Services
{
    using System.Collections.Generic;
    using Chiffon.Entities;

    public interface IPatternService
    {
        IEnumerable<Pattern> GetPatternViews(DesignerKey designerKey, string categoryKey, string reference);
        PreviewPagedList ListPreviews(DesignerKey designerKey, int pageIndex, int pageSize);
        PreviewPagedList ListPreviews(DesignerKey designerKey, string categoryKey, int pageIndex, int pageSize);
    }
}