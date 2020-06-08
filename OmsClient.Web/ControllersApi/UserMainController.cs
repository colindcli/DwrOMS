using OmsClient.Biz;
using OmsClient.Common.Utilitys;
using OmsClient.Entity;
using OmsClient.Entity.Results;
using OmsClient.Web.ControllersBase;
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
    public class UserMainController : BaseUserApiController
    {
        private static readonly UserMainBiz Biz = new UserMainBiz();

        /// <summary>
        /// 菜单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult GetMainMenu()
        {
            var result = Biz.GetMainMenu(UserModel);
            return ReturnResult(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult GetUserInfo()
        {
            var result = Biz.GetUserInfo(UserModel);
            return ReturnResult(result);
        }

        /// <summary>
        /// 修改密码（修改公司所有人的密码）
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult UpdatePwd(User model)
        {
            var result = Biz.UpdatePwd(UserModel, model.UserId, model.UserPwd, out var msg);
            return ReturnResult(result, msg);
        }

        /// <summary>
        /// 修改密码 (修改我自己的密码)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult UpdateMyPwd(User request)
        {
            var result = Biz.UpdatePwd(UserModel, UserModel.UserId, request.UserPwd, out var msg);
            return ReturnResult(result, msg);
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult Logout()
        {
            TokenUserHandle.ClearTaken();
            return ReturnResult(true);
        }
    }
}
