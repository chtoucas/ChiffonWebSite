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
    using Chiffon.Common.Filters;
    using Chiffon.Infrastructure;
    using Chiffon.Mail;
    using Chiffon.Resources;
    using Chiffon.ViewModels;
    using Narvalo;
    using Narvalo.Web.Security;
    using Addressing = Chiffon.Infrastructure.Addressing;

    [AllowAnonymous]
    [CLSCompliant(false)]
    public class AccountController : ChiffonController
    {
        const int RandomPasswordLength_ = 7;
        const string RandomPasswordLetters_ = "abcdefghijkmnpqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ";
        const string RandomPasswordNumbers_ = "23456789";

        //static readonly Regex EmailAddressRegex
        //       = new Regex(@"^[\w\.\-_]+@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$", RegexOptions.Compiled);

        readonly ChiffonConfig _config;
        readonly IFormsAuthenticationService _formsService;

        public AccountController(
            ChiffonEnvironment environment,
            Addressing.ISiteMap siteMap,
            IFormsAuthenticationService formsService,
            ChiffonConfig config)
            : base(environment, siteMap)
        {
            Requires.NotNull(config, "config");

            _config = config;
            _formsService = formsService;
        }

        string ConnectionString_ { get { return _config.SqlConnectionString; } }

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
        public ActionResult Register(RegisterViewModel contact)
        {
            Requires.NotNull(contact, "contact");

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
                return View(Constants.ViewName.Account.Register, contact);
            }

            if (IsEmailAddressAlreadyTaken_(contact.EmailAddress)) {
                return View(Constants.ViewName.Account.RegisterEmailAlreadyTaken);
            }

            LayoutViewModel.MainHeading = SR.Account_Register_MainHeadingOnSuccess;

            if (contact.ReturnUrl == null) { contact.ReturnUrl = String.Empty; }

            var password = CreateContact_(contact);
            string userName = String.Format(CultureInfo.CurrentCulture,
                SR.MailAddressDisplayNameFormat, contact.FirstName, contact.LastName);

            _formsService.SignIn(userName, false /* createPersistentCookie */);

            // Envoi de l'email de confirmation d'inscription après la connection.
            var emailAddress = new MailAddress(contact.EmailAddress, userName);
            var mailMerge = new MailMerge();

            var welcomeMessage
                = mailMerge.Welcome(emailAddress, password, Environment.BaseUri, CultureInfo.CurrentUICulture.TwoLetterISOLanguageName);
            var alertMessage
                = mailMerge.NewMember(emailAddress, contact.FirstName, contact.LastName, contact.CompanyName);

            using (var smtpClient = new SmtpClient()) {
                smtpClient.Send(welcomeMessage);
                smtpClient.Send(alertMessage);
            }

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
            var password = CreateRandomPassword_(RandomPasswordLength_);

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
            var chars = new char[passwordLength];
            var rd = new Random();

            bool useLetter = true;
            for (int i = 0; i < passwordLength; i++) {
                if (useLetter) {
                    chars[i] = RandomPasswordLetters_[rd.Next(0, RandomPasswordLetters_.Length)];
                    useLetter = false;
                }
                else {
                    chars[i] = RandomPasswordNumbers_[rd.Next(0, RandomPasswordNumbers_.Length)];
                    useLetter = true;
                }

            }

            return new String(chars);
        }
    }
}
