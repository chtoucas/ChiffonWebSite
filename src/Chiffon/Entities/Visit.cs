namespace Chiffon.Entities
{
    using System;
    using System.Globalization;

    public class Visit
    {
        readonly DateTime _time;

        public Visit(DateTime time)
        {
            _time = time;
        }

        public DateTime Time
        {
            get { return _time; }
        }

        public override string ToString()
        {
            return _time.ToString(CultureInfo.CurrentCulture);
        }

        public string ToString(IFormatProvider provider)
        {
            return _time.ToString(provider);
        }
    }
}
