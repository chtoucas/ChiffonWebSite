using System.Reflection;
using System.Runtime.InteropServices;
using Chiffon.WebSite;
using WebActivatorEx;

[assembly: AssemblyTitle("Chiffon.WebSite.UI")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("Chiffon.WebSite.UI")]
[assembly: AssemblyCopyright("Copyright ©  2013")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

[assembly: ComVisible(false)]

[assembly: Guid("a572da2f-38b1-45d5-8265-828e8d3b8e21")]

[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]


// WARNING: cet attribut ne peut être utilisé qu'une fois par assemblée.
//[assembly: System.Web.PreApplicationStartMethod(typeof(AppActivator), "PreStart")]

[assembly: PreApplicationStartMethod(typeof(AppActivator), "PreStart")]
[assembly: PostApplicationStartMethod(typeof(AppActivator), "PostStart")]
[assembly: ApplicationShutdownMethod(typeof(AppActivator), "Shutdown")]