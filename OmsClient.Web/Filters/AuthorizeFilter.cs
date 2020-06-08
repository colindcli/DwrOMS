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
    /// 权限过滤
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeFilter : AuthorizeAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.Controller is BaseUserController userController)
            {
                var m = TokenUserHandle.GetToken();
                if (m == null)
                {
                    base.OnAuthorization(filterContext);
                    return;
                }
                userController.UserModel = m;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.Controller is BaseUserController)
            {
                filterContext.Result = new RedirectResult("/login");
                return;
            }
        }
    }
}