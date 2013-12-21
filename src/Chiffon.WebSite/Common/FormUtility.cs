namespace Chiffon.Common
{
    using System;

    public static class FormUtility
    {
        public static bool IsCheckboxOn(string value)
        {
            return String.IsNullOrEmpty(value) ? false : (value == "on");
        }
    }
}