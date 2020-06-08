using OmsClient.Common;
using OmsClient.Common.Utilitys;
using OmsClient.Entity.Enums;
using OmsClient.Entity.Results;
using OmsClient.Web.ControllersBase;
using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Controllers;

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
    public class AuthorizeApiFilter : AuthorizeAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            base.IsAuthorized(actionContext);
            if (actionContext.ControllerContext.Controller is BaseUserApiController userController)
            {
                var m = TokenUserHandle.GetToken();
                if (m == null)
                {
                    return false;
                }

                userController.UserModel = m;
                return true;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionContext"></param>
        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            base.HandleUnauthorizedRequest(actionContext);

            var code = (int)ReturnCode.TokenInvalid;
            var result = new ResponseResult<string>()
            {
                code = code,
                msg = Config.DictReturnCode[code],
                data = null
            };
            actionContext.Response = new HttpResponseMessage()
            {
                Content = new ObjectContent<IResponseResult>(result, new JsonMediaTypeFormatter(), "application/json")
            };
        }
    }
}