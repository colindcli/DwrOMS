using OmsClient.Common.Utilitys;
using OmsClient.Web.ControllersBase;
using System;
using System.Web.Mvc;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Web.Filters
{
    /// <summary>
    /// 过滤
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ExecutePageFilter : ActionFilterAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.Controller is BasePageController)
            {
                var controller = (BasePageController)filterContext.Controller;
                //是否已登录
                var m = TokenUserHandle.GetToken();
                controller.PageInfo.IsLogin = m != null;
                controller.ViewBag.Page = controller.PageInfo;
            }
        }
    }
}