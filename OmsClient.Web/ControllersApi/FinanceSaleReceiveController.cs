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
    //[MenuApiFilter(MenuEnum.SaleReceive)]
    public class FinanceSaleReceiveController : BaseUserApiController
    {
        private static readonly FinanaceSaleReceiveBiz Biz = new FinanaceSaleReceiveBiz();

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
        public IResponseResult AddSaleReceive(SaleReceive model)
        {
            var result = Biz.AddSaleReceive(UserModel, model, out var msg);
            return ReturnResult(result, msg);
        }

        /// <summary>
        /// 获取一条记录
        /// </summary>
        /// <param name="saleReceiveId"></param>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetSaleReceiveById(Guid saleReceiveId)
        {
            var result = Biz.GetSaleReceiveById(UserModel, saleReceiveId);
            return ReturnResult(result);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult UpdateSaleReceive(SaleReceive model)
        {
            var result = Biz.UpdateSaleReceive(UserModel, model, out var msg);
            return ReturnResult(result, msg);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult DeleteSaleReceive(SaleReceive model)
        {
            var result = Biz.DeleteSaleReceive(UserModel, model, out var msg);
            return ReturnResult(result, msg);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="saleOrderId"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetSaleReceiveList(Guid saleOrderId, int page, int limit, string keyword = null)
        {
            return Biz.GetSaleReceiveList(UserModel, saleOrderId, page, limit, keyword);
        }

        /// <summary>
        /// 获取所有记录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetSaleReceiveList(Guid saleOrderId)
        {
            var result = Biz.GetSaleReceiveList(UserModel, saleOrderId);
            return ReturnResult(result);
        }
    }
}
