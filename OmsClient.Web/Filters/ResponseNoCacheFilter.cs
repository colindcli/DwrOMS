using System;
using System.Web.Mvc;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Web.Filters
{
    /// <summary>
    /// 客户端不缓存
    /// </summary>
    public class ResponseNoCacheFilter : ActionFilterAttribute
    {
        /// <summary>
        /// 不缓存
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            filterContext.HttpContext.Response.CacheControl = "no-cache";
            filterContext.HttpContext.Response.ExpiresAbsolute = DateTime.Now.AddYears(-1);
            filterContext.HttpContext.Response.Expires = -1;
            base.OnResultExecuted(filterContext);
        }
    }
}