namespace Chiffon.Services
{
    using System;
    using System.Globalization;

    public class MemberInfo
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string DisplayName
        {
            get
            {
                return String.Format(CultureInfo.CurrentCulture, SR.MemberDisplayNameFormat, FirstName, LastName);
            }
        }
    }
}
