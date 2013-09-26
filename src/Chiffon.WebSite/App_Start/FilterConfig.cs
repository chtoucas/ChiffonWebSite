namespace Chiffon
{
    using System.Web.Mvc;
    using Narvalo;

    public static class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            Requires.NotNull(filters, "filters");

            filters.Add(new HandleErrorAttribute());
        }
    }
}