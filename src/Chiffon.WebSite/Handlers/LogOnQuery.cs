namespace Chiffon.Handlers
{
    using System;
    using Narvalo.Fx;

    public class LogOnQuery
    {
        public Maybe<string> Email { get; set; }
        public Maybe<string> Password { get; set; }
        public Maybe<Uri> TargetUrl { get; set; }

        public bool IsIncomplete
        {
            get
            {
                return Email.IsNone || Password.IsNone;
            }
        }
    }
}
