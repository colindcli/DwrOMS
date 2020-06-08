using OmsClient.Biz.Finanaces;
using OmsClient.Biz.Users;
using OmsClient.Biz.Warehouses;
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
    /// 财务收款
    /// </summary>
    public class FinanceSaleOrderController : BaseUserApiController
    {
        private static readonly FinanaceSaleOrderBiz Biz = new FinanaceSaleOrderBiz();

        /// <summary>
        /// 统计订单数量
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetMyOrderNumber()
        {
            var result = Biz.GetMyOrderNumber(UserModel);
            return ReturnResult(result);
        }

        /// <summary>
        /// 待收款销售单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetNotReceiveOrder(int page, int limit, string keyword = null)
        {
            return Biz.GetNotReceiveOrder(UserModel, page, limit, keyword);
        }

        /// <summary>
        /// 后收款销售单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetAfterOrder(int page, int limit, string keyword = null)
        {
            return Biz.GetAfterOrder(UserModel, page, limit, keyword);
        }

        /// <summary>
        /// 已收款销售单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetHadReceiveOrder(int page, int limit, string keyword = null)
        {
            return Biz.GetHadReceiveOrder(UserModel, page, limit, keyword);
        }

        /// <summary>
        /// 获取编辑数据
        /// </summary>
        /// <param name="saleOrderId"></param>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetSaleOrderDetail(Guid saleOrderId)
        {
            var result = new UserSaleOrderBiz().GetSaleOrderDetail(UserModel, saleOrderId);
            return ReturnResult(result);
        }

        /// <summary>
        /// 获取订单产品列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetSaleOrderDetailProduct(Guid saleOrderId)
        {
            return new UserSaleOrderBiz().GetSaleOrderDetailProduct(UserModel, saleOrderId);
        }
        
        /// <summary>
        /// 获取财务进度记录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetSaleReceiveRrcord(Guid saleOrderId)
        {
            var result = new FinanaceSaleReceiveBiz().GetSaleReceiveRrcord(UserModel, saleOrderId, out var msg);
            if (result == null)
            {
                return ReturnResult(false, msg);
            }

            return ReturnResult(result);
        }

        /// <summary>
        /// 待收款提交
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public IResponseResult PostUnpayReceive(SaleOrderReceiveModel request)
        {
            var result = Biz.PostUnpayReceive(UserModel, request, out var msg);
            return ReturnResult(result, msg);
        }

        /// <summary>
        /// 后收款提交
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public IResponseResult PostPayReceive(SaleOrderReceiveModel request)
        {
            var result = Biz.PostPayReceive(UserModel, request, out var msg);
            return ReturnResult(result, msg);
        }

        /// <summary>
        /// 物流列表和统计
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetSaleTrackResult(Guid saleOrderId)
        {
            var result = new WarehouseSaleTrackBiz().GetSaleTrackResult(UserModel, saleOrderId);
            return ReturnResult(result);
        }
    }
}
