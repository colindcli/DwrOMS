using System;
using System.Web.Mvc;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Web.Filters
{
    /// <summary>
    /// 客户端缓存
    /// </summary>
    public class ResponseCacheFilter : ActionFilterAttribute
    {
        private int Seconds { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="second">单位：秒；默认:3600*24*365=31536000</param>
        public ResponseCacheFilter(int second = 31536000)
        {
            Seconds = second;
        }

        /// <summary>
        /// 缓存
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            var dt = Seconds > 0 ? DateTime.Now.AddSeconds(Seconds) : DateTime.Now.AddYears(1);
            filterContext.HttpContext.Response.Cache.SetExpires(dt);
            base.OnResultExecuted(filterContext);
        }
    }
}