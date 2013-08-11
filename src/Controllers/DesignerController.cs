namespace Chiffon.Controllers
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Web.Mvc;
    using Chiffon.Common;
    using Chiffon.Entities;
    using Chiffon.Infrastructure;
    using Chiffon.ViewModels;
    using Narvalo;
    using Narvalo.Data;

    public partial class DesignerController : BaseController
    {
        [HttpGet]
        public virtual ActionResult Index(string designer)
        {
            return View(ViewPath.Designer.Index);
        }

        [HttpGet]
        public virtual ActionResult Pattern(string designer, string reference)
        {
            return View(ViewPath.Designer.Pattern);
        }
    }
}
