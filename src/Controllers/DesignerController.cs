namespace Chiffon.Controllers
{
    using System.Web.Mvc;
    using Chiffon.Common;

    public partial class DesignerController : Controller
    {
        [HttpGet]
        public virtual ActionResult Index(string designer)
        {
            return View(ViewName.Designer.Index);
        }

        [HttpGet]
        public virtual ActionResult Pattern(string designer, string reference)
        {
            return View(ViewName.Designer.Pattern);
        }
    }
}
