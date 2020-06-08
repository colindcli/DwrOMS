using OmsClient.Biz.Finanaces;
using OmsClient.Biz.Manages;
using OmsClient.Biz.Users;
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
    /// 管理审核采购单
    /// </summary>
    public class ManageBuyOrderController : BaseUserApiController
    {
        private static readonly ManageBuyOrderBiz Biz = new ManageBuyOrderBiz();

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
        /// 待审核采购单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetUncheckOrder(int page, int limit, string keyword = null)
        {
            return Biz.GetUncheckOrder(UserModel, page, limit, keyword);
        }

        /// <summary>
        /// 已审核采购单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetCheckOrder(int page, int limit, string keyword = null)
        {
            return Biz.GetCheckOrder(UserModel, page, limit, keyword);
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public IResponseResult CheckBuyOrder(BuyOrderCheckModel request)
        {
            var result = Biz.CheckBuyOrder(UserModel, request, out var msg);
            return ReturnResult(result, msg);
        }

        /// <summary>
        /// 获取编辑数据
        /// </summary>
        /// <param name="buyOrderId"></param>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetBuyOrderDetail(Guid buyOrderId)
        {
            var result = new UserBuyOrderBiz().GetBuyOrderDetail(UserModel, buyOrderId, out var msg);
            if (result == null)
            {
                return ReturnResult(false, msg);
            }

            return ReturnResult(result);
        }

        /// <summary>
        /// 获取订单产品列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetBuyOrderDetailProduct(Guid buyOrderId)
        {
            return new UserBuyOrderBiz().GetBuyOrderDetailProduct(UserModel, buyOrderId);
        }

        /// <summary>
        /// 获取财务进度记录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetBuyPayRrcord(Guid buyOrderId)
        {
            var result = new FinanaceBuyPayBiz().GetBuyPayRrcord(UserModel, buyOrderId, out var msg);
            if (result == null)
            {
                return ReturnResult(false, msg);
            }

            return ReturnResult(result);
        }
    }
}
