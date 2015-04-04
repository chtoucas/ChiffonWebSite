namespace Chiffon.Entities
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Net.Mail;

    /// <summary>
    /// Représente un membre du site.
    /// </summary>
    public sealed class Member
    {
        private static readonly Member s_Anonymous = new Member(true) {
            Email = String.Empty,
            FirstName = String.Empty,
            LastName = String.Empty
        };

        private readonly bool _anonymous;

        private string _displayName;
        private MailAddress _emailAddress;

        public Member() : this(false) { }

        private Member(bool anonymous)
        {
            _anonymous = anonymous;
        }

        public static Member Anonymous { get { return s_Anonymous; } }

        /// <summary>
        /// Retourne le nom du membre adapté à la culture en cours d'utilisation :
        /// <see cref="System.Globalization.CultureInfo.CurrentUICulture"/>.
        /// </summary>
        public string DisplayName
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);

                if (_displayName == null)
                {
                    _displayName = _anonymous
                        ? Strings.Member_DisplayName_Anonymous
                        : String.Format(
                            CultureInfo.CurrentUICulture,
                            Strings.Member_DisplayName_Format,
                            FirstName,
                            LastName);
                }

                return _displayName;
            }
        }

        /// <summary>
        /// Assigne ou retourne l'adresse e-mail du membre.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Retourne l'adresse électronique du membre construite à partir de l'e-mail
        /// et du nom du membre.
        /// </summary>
        public MailAddress EmailAddress
        {
            get
            {
                Contract.Ensures(Contract.Result<MailAddress>() != null);

                if (_anonymous)
                {
                    throw new NotSupportedException("An anonymous user does not have an email address.");
                }

                if (_emailAddress == null)
                {
                    // XXX: Doit-on préciser l'encodage du nom ?
                    _emailAddress = new MailAddress(Email, DisplayName);
                }

                return _emailAddress;
            }
        }

        /// <summary>
        /// Assigne ou retourne le prénom du membre.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Assigne ou retourne le nom de famille du membre.
        /// </summary>
        public string LastName { get; set; }
    }
}
