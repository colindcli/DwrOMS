using OmsClient.Biz;
using OmsClient.Entity;
using OmsClient.Entity.Enums;
using OmsClient.Entity.Results;
using OmsClient.Web.ControllersBase;
using OmsClient.Web.Filters;
using System;
using System.Web.Http;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Web.ControllersApi
{
    /// <summary>
    /// 管理员列表
    /// </summary>
    [MenuApiFilter(MenuEnum.User)]
    public class UserController : BaseUserApiController
    {
        private static readonly UserMainBiz Biz = new UserMainBiz();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetUserRoleList(string userRoleName = null)
        {
            var result = Biz.GetUserRoleList(userRoleName, PageIndex, PageSize);
            return ReturnResult(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetUserList(string userName = null, Guid? userRoleId = null)
        {
            return Biz.GetUserList(userName, userRoleId, PageIndex, PageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult AddUser(User model)
        {
            var result = Biz.AddUser(model);
            if (result == null)
            {
                return ReturnResult(false, ReturnCode.AddFailed);
            }

            return ReturnResult(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult UpdatePwd(User model)
        {
            Biz.UpdatePwd(model);
            return ReturnResult(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult UpdateUser(User model)
        {
            Biz.UpdateUser(model);
            return ReturnResult(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult DeleteUser(User model)
        {
            var b = Biz.DeleteUser(model);
            return ReturnResult(b, b ? ReturnCode.Ok : ReturnCode.DeleteFailed);
        }
    }
}
