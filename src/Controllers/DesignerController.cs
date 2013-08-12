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

    public partial class DesignerController : Controller
    {
        [HttpGet]
        public virtual ActionResult Index(string designer)
        {
            return View(ViewPath.Index);
        }

        [HttpGet]
        public virtual ActionResult Pattern(string designer, string reference)
        {
            return View(ViewPath.Pattern);
        }

        static class ViewPath
        {
            public const string Index = "~/Views/Member/Index.cshtml";
            public const string Pattern = "~/Views/Member/Pattern.cshtml";
        }
    }
}
