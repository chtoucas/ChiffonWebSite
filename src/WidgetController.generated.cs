// <auto-generated />
// This file was generated by a T4 template.
// Don't change it directly as your change would get overwritten.  Instead, make changes
// to the .tt file (i.e. the T4 template) and save it to regenerate this file.

// Make sure the compiler doesn't complain about missing Xml comments
#pragma warning disable 1591
#region T4MVC

using System;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Web.Routing;
using T4MVC;
namespace Chiffon.Controllers
{
    public partial class WidgetController
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected WidgetController(Dummy d) { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoute(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoutePermanent(callInfo.RouteValueDictionary);
        }


        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public WidgetController Actions { get { return MVC.Widget; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "Widget";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "Widget";

        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string MemberMenu = "MemberMenu";
            public readonly string CommonJavaScript = "CommonJavaScript";
            public readonly string CommonStylesheet = "CommonStylesheet";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string MemberMenu = "MemberMenu";
            public const string CommonJavaScript = "CommonJavaScript";
            public const string CommonStylesheet = "CommonStylesheet";
        }


        static readonly ViewsClass s_views = new ViewsClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ViewsClass Views { get { return s_views; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ViewsClass
        {
            static readonly _ViewNamesClass s_ViewNames = new _ViewNamesClass();
            public _ViewNamesClass ViewNames { get { return s_ViewNames; } }
            public class _ViewNamesClass
            {
            }
            static readonly _DebugClass s_Debug = new _DebugClass();
            public _DebugClass Debug { get { return s_Debug; } }
            [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
            public partial class _DebugClass
            {
                static readonly _ViewNamesClass s_ViewNames = new _ViewNamesClass();
                public _ViewNamesClass ViewNames { get { return s_ViewNames; } }
                public class _ViewNamesClass
                {
                    public readonly string JavaScript = "JavaScript";
                    public readonly string Stylesheet = "Stylesheet";
                }
                public readonly string JavaScript = "~/Views/Widget/Debug/JavaScript.cshtml";
                public readonly string Stylesheet = "~/Views/Widget/Debug/Stylesheet.cshtml";
            }
            static readonly _ReleaseClass s_Release = new _ReleaseClass();
            public _ReleaseClass Release { get { return s_Release; } }
            [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
            public partial class _ReleaseClass
            {
                static readonly _ViewNamesClass s_ViewNames = new _ViewNamesClass();
                public _ViewNamesClass ViewNames { get { return s_ViewNames; } }
                public class _ViewNamesClass
                {
                    public readonly string JavaScript = "JavaScript";
                    public readonly string Stylesheet = "Stylesheet";
                }
                public readonly string JavaScript = "~/Views/Widget/Release/JavaScript.cshtml";
                public readonly string Stylesheet = "~/Views/Widget/Release/Stylesheet.cshtml";
            }
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public partial class T4MVC_WidgetController : Chiffon.Controllers.WidgetController
    {
        public T4MVC_WidgetController() : base(Dummy.Instance) { }

        partial void MemberMenuOverride(T4MVC_System_Web_Mvc_PartialViewResult callInfo);

        public override System.Web.Mvc.PartialViewResult MemberMenu()
        {
            var callInfo = new T4MVC_System_Web_Mvc_PartialViewResult(Area, Name, ActionNames.MemberMenu);
            MemberMenuOverride(callInfo);
            return callInfo;
        }

        partial void CommonJavaScriptOverride(T4MVC_System_Web_Mvc_PartialViewResult callInfo);

        public override System.Web.Mvc.PartialViewResult CommonJavaScript()
        {
            var callInfo = new T4MVC_System_Web_Mvc_PartialViewResult(Area, Name, ActionNames.CommonJavaScript);
            CommonJavaScriptOverride(callInfo);
            return callInfo;
        }

        partial void CommonStylesheetOverride(T4MVC_System_Web_Mvc_PartialViewResult callInfo);

        public override System.Web.Mvc.PartialViewResult CommonStylesheet()
        {
            var callInfo = new T4MVC_System_Web_Mvc_PartialViewResult(Area, Name, ActionNames.CommonStylesheet);
            CommonStylesheetOverride(callInfo);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591
