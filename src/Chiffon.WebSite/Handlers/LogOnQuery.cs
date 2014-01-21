namespace Chiffon.Handlers
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Narvalo.Fx;

    public class LogOnQuery
    {
        [Required]
        [StringLength(200, MinimumLength = 5)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 1)]
        public string Password { get; set; }

        public Maybe<Uri> TargetUrl { get; set; }
    }
}
