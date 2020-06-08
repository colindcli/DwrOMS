using OmsClient.Biz.Warehouses;
using OmsClient.Entity;
using OmsClient.Entity.Results;
using OmsClient.Web.ControllersBase;
using System;
using System.Web.Http;
using OmsClient.Biz.Finanaces;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Web.ControllersApi
{
    /// <summary>
    /// 
    /// </summary>
    //[MenuApiFilter(MenuEnum.SaleTrack)]
    public class WarehouseSaleTrackController : BaseUserApiController
    {
        private static readonly WarehouseSaleTrackBiz Biz = new WarehouseSaleTrackBiz();

        /// <summary>
        /// 币种和账号
        /// </summary>
        /// <param name="type">0全部账号；1收款；2付款</param>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetAccountCurrency(int type = 0)
        {
            var result = new FinanaceSaleReceiveBiz().GetAccountCurrency(UserModel, type);
            return ReturnResult(result);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult AddSaleTrack(SaleTrack model)
        {
            var result = Biz.AddSaleTrack(UserModel, model, out var msg);
            return ReturnResult(result, msg);
        }

        /// <summary>
        /// 获取一条记录
        /// </summary>
        /// <param name="saleTrackId"></param>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetSaleTrackById(Guid saleTrackId)
        {
            var result = Biz.GetSaleTrackById(UserModel, saleTrackId);
            return ReturnResult(result);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult UpdateSaleTrack(SaleTrack model)
        {
            var result = Biz.UpdateSaleTrack(UserModel, model, out var msg);
            return ReturnResult(result, msg);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult DeleteSaleTrack(SaleTrack model)
        {
            var result = Biz.DeleteSaleTrack(UserModel, model);
            return ReturnResult(result);
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
        public IResponseResult GetSaleTrackList(Guid saleOrderId, int page, int limit, string keyword = null)
        {
            return Biz.GetSaleTrackList(UserModel, saleOrderId, page, limit, keyword);
        }

        /// <summary>
        /// 获取所有记录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetSaleTrackList(Guid saleOrderId)
        {
            var result = Biz.GetSaleTrackList(UserModel, saleOrderId);
            return ReturnResult(result);
        }
    }
}
