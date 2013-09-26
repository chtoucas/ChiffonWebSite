namespace Chiffon.Handlers
{
    using System;
    using Narvalo.Fx;

    public class LogOnQuery
    {
        public string Token { get; set; }
        public Maybe<Uri> TargetUrl { get; set; }
    }
}
