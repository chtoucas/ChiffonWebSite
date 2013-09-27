using System.Reflection;
using System.Runtime.InteropServices;
using Chiffon;
using WebActivatorEx;

[assembly: AssemblyTitle("Chiffon.MediaSite")]
[assembly: AssemblyDescription("Chiffon MediaSite")]
[assembly: AssemblyCulture("")]

[assembly: Guid("ef8e0239-76f7-4d53-8439-de3e419aef2f")]

// WARNING: cet attribut ne peut être utilisé qu'une fois par assemblée.
//[assembly: System.Web.PreApplicationStartMethod(typeof(AppActivator), "PreStart")]

[assembly: PreApplicationStartMethod(typeof(AppActivator), "PreStart")]
[assembly: PostApplicationStartMethod(typeof(AppActivator), "PostStart")]
[assembly: ApplicationShutdownMethod(typeof(AppActivator), "Shutdown")]