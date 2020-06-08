using System.Web.Http.Filters;
using OmsClient.Web.Filters;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Web
{
    /// <summary>
    /// HTTP Filter
    /// </summary>
    public class HttpFilterConfig
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filters"></param>
        public static void RegisterGlobalFilters(HttpFilterCollection filters)
        {
            filters.Add(new HttpErrorAttribute());
        }
    }
}