namespace Chiffon.Infrastructure
{
    using System;
    using System.Threading;

    public class EnglishChiffonEnvironment : ChiffonEnvironmentBase
    {
        public EnglishChiffonEnvironment(Uri baseUri)
            : base(ChiffonCulture.Create(ChiffonLanguage.English), baseUri) { }

        public override void Initialize()
        {
            // Culture utilisée par ResourceManager.
            Thread.CurrentThread.CurrentUICulture = Culture.UICulture;
            // Culture utilisée par System.Globalization.
            Thread.CurrentThread.CurrentCulture = Culture.Culture;
        }
    }

}