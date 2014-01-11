namespace Chiffon.Entities
{
    using System;
    using System.Globalization;
    using System.Net.Mail;

    /// <summary>
    /// Représente un membre du site.
    /// </summary>
    [Serializable]
    public class Member
    {
        [NonSerialized]
        string _displayName;
        [NonSerialized]
        MailAddress _emailAddress;

        /// <summary>
        /// Retourne le nom du membre adapté à la culture en cours d'utilisation :
        /// <see cref="System.Globalization.CultureInfo.CurrentUICulture"/>.
        /// </summary>
        public string DisplayName
        {
            get
            {
                if (_displayName == null) {
                    _displayName = String.Format(CultureInfo.CurrentUICulture,
                        SR.MemberDisplayNameFormat, FirstName, LastName);
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
                if (_emailAddress == null) {
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
