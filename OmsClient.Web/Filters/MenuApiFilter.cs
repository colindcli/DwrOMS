using OmsClient.Biz;
using OmsClient.Common.Utilitys;
using OmsClient.Entity.Enums;
using OmsClient.Entity.Results;
using OmsClient.Web.ControllersBase;
using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using OmsClient.Common;

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
    public class MenuApiFilter : ActionFilterAttribute
    {

        private static readonly AdminUserBiz Biz = new AdminUserBiz();

        /// <summary>
        /// 
        /// </summary>
        private int AdminMenuId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="menu"></param>
        public MenuApiFilter(MenuEnum menu)
        {
            AdminMenuId = (int)menu;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            //if (actionContext.ControllerContext.Controller is BaseUserApiController userController)
            //{
            //    var userId = userController.UserModel.UserId;

            //}
            if (actionContext.ControllerContext.Controller is BaseUserApiController adminController)
            {
                var userId = adminController.UserModel.UserId;
                var menuIds = Biz.GetUserMenuList(userId);
                if (!menuIds.Contains(AdminMenuId))
                {
                    var code = (int)ReturnCode.PageInvalid;
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

            base.OnActionExecuting(actionContext);
        }
    }
}