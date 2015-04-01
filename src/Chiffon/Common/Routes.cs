namespace Chiffon.Common
{
    public static class Routes
    {
        public const string About = "informations";
        public const string Contact = "contact";
        public const string Newsletter = "newsletter";
        public const string Login = "connexion";
        public const string Register = "inscription";

        // TODO: Utiliser ces constantes dans SiteMap (cf. SmartFormat.NET).
        public const string Designer = "{designerKey}";
        public const string DesignerCategory = "{designerKey}/{categoryKey}";
        public const string DesignerPattern = "{designerKey}/{categoryKey}/{reference}";

        public const string LogOff = "disconnecte";
        public const string LogOn = "connecte";
        public const string Pattern = "motif";
    }
}
