namespace Chiffon.Controllers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using System.Web.Mvc;
    using Chiffon.Common;
    using Chiffon.Infrastructure;
    using Chiffon.Infrastructure.Addressing;
    using Chiffon.Resources;
    using Chiffon.Services;
    using Chiffon.ViewModels;
    using Narvalo;
    using Narvalo.Linq;

    public class AccountController : ChiffonController
    {
        readonly IMemberService _memberService;

        public AccountController(ChiffonEnvironment environment, ISiteMap siteMap, IMemberService memberService)
            : base(environment, siteMap)
        {
            Require.NotNull(memberService, "memberService");

            _memberService = memberService;
        }

        [HttpGet]
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "0#")]
        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Login")]
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
            LayoutViewModel.MainMenuCssClass = "login";

            return View(Constants.ViewName.Account.Login, model);
        }

        [HttpGet]
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "0#")]
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
            LayoutViewModel.MainMenuCssClass = "register";

            return View(Constants.ViewName.Account.Register, model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            Require.NotNull(model, "model");

            if (User.Identity.IsAuthenticated) {
                return RedirectToHome();
            }

            // Ontologie.
            Ontology.Title = SR.Account_Register_Title;
            Ontology.Description = SR.Account_Register_Description;
            Ontology.Relationships.CanonicalUrl = SiteMap.Register();

            // LayoutViewModel.
            LayoutViewModel.AddAlternateUrls(Environment.Language, _ => _.Register());
            LayoutViewModel.MainMenuCssClass = "register";
            LayoutViewModel.MainHeading = SR.Account_Register_MainHeading;

            if (!ModelState.IsValid) {
                return View(Constants.ViewName.Account.Register, model);
            }

            _memberService.MemberCreated += (sender, e) =>
            {
                (new AuthentificationService(HttpContext)).SignIn(e.Member);
            };

            var result = _memberService.RegisterMember(new RegisterMemberRequest {
                CompanyName = model.CompanyName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                NewsletterChecked = FormUtility.IsCheckBoxOn(model.Newsletter),
            });

            if (result.IsBreak) {
                return View(Constants.ViewName.Account.RegisterFailure, new RegisterFailureViewModel {
                    Message = result.Message
                });
            }

            string returnUrl = (from _ in MayParseTo.Uri(model.ReturnUrl, UriKind.Relative) select _.ToString())
                .ValueOrElse(String.Empty);

            return RedirectToRoute(Constants.RouteName.Account.RegisterSuccess, new { returnUrl = returnUrl });
        }

        [HttpGet]
        [Authorize]
        [OntologyFilter(Disabled = true)]
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "0#")]
        public ActionResult RegisterSuccess(string returnUrl)
        {
            if (!User.Identity.IsAuthenticated) {
                return RedirectToRoute(Constants.RouteName.Account.Register);
            }

            string nextUrl = (from _ in MayParseTo.Uri(returnUrl, UriKind.Relative) select _.ToString())
                .ValueOrElse(Url.RouteUrl(Constants.RouteName.Home.Index));

            // LayoutViewModel.
            LayoutViewModel.AddAlternateUrls(Environment.Language, _ => _.Register());
            LayoutViewModel.MainHeading = SR.Account_RegisterSuccess_MainHeading;
            LayoutViewModel.MainMenuCssClass = "register";

            return View(Constants.ViewName.Account.RegisterSuccess, new RegisterSuccessViewModel { NextUrl = nextUrl });
        }

        [HttpGet]
        public ActionResult Newsletter()
        {
            // Ontologie.
            Ontology.Title = SR.Account_Newsletter_Title;
            Ontology.Description = SR.Account_Newsletter_Description;
            Ontology.Relationships.CanonicalUrl = SiteMap.Newsletter();

            // LayoutViewModel.
            LayoutViewModel.AddAlternateUrls(Environment.Language, _ => _.Newsletter());
            LayoutViewModel.MainHeading = SR.Account_Newsletter_MainHeading;
            LayoutViewModel.MainMenuCssClass = "newsletter";

            return View(Constants.ViewName.Account.Newsletter);
        }
    }
}
