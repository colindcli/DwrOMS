using RazorGenerator.Mvc;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

#if !DEBUG
[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(OmsClient.Web.RazorGeneratorMvcStart), "Start")]
#endif

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Web
{
    /// <summary>
    /// 
    /// </summary>
    public static class RazorGeneratorMvcStart
    {
        /// <summary>
        /// 
        /// </summary>
        public static void Start()
        {
            var engine = new PrecompiledMvcEngine(typeof(RazorGeneratorMvcStart).Assembly)
            {
                UsePhysicalViewsIfNewer = HttpContext.Current.Request.IsLocal
            };

            ViewEngines.Engines.Insert(0, engine);

            // StartPage lookups are done by WebPages. 
            VirtualPathFactoryManager.RegisterVirtualPathFactory(engine);
        }
    }
}
