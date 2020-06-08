using OmsClient.Biz.Users;
using OmsClient.Biz.Warehouses;
using OmsClient.Entity.Results;
using OmsClient.Web.ControllersBase;
using System;
using System.Net.Http;
using System.Web.Http;
using OmsClient.Biz.Finanaces;
using OmsClient.Entity;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Web.ControllersApi
{
    /// <summary>
    /// 仓库备货
    /// </summary>
    public class WarehouseSaleOrderController : BaseUserApiController
    {
        private static readonly WarehouseSaleOrderBiz Biz = new WarehouseSaleOrderBiz();

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
        /// 待备货销售单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetStockingOrder(int page, int limit, string keyword = null)
        {
            return Biz.GetStockingOrder(UserModel, page, limit, keyword);
        }

        /// <summary>
        /// 已备货销售单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetStockedOrder(int page, int limit, string keyword = null)
        {
            return Biz.GetStockedOrder(UserModel, page, limit, keyword);
        }

        /// <summary>
        /// 待发货销售单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetShipmentOrder(int page, int limit, string keyword = null)
        {
            return Biz.GetShipmentOrder(UserModel, page, limit, keyword);
        }

        /// <summary>
        /// 已发货销售单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetFinishedOrder(int page, int limit, string keyword = null)
        {
            return Biz.GetFinishedOrder(UserModel, page, limit, keyword);
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
        /// 获取备货结果
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetStockingResult(Guid saleOrderId)
        {
            var result = Biz.GetStockingResult(UserModel, saleOrderId, out var msg, out var _);
            if (result == null)
            {
                return ReturnResult(false, msg);
            }

            return ReturnResult(result);
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
        /// 导出产品
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage ExportStocking(Guid saleOrderId)
        {
            var result = Biz.ExportStocking(UserModel, saleOrderId, out var msg, out var bt, out var order);
            if (!result)
            {
                return new HttpResponseMessage()
                {
                    Content = new StringContent(msg)
                };
            }

            return ResponseAttachment(bt, $"Order_{order.SaleOrderNumber}.xlsx");
        }

        /// <summary>
        /// 开始备货
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult StartStocking(Guid saleOrderId)
        {
            var result = Biz.StartStocking(UserModel, saleOrderId, out var msg);
            return ReturnResult(result, msg);
        }

        /// <summary>
        /// 完成备货
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult EndStocking(Guid saleOrderId)
        {
            var result = Biz.EndStocking(UserModel, saleOrderId, out var msg);
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

        /// <summary>
        /// 出库发货
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult StockOut(SaleOrder order)
        {
            var result = Biz.StockOut(UserModel, order, out var msg);
            return ReturnResult(result, msg);
        }
    }
}
