namespace Chiffon.Mailers
{
    using System;
    using Chiffon.Resources;
    using Mvc.Mailer;

    [CLSCompliant(false)]
    public class AccountMailer : MailerBase, IAccountMailer
    {
        public AccountMailer()
        {
            MasterName = "_Layout";
        }

        public virtual MvcMailMessage Welcome()
        {
            ViewBag.EmailAddress = "monmail@gmail.com";
            ViewBag.Password = "monmotdepasse";

            return Populate(x =>
            {
                x.Subject = MailResources.Welcome_Subject;
                x.ViewName = "Welcome";
                //x.To.Add("some-email@example.com");
            });
        }
    }
}