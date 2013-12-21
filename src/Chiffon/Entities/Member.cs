namespace Chiffon.Entities
{
    using System;
    using System.Globalization;
    using System.Net.Mail;
    using Narvalo;

    public class Member
    {
        readonly string _email;
        readonly string _firstName;
        readonly string _lastName;

        string _displayName;
        MailAddress _emailAddress;

        public Member(string email, string firstName, string lastName)
        {
            Requires.NotNull(email, "email");
            Requires.NotNull(firstName, "firstName");
            Requires.NotNull(lastName, "lastName");

            _email = email;
            _firstName = firstName;
            _lastName = lastName;
        }

        public string Email { get { return _email; } }
        public string FirstName { get { return _firstName; } }
        public string LastName { get { return _lastName; } }

        public string DisplayName
        {
            get
            {
                if (_displayName == null) {
                    _displayName = String.Format(CultureInfo.CurrentCulture, SR.MemberDisplayNameFormat, FirstName, LastName);
                }
                return _displayName;
            }
        }

        public MailAddress EmailAddress
        {
            get
            {
                if (_emailAddress == null) {
                    _emailAddress = new MailAddress(Email, DisplayName);
                }
                return _emailAddress;
            }
        }
    }

}
