namespace Chiffon.Controllers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using System.Web.Mvc;
    using Chiffon.Common;
    using Chiffon.Common.Filters;
    using Chiffon.Infrastructure;
    using Chiffon.Resources;
    using Chiffon.Services;
    using Chiffon.ViewModels;
    using Narvalo;
    using Addressing = Chiffon.Infrastructure.Addressing;

    [AllowAnonymous]
    [CLSCompliant(false)]
    public class AccountController : ChiffonController
    {
        readonly IMemberService _memberService;

        public AccountController(ChiffonEnvironment environment, Addressing.ISiteMap siteMap, IMemberService memberService)
            : base(environment, siteMap)
        {
            Requires.NotNull(memberService, "memberService");

            _memberService = memberService;
        }

        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "0#")]
        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Login")]
        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated) {
                return RedirectToHome();
            }

            // Modèle.
            var model = new LoginViewModel { ReturnUrl = returnUrl };

            // Ontologie.
            Ontology.Title = SR.Account_Login_Title;
            Ontology.Description = SR.Account_Login_Description;
            Ontology.Relationships.CanonicalUrl = SiteMap.Login();

            // LayoutViewModel.
            LayoutViewModel.AddAlternateUrls(Environment.Language, _ => _.Login());
            LayoutViewModel.MainHeading = SR.Account_Login_MainHeading;
            LayoutViewModel.MainNavCssClass = "login";

            return View(Constants.ViewName.Account.Login, model);
        }

        [HttpGet]
        public ActionResult Register(string returnUrl)
        {
            if (User.Identity.IsAuthenticated) {
                return RedirectToHome();
            }

            // Modèle.
            var model = new RegisterViewModel {
                ReturnUrl = returnUrl,
            };

            // Ontologie.
            Ontology.Title = SR.Account_Register_Title;
            Ontology.Description = SR.Account_Register_Description;
            Ontology.Relationships.CanonicalUrl = SiteMap.Register();

            // LayoutViewModel.
            LayoutViewModel.AddAlternateUrls(Environment.Language, _ => _.Register());
            LayoutViewModel.MainHeading = SR.Account_Register_MainHeading;
            LayoutViewModel.MainNavCssClass = "register";

            return View(Constants.ViewName.Account.Register, model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            Requires.NotNull(model, "model");

            if (User.Identity.IsAuthenticated) {
                return RedirectToHome();
            }

            // Ontologie.
            Ontology.Title = SR.Account_Register_Title;
            Ontology.Description = SR.Account_Register_Description;
            Ontology.Relationships.CanonicalUrl = SiteMap.Register();

            // LayoutViewModel.
            LayoutViewModel.AddAlternateUrls(Environment.Language, _ => _.Register());
            LayoutViewModel.MainNavCssClass = "register";
            LayoutViewModel.MainHeading = SR.Account_Register_MainHeading;

            if (!ModelState.IsValid) {
                return View(Constants.ViewName.Account.Register, model);
            }

            _memberService.MemberCreated += (object sender, MemberCreatedEventArgs e) =>
            {
                (new AuthentificationService(HttpContext)).SignIn(e.Member);
            };

            var outcome = _memberService.RegisterMember(new RegisterMemberQuery {
                CompanyName = model.CompanyName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                NewsletterChecked = FormUtility.IsCheckboxOn(model.Newsletter),
            });

            if (outcome.Unsuccessful) {
                return View(Constants.ViewName.Account.RegisterFailure, new RegisterFailureViewModel {
                    Message = outcome.Error.Message
                });
            }

            string returnUrl = MayParse.ToUri(model.ReturnUrl, UriKind.Relative)
                .Map(_ => _.ToString())
                .ValueOrElse(String.Empty);

            if (returnUrl.Length > 0) {
                return RedirectToRoute(Constants.RouteName.Account.RegisterSuccess, new { returnUrl = returnUrl });
            }
            else {
                return RedirectToRoute(Constants.RouteName.Account.RegisterSuccess);
            }
        }

        [HttpGet]
        public ActionResult RegisterSuccess(string returnUrl)
        {
            if (!User.Identity.IsAuthenticated) {
                return new RedirectResult(Url.RouteUrl(Constants.RouteName.Account.Register));
            }

            string nextUrl = MayParse.ToUri(returnUrl, UriKind.Relative)
                .Map(_ => _.ToString())
                .ValueOrElse(Url.RouteUrl(Constants.RouteName.Home.Index));

            // Ontologie.
            Ontology.Relationships.CanonicalUrl = SiteMap.RegisterSuccess();

            // LayoutViewModel.
            LayoutViewModel.AddAlternateUrls(Environment.Language, _ => _.Register());
            LayoutViewModel.MainHeading = SR.Account_RegisterSuccess_MainHeading;
            LayoutViewModel.MainNavCssClass = "register";

            return View(Constants.ViewName.Account.RegisterSuccess, new RegisterSuccessViewModel { NextUrl = nextUrl });
        }

        [HttpGet]
        [OntologyFilter(RobotsDirective = "index, follow")]
        public ActionResult Newsletter()
        {
            // Ontologie.
            Ontology.Title = SR.Account_Newsletter_Title;
            Ontology.Description = SR.Account_Newsletter_Description;
            Ontology.Relationships.CanonicalUrl = SiteMap.Newsletter();

            // LayoutViewModel.
            LayoutViewModel.AddAlternateUrls(Environment.Language, _ => _.Newsletter());
            LayoutViewModel.MainHeading = SR.Account_Newsletter_MainHeading;
            LayoutViewModel.MainNavCssClass = "newsletter";

            return View(Constants.ViewName.Account.Newsletter);
        }
    }
}
