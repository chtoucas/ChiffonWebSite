﻿namespace Chiffon.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Chiffon.Common;
    using Chiffon.Common.Filters;
    using Chiffon.Data;
    using Chiffon.Infrastructure;
    using Chiffon.Infrastructure.Addressing;
    using Chiffon.Resources;
    using Chiffon.ViewModels;
    using Narvalo;

    [SeoPolicy(RobotsDirective = "index, follow")]
    public class HomeController : PageController
    {
        readonly ViewModelQueries _queries;

        public HomeController(ChiffonEnvironment environment, ISiteMap siteMap, ViewModelQueries queries)
            : base(environment, siteMap)
        {
            Requires.NotNull(queries, "queries");

            _queries = queries;
        }

        [HttpGet]
        public ActionResult Index()
        {
            IEnumerable<PatternViewItem> model = _queries.GetHomeViewModel();

            ViewBag.Title = SR.Home_Index_Title;
            ViewBag.MetaDescription = SR.Home_Index_Description;
            ViewBag.CanonicalLink = SiteMap.Home().ToString();

            return View(ViewName.Home.Index, model);
        }

        [HttpGet]
        public ActionResult About()
        {
            ViewBag.Title = SR.Home_About_Title;
            ViewBag.MetaDescription = SR.Home_About_Description;
            ViewBag.CanonicalLink = SiteMap.About().ToString();

            return View(ViewName.Home.About);
        }

        [HttpGet]
        public ActionResult Contact()
        {
            ViewBag.Title = SR.Home_Contact_Title;
            ViewBag.MetaDescription = SR.Home_Contact_Description;
            ViewBag.CanonicalLink = SiteMap.Contact().ToString();

            return View(ViewName.Home.Contact);
        }

        [HttpGet]
        public ActionResult Newsletter()
        {
            ViewBag.Title = SR.Home_Newsletter_Title;
            ViewBag.MetaDescription = SR.Home_Newsletter_Description;
            ViewBag.CanonicalLink = SiteMap.Newsletter().ToString();

            return View(ViewName.Home.Newsletter);
        }
    }
}
