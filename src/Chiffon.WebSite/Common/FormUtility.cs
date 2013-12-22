namespace Chiffon.Common
{
    using System;

    public static class FormUtility
    {
        public static bool IsCheckBoxOn(string value)
        {
            return String.IsNullOrEmpty(value) ? false : (value == "on");
        }
    }
}