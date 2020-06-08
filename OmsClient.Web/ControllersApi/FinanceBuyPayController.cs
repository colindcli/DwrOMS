using OmsClient.Biz.Finanaces;
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
    //[MenuApiFilter(MenuEnum.BuyPay)]
    public class FinanceBuyPayController : BaseUserApiController
    {
        private static readonly FinanaceBuyPayBiz Biz = new FinanaceBuyPayBiz();

        /// <summary>
        /// 币种和账号
        /// </summary>
        /// <param name="type">0全部账号；1收款；2付款</param>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetAccountCurrency(int type = 0)
        {
            var result = Biz.GetAccountCurrency(UserModel, type);
            return ReturnResult(result);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult AddBuyPay(BuyPay model)
        {
            var result = Biz.AddBuyPay(UserModel, model, out var msg);
            return ReturnResult(result, msg);
        }

        /// <summary>
        /// 获取一条记录
        /// </summary>
        /// <param name="buyReceiveId"></param>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetBuyPayById(Guid buyReceiveId)
        {
            var result = Biz.GetBuyPayById(UserModel, buyReceiveId);
            return ReturnResult(result);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult UpdateBuyPay(BuyPay model)
        {
            var result = Biz.UpdateBuyPay(UserModel, model, out var msg);
            return ReturnResult(result, msg);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult DeleteBuyPay(BuyPay model)
        {
            var result = Biz.DeleteBuyPay(UserModel, model, out var msg);
            return ReturnResult(result, msg);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="buyOrderId"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetBuyPayList(Guid buyOrderId, int page, int limit, string keyword = null)
        {
            return Biz.GetBuyPayList(UserModel, buyOrderId, page, limit, keyword);
        }

        /// <summary>
        /// 获取所有记录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetBuyPayList(Guid buyOrderId)
        {
            var result = Biz.GetBuyPayList(UserModel, buyOrderId);
            return ReturnResult(result);
        }
    }
}
