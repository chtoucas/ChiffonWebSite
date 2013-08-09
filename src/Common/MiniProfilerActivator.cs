namespace Chiffon.WebSite
{
    using System.Diagnostics;
    using System.Linq;
    using System.Web.Mvc;
    using StackExchange.Profiling.MVCHelpers;
    //using System.Data;
    //using System.Data.Entity;
    //using System.Data.Entity.Infrastructure;
    //using StackExchange.Profiling.Data.EntityFramework;
    //using StackExchange.Profiling.Data.Linq2Sql;

    public static class MiniProfilerActivator
    {
        [Conditional("PROFILE")]
        public static void PreStart()
        {
            MiniProfilerModule.SelfRegister();
        }

        [Conditional("PROFILE")]
        public static void PostStart()
        {
            // Intercept ViewEngines to profile all partial views and regular views.
            // If you prefer to insert your profiling blocks manually you can comment this out
            var copy = ViewEngines.Engines.ToList();
            ViewEngines.Engines.Clear();
            foreach (var item in copy) {
                ViewEngines.Engines.Add(new ProfilingViewEngine(item));
            }
        }

        //public static void PreStart()
        //{
        //    // Be sure to restart you ASP.NET Developement server, this code will not run until you do that. 

        //    //TODO: See - _MINIPROFILER UPDATED Layout.cshtml
        //    //      For profiling to display in the UI you will have to include the line @StackExchange.Profiling.MiniProfiler.RenderIncludes() 
        //    //      in your master layout

        //    //TODO: Non SQL Server based installs can use other formatters like: new StackExchange.Profiling.SqlFormatters.InlineFormatter()
        //    //MiniProfiler.Settings.SqlFormatter = new StackExchange.Profiling.SqlFormatters.SqlServerFormatter();

        //    //TODO: To profile a standard DbConnection: 
        //    // var profiled = new ProfiledDbConnection(cnn, MiniProfiler.Current);

        //    //TODO: If you are profiling EF code first try: 
        //    // MiniProfilerEF.Initialize();

        //    //Make sure the MiniProfiler handles BeginRequest and EndRequest
        //    //DynamicModuleUtility.RegisterModule(typeof(MiniProfilerModule));

        //    //Setup profiler for Controllers via a Global ActionFilter
        //    GlobalFilters.Filters.Add(new ProfilingActionFilter());

        //    // You can use this to check if a request is allowed to view results
        //    //MiniProfiler.Settings.Results_Authorize = (request) =>
        //    //{
        //    // you should implement this if you need to restrict visibility of profiling on a per request basis 
        //    //    return !DisableProfilingResults; 
        //    //};

        //    // the list of all sessions in the store is restricted by default, you must return true to alllow it
        //    //MiniProfiler.Settings.Results_List_Authorize = (request) =>
        //    //{
        //    // you may implement this if you need to restrict visibility of profiling lists on a per request basis 
        //    //return true; // all requests are kosher
        //    //};
        //}
    }
}

