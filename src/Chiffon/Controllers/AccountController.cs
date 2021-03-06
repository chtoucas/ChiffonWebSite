﻿namespace Chiffon.Controllers
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Web;
    using System.Web.Mvc;

    using Chiffon.Common;
    using Chiffon.Services;
    using Chiffon.Views;
    using Narvalo;

    public sealed class AccountController : ChiffonController
    {
        private readonly IMemberService _memberService;

        public AccountController(ChiffonEnvironment environment, ISiteMap siteMap, IMemberService memberService)
            : base(environment, siteMap)
        {
            Require.NotNull(memberService, "memberService");
            Contract.Requires(siteMap != null);

            _memberService = memberService;
        }

        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToHome();
            }

#if SHOWCASE
            return Redirect("~/go?targetUrl=" + returnUrl);
#else
            // Modèle.
            var model = new LoginViewModel { ReturnUrl = returnUrl };

            // Ontologie.
            Ontology.Title = Strings.Account_Login_Title;
            Ontology.Description = Strings.Account_Login_Description;
            Ontology.Relationships.CanonicalUrl = SiteMap.Login();

            // LayoutViewModel.
            LayoutViewModel.AddAlternateUrls(Environment.Language, _ => _.Login());
            LayoutViewModel.MainHeading = Strings.Account_Login_MainHeading;
            LayoutViewModel.MainMenuCssClass = "login";

            return View(Constants.ViewName.Account.Login, model);
#endif
        }

        [HttpGet]
        public ActionResult Register(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToHome();
            }
            
#if SHOWCASE
            return Redirect("~/go?targetUrl=" + returnUrl);
#else
            // Modèle.
            var model = new RegisterViewModel {
                ReturnUrl = returnUrl,
            };

            // Ontologie.
            Ontology.Title = Strings.Account_Register_Title;
            Ontology.Description = Strings.Account_Register_Description;
            Ontology.Relationships.CanonicalUrl = SiteMap.Register();

            // LayoutViewModel.
            LayoutViewModel.AddAlternateUrls(Environment.Language, _ => _.Register());
            LayoutViewModel.MainHeading = Strings.Account_Register_MainHeading;
            LayoutViewModel.MainMenuCssClass = "register";

            return View(Constants.ViewName.Account.Register, model);
#endif
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            Require.NotNull(model, "model");

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToHome();
            }

            // Ontologie.
            Ontology.Title = Strings.Account_Register_Title;
            Ontology.Description = Strings.Account_Register_Description;
            Ontology.Relationships.CanonicalUrl = SiteMap.Register();

            // LayoutViewModel.
            LayoutViewModel.AddAlternateUrls(Environment.Language, _ => _.Register());
            LayoutViewModel.MainMenuCssClass = "register";
            LayoutViewModel.MainHeading = Strings.Account_Register_MainHeading;

            if (!ModelState.IsValid)
            {
                return View(Constants.ViewName.Account.Register, model);
            }

            _memberService.MemberCreated += (sender, e) =>
            {
                new AuthenticationService(HttpContext).SignIn(e.Member);
            };

            var result = _memberService.RegisterMember(new RegisterMemberRequest {
                CompanyName = model.CompanyName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                NewsletterChecked = IsCheckBoxOn_(model.Newsletter),
            });

            if (result.IsBreak)
            {
                return View(
                    Constants.ViewName.Account.RegisterFailure,
                    new RegisterFailureViewModel {
                        Message = result.Reason
                    });
            }

            string returnUrl = (from _ in ParseTo.Uri(model.ReturnUrl, UriKind.Relative) select _.ToString())
                .ValueOrElse(String.Empty);

            return RedirectToRoute(Constants.RouteName.Account.RegisterSuccess, new { returnUrl = returnUrl });
        }

        [HttpGet]
        [Authorize]
        [OntologyFilter(Disabled = true)]
        public ActionResult RegisterSuccess(string returnUrl)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToRoute(Constants.RouteName.Account.Register);
            }

            string nextUrl = (from _ in ParseTo.Uri(returnUrl, UriKind.Relative) select _.ToString())
                .ValueOrElse(Url.RouteUrl(Constants.RouteName.Home.Index));

            // LayoutViewModel.
            LayoutViewModel.AddAlternateUrls(Environment.Language, _ => _.Register());
            LayoutViewModel.MainHeading = Strings.Account_RegisterSuccess_MainHeading;
            LayoutViewModel.MainMenuCssClass = "register";

            return View(Constants.ViewName.Account.RegisterSuccess, new RegisterSuccessViewModel { NextUrl = nextUrl });
        }

        private static bool IsCheckBoxOn_(string value)
        {
            return String.IsNullOrEmpty(value) ? false : (value == "on");
        }
    }
}
