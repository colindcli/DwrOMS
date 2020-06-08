using OmsClient.Biz.Users;
using OmsClient.Entity;
using OmsClient.Entity.Models;
using OmsClient.Entity.Results;
using OmsClient.Web.ControllersBase;
using System;
using System.Web.Http;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Web.ControllersApi
{
    /// <summary>
    /// 
    /// </summary>
    //[MenuApiFilter(MenuEnum.UserRole)]
    public class UserUserRoleController : BaseUserApiController
    {
        private static readonly UserUserRoleBiz Biz = new UserUserRoleBiz();

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult AddUserRole(UserRole model)
        {
            var result = Biz.AddUserRole(UserModel, model);
            return ReturnResult(result);
        }

        /// <summary>
        /// 获取一条记录
        /// </summary>
        /// <param name="userRoleId"></param>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetUserRoleById(Guid userRoleId)
        {
            var result = Biz.GetUserRoleById(UserModel, userRoleId);
            return ReturnResult(result);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult UpdateUserRole(UserRole model)
        {
            var result = Biz.UpdateUserRole(UserModel, model);
            return ReturnResult(result);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult DeleteUserRole(UserRole model)
        {
            var result = Biz.DeleteUserRole(UserModel, model, out var msg);
            return ReturnResult(result, msg);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetUserRoleList(int page, int limit, string keyword = null)
        {
            return Biz.GetUserRoleList(UserModel, page, limit, keyword);
        }

        /// <summary>
        /// 获取所有记录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetUserRoleList()
        {
            var result = Biz.GetUserRoleList(UserModel);
            return ReturnResult(result);
        }

        /// <summary>
        /// 获取一条设置菜单
        /// </summary>
        /// <param name="userRoleId"></param>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetUserRoleSetById(Guid userRoleId)
        {
            var result = Biz.GetUserRoleSetById(UserModel, userRoleId);
            return ReturnResult(result);
        }

        /// <summary>
        /// 设置菜单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult UpdateUserRoleSet(UserRoleMenuRequest model)
        {
            var result = Biz.UpdateUserRoleSet(UserModel, model, out var msg);
            return ReturnResult(result, msg);
        }
    }
}
