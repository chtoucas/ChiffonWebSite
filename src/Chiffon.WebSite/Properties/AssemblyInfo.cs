﻿using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using Chiffon;
using WebActivatorEx;

[assembly: AssemblyTitle("Chiffon.WebSite")]
[assembly: AssemblyDescription("Chiffon WebSite")]
[assembly: AssemblyCulture("")]
[assembly: NeutralResourcesLanguage("fr-FR")]

[assembly: Guid("a572da2f-38b1-45d5-8265-828e8d3b8e21")]

// WARNING: cet attribut ne peut être utilisé qu'une fois par assemblée.
//[assembly: System.Web.PreApplicationStartMethod(typeof(AppActivator), "PreStart")]

[assembly: PreApplicationStartMethod(typeof(AppActivator), "PreStart")]
[assembly: PostApplicationStartMethod(typeof(AppActivator), "PostStart")]
[assembly: ApplicationShutdownMethod(typeof(AppActivator), "Shutdown")]