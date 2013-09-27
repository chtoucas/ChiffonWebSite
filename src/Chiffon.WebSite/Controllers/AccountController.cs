namespace Chiffon.Controllers
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    //using System.Net.Mail;
    //using System.Text.RegularExpressions;
    using System.Web.Mvc;
    using Chiffon.Common;
    using Chiffon.Infrastructure;
    using Chiffon.Infrastructure.Addressing;
    using Chiffon.Resources;
    using Chiffon.Services;
    using Chiffon.ViewModels;
    using Narvalo;
    using Narvalo.Web.Security;

    [AllowAnonymous]
    public class AccountController : PageController
    {
        //static readonly Regex EmailAddressRegex
        //       = new Regex(@"^[\w\.\-_]+@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$", RegexOptions.Compiled);

        readonly ChiffonConfig _config;
        readonly IFormsAuthenticationService _formsService;
        readonly IMemberService _memberService;

        public AccountController(
            ChiffonEnvironment environment,
            ISiteMap siteMap,
            IMemberService memberService,
            IFormsAuthenticationService formsService,
            ChiffonConfig config)
            : base(environment, siteMap)
        {
            Requires.NotNull(config, "config");
            Requires.NotNull(memberService, "memberService");

            _config = config;
            _formsService = formsService;
            _memberService = memberService;
        }

        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            ViewBag.Title = SR.Account_Login_Title;
            ViewBag.MetaDescription = SR.Account_Login_Description;
            ViewBag.CanonicalLink = SiteMap.Login().ToString();

            if (Request.IsAjaxRequest()) {
                return PartialView(ViewName.Account.Login);
            }
            else {
                return View(ViewName.Account.Login);
            }
        }

        [HttpGet]
        public ActionResult Register(string returnUrl)
        {
            ViewBag.Title = SR.Account_Register_Title;
            ViewBag.MetaDescription = SR.Account_Register_Description;
            ViewBag.CanonicalLink = SiteMap.Register().ToString();

            if (Request.IsAjaxRequest()) {
                return PartialView(ViewName.Account.Register, new RegisterViewModel());
            }
            else {
                return View(ViewName.Account.Register, new RegisterViewModel());
            }
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel contact)
        {
            if (ModelState.IsValid) {
                if (IsEmailAddressAlreadyTaken_(contact.EmailAddress)) {
                    return View(ViewName.Account.RegisterTwice);
                }

                ViewBag.Title = SR.Account_Register_Title;
                ViewBag.MetaDescription = SR.Account_Register_Description;
                ViewBag.CanonicalLink = SiteMap.Register().ToString();

                // FIXME:
                if (contact.Message == null) { contact.Message = String.Empty; }
                if (contact.ReturnUrl == null) { contact.ReturnUrl = String.Empty; }

                //var publicKey = 
                CreateContact_(contact);

                // FIXME: 
                string userName = contact.Firstname + " " + contact.Lastname;
                _formsService.SignIn(userName, false /* createPersistentCookie */);

                // FIXME: vérifier le contenu de l'URL.
                // FIXME: rajouter un indicateur que tout s'est bien passé.
                var nextUrl = MayParse.ToUri(contact.ReturnUrl, UriKind.Relative);
                if (nextUrl.IsSome) {
                    return Redirect(nextUrl.ToString());
                }
                else {
                    return RedirectToRoute(RouteName.Home.Index);
                }

                //var model = new NewContactViewModel {
                //    Firstname = contact.Firstname,
                //    Lastname = contact.Lastname,
                //    PublicKey = publicKey,
                //};

                //return View(ViewName.Contact.PostRegister, model);
            }
            else {
                ViewBag.Title = SR.Account_Register_Title;
                ViewBag.MetaDescription = SR.Account_Register_Description;
                ViewBag.CanonicalLink = SiteMap.Register().ToString();

                return View(ViewName.Account.Register, contact);
            }
        }

        [HttpGet]
        public ActionResult Newsletter()
        {
            ViewBag.Title = SR.Account_Newsletter_Title;
            ViewBag.MetaDescription = SR.Account_Newsletter_Description;
            ViewBag.CanonicalLink = SiteMap.Newsletter().ToString();
            ViewBag.MainMenuClass = "newsletter";

            return View(ViewName.Account.Newsletter);
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

        void CreateContact_(RegisterViewModel contact)
        {
            var publicKey = CreateRandomPassword_(25);

            using (var cnx = new SqlConnection(ConnectionString_)) {
                using (var cmd = new SqlCommand()) {
                    cmd.CommandText = "usp_NewContact";
                    cmd.Connection = cnx;
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameterCollection p = cmd.Parameters;
                    p.Add("@email_address", SqlDbType.NVarChar).Value = contact.EmailAddress;
                    p.Add("@firstname", SqlDbType.NVarChar).Value = contact.Firstname;
                    p.Add("@lastname", SqlDbType.NVarChar).Value = contact.Lastname;
                    p.Add("@company_name", SqlDbType.NVarChar).Value = contact.CompanyName;
                    p.Add("@public_key", SqlDbType.NVarChar).Value = publicKey;
                    p.Add("@message", SqlDbType.NVarChar).Value = contact.Message;

                    cnx.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            //return publicKey;
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
