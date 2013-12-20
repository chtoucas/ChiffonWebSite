namespace Chiffon.Controllers
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Net.Mail;
    //using System.Text.RegularExpressions;
    using System.Web.Mvc;
    using Chiffon.Common;
    using Chiffon.Infrastructure;
    using Chiffon.Mail;
    using Chiffon.Resources;
    using Chiffon.ViewModels;
    using Narvalo;
    using Narvalo.Web.Security;
    using Addressing = Chiffon.Infrastructure.Addressing;

    // FIXME: Cette classe est complètement bancale mais c'est voulu tant qu'on n'a pas 
    // une idée plus précise du processus d'inscription.
    // Si l'utilisateur est déconnecté dégager le visiteur.
    [AllowAnonymous]
    [CLSCompliant(false)]
    public class AccountController : ChiffonController
    {
        //static readonly Regex EmailAddressRegex
        //       = new Regex(@"^[\w\.\-_]+@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$", RegexOptions.Compiled);

        readonly ChiffonConfig _config;
        //readonly ISmtpClient _smtpClient;
        readonly IFormsAuthenticationService _formsService;
        //readonly IMemberService _memberService;

        public AccountController(
            ChiffonEnvironment environment,
            //ISmtpClient smtpClient,
            Addressing.ISiteMap siteMap,
            //IMemberService memberService,
            IFormsAuthenticationService formsService,
            ChiffonConfig config)
            : base(environment, siteMap)
        {
            Requires.NotNull(config, "config");
            //Requires.NotNull(memberService, "memberService");

            _config = config;
            //_smtpClient = smtpClient;
            _formsService = formsService;
            //_memberService = memberService;
        }

        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "0#")]
        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Login")]
        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated) {
                return new RedirectResult(Url.RouteUrl(Constants.RouteName.Home.Index));
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

            //if (Request.IsAjaxRequest()) {
            //    return PartialView(Constants.ViewName.Account.Login, model);
            //}
            //else {
            return View(Constants.ViewName.Account.Login, model);
            //}
        }

        [HttpGet]
        public ActionResult Register(string returnUrl)
        {
            if (User.Identity.IsAuthenticated) {
                return new RedirectResult(Url.RouteUrl(Constants.RouteName.Home.Index));
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

            //if (Request.IsAjaxRequest()) {
            //    return PartialView(Constants.ViewName.Account.Register, model);
            //}
            //else {
            return View(Constants.ViewName.Account.Register, model);
            //}
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel contact)
        {
            Requires.NotNull(contact, "contact");

            if (User.Identity.IsAuthenticated) {
                return new RedirectResult(Url.RouteUrl(Constants.RouteName.Home.Index));
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
                return View(Constants.ViewName.Account.Register, contact);
            }

            if (IsEmailAddressAlreadyTaken_(contact.EmailAddress)) {
                return View(Constants.ViewName.Account.RegisterEmailAlreadyTaken);
            }

            LayoutViewModel.MainHeading = SR.Account_Register_MainHeadingOnSuccess;

            // FIXME:
            //if (contact.Message == null) { contact.Message = String.Empty; }
            if (contact.ReturnUrl == null) { contact.ReturnUrl = String.Empty; }

            var password = CreateContact_(contact);

            // Envoi de l'email de confirmation d'inscription.
            // FIXME: Utiliser LastName, FirstName pour les anglishes.
            var emailAddress = new MailAddress(
                contact.EmailAddress, contact.FirstName + " " + contact.LastName);

            // FIXME: 
            string userName = contact.FirstName + " " + contact.LastName;
            _formsService.SignIn(userName, false /* createPersistentCookie */);

            // On envoie le mail après la connection.
            var mailMerge = new MailMerge();

            var welcomeMessage
                = mailMerge.Welcome(emailAddress, password, Environment.BaseUri, CultureInfo.CurrentUICulture.TwoLetterISOLanguageName);
            var alertMessage
                = mailMerge.NewMember(emailAddress, contact.FirstName, contact.LastName, contact.CompanyName);

            //_smtpClient.Send(message);
            using (var smtpClient = new SmtpClient()) {
                smtpClient.Send(welcomeMessage);
                smtpClient.Send(alertMessage);
            }

            // FIXME: vérifier le contenu de l'URL.
            var nextUrl = MayParse.ToUri(contact.ReturnUrl, UriKind.Relative);
            if (nextUrl.IsSome) {
                return RedirectToRoute(Constants.RouteName.Account.RegisterConfirmation, new
                {
                    nextUrl = nextUrl.Value.ToString()
                });
            }
            else {
                return RedirectToRoute(Constants.RouteName.Account.RegisterConfirmation);
            }
        }

        [HttpGet]
        public ActionResult RegisterConfirmation(string nextUrl)
        {
            if (!User.Identity.IsAuthenticated) {
                return new RedirectResult(Url.RouteUrl(Constants.RouteName.Account.Register));
            }

            if (String.IsNullOrEmpty(nextUrl)) {
                nextUrl = Url.RouteUrl(Constants.RouteName.Home.Index);
            }

            // Ontologie.
            Ontology.Relationships.CanonicalUrl = SiteMap.RegisterConfirmation();

            // LayoutViewModel.
            LayoutViewModel.AddAlternateUrls(Environment.Language, _ => _.Register());
            LayoutViewModel.MainHeading = SR.Account_RegisterConfirmation_MainHeading;
            LayoutViewModel.MainNavCssClass = "register";

            return View(Constants.ViewName.Account.RegisterConfirmation, new RegisterConfirmationViewModel { NextUrl = nextUrl });
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
            LayoutViewModel.MainNavCssClass = "newsletter";

            return View(Constants.ViewName.Account.Newsletter);
        }

        // FIXME:

        string ConnectionString_ { get { return _config.SqlConnectionString; } }

        //static bool IsValidEmailAddress_(string value)
        //{
        //    return !String.IsNullOrWhiteSpace(value)
        //        && IsValidDotNetMailAddress(value)
        //        && EmailAddressRegex.IsMatch(value);
        //}

        //static bool IsValidDotNetMailAddress(string value)
        //{
        //    MailAddress address = null;

        //    try {
        //        address = new MailAddress(value);
        //    }
        //    catch (FormatException) {
        //    }

        //    return address != null;
        //}

        bool IsEmailAddressAlreadyTaken_(string emailAddress)
        {
            bool exists = false;

            using (var cnx = new SqlConnection(ConnectionString_)) {
                using (var cmd = new SqlCommand()) {
                    cmd.CommandText = "usp_GetPublicKeyByEmailAddress";
                    cmd.Connection = cnx;
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameterCollection p = cmd.Parameters;
                    p.Add("@email_address", SqlDbType.NVarChar).Value = emailAddress;

                    cnx.Open();
                    using (var rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)) {
                        if (rdr.Read()) {
                            exists = true;
                        }
                    }
                }
            }

            return exists;
        }

        string CreateContact_(RegisterViewModel contact)
        {
            var password = CreateRandomPassword_(7);

            using (var cnx = new SqlConnection(ConnectionString_)) {
                using (var cmd = new SqlCommand()) {
                    cmd.CommandText = "usp_NewContact";
                    cmd.Connection = cnx;
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameterCollection p = cmd.Parameters;
                    p.Add("@email_address", SqlDbType.NVarChar).Value = contact.EmailAddress;
                    p.Add("@firstname", SqlDbType.NVarChar).Value = contact.FirstName;
                    p.Add("@lastname", SqlDbType.NVarChar).Value = contact.LastName;
                    p.Add("@company_name", SqlDbType.NVarChar).Value = contact.CompanyName;
                    p.Add("@password", SqlDbType.NVarChar).Value = password;
                    //p.Add("@message", SqlDbType.NVarChar).Value = contact.Message;

                    cnx.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            return password;
        }

        // Cf. http://madskristensen.net/post/Generate-random-password-in-C.aspx
        // Cf. http://stackoverflow.com/questions/54991/generating-random-passwords
        static string CreateRandomPassword_(int passwordLength)
        {
            string allowedLetterChars = "abcdefghijkmnpqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ";
            string allowedNumberChars = "23456789";
            char[] chars = new char[passwordLength];
            Random rd = new Random();

            bool useLetter = true;
            for (int i = 0; i < passwordLength; i++) {
                if (useLetter) {
                    chars[i] = allowedLetterChars[rd.Next(0, allowedLetterChars.Length)];
                    useLetter = false;
                }
                else {
                    chars[i] = allowedNumberChars[rd.Next(0, allowedNumberChars.Length)];
                    useLetter = true;
                }

            }

            return new string(chars);
        }
    }
}
