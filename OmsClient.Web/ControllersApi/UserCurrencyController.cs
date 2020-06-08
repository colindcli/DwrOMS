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
    //[MenuApiFilter(MenuEnum.Currency)]
    public class UserCurrencyController : BaseUserApiController
    {
        private static readonly UserCurrencyBiz Biz = new UserCurrencyBiz();

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult AddCurrency(Currency model)
        {
            var result = Biz.AddCurrency(UserModel, model);
            return ReturnResult(result);
        }

        /// <summary>
        /// 获取一条记录
        /// </summary>
        /// <param name="currencyId"></param>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetCurrencyById(Guid currencyId)
        {
            var result = Biz.GetCurrencyById(UserModel, currencyId);
            return ReturnResult(result);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult UpdateCurrency(Currency model)
        {
            var result = Biz.UpdateCurrency(UserModel, model);
            return ReturnResult(result);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult DeleteCurrency(Currency model)
        {
            var result = Biz.DeleteCurrency(UserModel, model, out var msg);
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
        public IResponseResult GetCurrencyList(int page, int limit, string keyword = null)
        {
            return Biz.GetCurrencyList(UserModel, page, limit, keyword);
        }

        /// <summary>
        /// 获取所有记录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetCurrencyList()
        {
            var result = Biz.GetCurrencyList(UserModel);
            return ReturnResult(result);
        }
    }
}
