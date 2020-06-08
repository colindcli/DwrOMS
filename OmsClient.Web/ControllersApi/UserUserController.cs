using OmsClient.Biz.Users;
using OmsClient.Entity;
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
    //[MenuApiFilter(MenuEnum.User)]
    public class UserUserController : BaseUserApiController
    {
        private static readonly UserUserBiz Biz = new UserUserBiz();

        /// <summary>
        /// 获取角色
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetUserRoleList()
        {
            var result = new UserUserRoleBiz().GetUserRoleList(UserModel);
            return ReturnResult(result);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult AddUser(User model)
        {
            var result = Biz.AddUser(UserModel, model, out var msg);
            return ReturnResult(result, msg);
        }

        /// <summary>
        /// 获取一条记录
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetUserById(Guid userId)
        {
            var result = Biz.GetUserById(UserModel, userId);
            return ReturnResult(result);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult UpdateUser(User model)
        {
            var result = Biz.UpdateUser(UserModel, model, out var msg);
            return ReturnResult(result, msg);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult UpdateUserPwd(User model)
        {
            var result = Biz.UpdateUserPwd(UserModel, model, out var msg);
            return ReturnResult(result, msg);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult DeleteUser(User model)
        {
            var result = Biz.DeleteUser(UserModel, model, out var msg);
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
        public IResponseResult GetUserList(int page, int limit, string keyword = null)
        {
            return Biz.GetUserList(UserModel, page, limit, keyword);
        }

        /// <summary>
        /// 获取所有记录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetUserList()
        {
            var result = Biz.GetUserList(UserModel);
            return ReturnResult(result);
        }
    }
}
