namespace Chiffon.Mailers
{
    using System;
    using Mvc.Mailer;

    [CLSCompliant(false)]
    public interface IAccountMailer
    {
        MvcMailMessage Welcome();
    }
}