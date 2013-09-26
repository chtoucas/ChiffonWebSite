using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using Chiffon;
using WebActivatorEx;

[assembly: AssemblyTitle("Chiffon.MediaSite")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("Chiffon")]
[assembly: AssemblyCopyright("Copyright ©  2013")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: NeutralResourcesLanguage("fr-FR")]

[assembly: ComVisible(false)]

[assembly: Guid("ef8e0239-76f7-4d53-8439-de3e419aef2f")]

// WARNING: cet attribut ne peut être utilisé qu'une fois par assemblée.
//[assembly: System.Web.PreApplicationStartMethod(typeof(AppActivator), "PreStart")]

[assembly: PreApplicationStartMethod(typeof(AppActivator), "PreStart")]
[assembly: PostApplicationStartMethod(typeof(AppActivator), "PostStart")]
[assembly: ApplicationShutdownMethod(typeof(AppActivator), "Shutdown")]