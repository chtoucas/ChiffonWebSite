namespace Chiffon
{
    using System.Web.Mvc;
    using Narvalo;

    public static class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            Requires.NotNull(filters, "filters");

            // NB: La gestion des erreurs est déléguée à customErrors pour avoir un contrôle
            // plus fin des codes HTTP de réponse et du message affiché (en anglais ou en français).
            // Si on réactive le filtre HandleError, il faut aussi créer une vue ~/Views/Shared/Error.cshml.
            //filters.Add(new HandleErrorAttribute());
        }
    }
}