namespace Chiffon.Handlers
{
    using System;
    using Narvalo.Fx;

    public class LogOnQuery
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public Maybe<Uri> TargetUrl { get; set; }
    }
}
