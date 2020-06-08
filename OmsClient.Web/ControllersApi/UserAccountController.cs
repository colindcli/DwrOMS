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
    //[MenuApiFilter(MenuEnum.Account)]
    public class UserAccountController : BaseUserApiController
    {
        private static readonly UserAccountBiz Biz = new UserAccountBiz();

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult AddAccount(Account model)
        {
            var result = Biz.AddAccount(UserModel, model);
            return ReturnResult(result);
        }

        /// <summary>
        /// 获取一条记录
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetAccountById(Guid accountId)
        {
            var result = Biz.GetAccountById(UserModel, accountId);
            return ReturnResult(result);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult UpdateAccount(Account model)
        {
            var result = Biz.UpdateAccount(UserModel, model);
            return ReturnResult(result);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult DeleteAccount(Account model)
        {
            var result = Biz.DeleteAccount(UserModel, model, out var msg);
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
        public IResponseResult GetAccountList(int page, int limit, string keyword = null)
        {
            return Biz.GetAccountList(UserModel, page, limit, keyword);
        }

        /// <summary>
        /// 获取所有记录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetAccountList()
        {
            var result = Biz.GetAccountList(UserModel);
            return ReturnResult(result);
        }
    }
}
