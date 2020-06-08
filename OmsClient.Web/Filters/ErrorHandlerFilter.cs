using OmsClient.Common;
using System.Text;
using System.Web.Mvc;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Web.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public class ErrorHandlerFilter : HandleErrorAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnException(ExceptionContext filterContext)
        {
            LogHelper.Fatal(filterContext.Exception.Message + "; URL：" + filterContext.HttpContext.Request.RawUrl, filterContext.Exception);
            filterContext.Result = new ContentResult()
            {
                Content = filterContext.Exception.Message,
                ContentEncoding = Encoding.UTF8,
                ContentType = "text/html"
            };
            filterContext.ExceptionHandled = true;
        }
    }
}