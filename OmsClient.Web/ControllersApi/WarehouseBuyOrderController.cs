using OmsClient.Biz.Warehouses;
using OmsClient.Entity;
using OmsClient.Entity.Results;
using OmsClient.Web.ControllersBase;
using System;
using System.Net.Http;
using System.Web.Http;
using OmsClient.Biz.Finanaces;
using OmsClient.Biz.Users;
using OmsClient.Entity.Models;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Web.ControllersApi
{
    /// <summary>
    /// 仓库入库
    /// </summary>
    public class WarehouseBuyOrderController : BaseUserApiController
    {
        private static readonly WarehouseBuyOrderBiz Biz = new WarehouseBuyOrderBiz();

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

        /// <summary>
        /// 待入库列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetUnstockInOrder(int page, int limit, string keyword = null)
        {
            return Biz.GetUnstockInOrder(UserModel, page, limit, keyword);
        }

        /// <summary>
        /// 已入库列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetStockInOrder(int page, int limit, string keyword = null)
        {
            return Biz.GetStockInOrder(UserModel, page, limit, keyword);
        }

        /// <summary>
        /// 导出验货单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage ExportStocking(Guid buyOrderId)
        {
            var result = Biz.ExportStocking(UserModel, buyOrderId, out var msg, out var bt, out var order);
            if (!result)
            {
                return new HttpResponseMessage()
                {
                    Content = new StringContent(msg)
                };
            }

            return ResponseAttachment(bt, $"YH_{order.BuyOrderNumber}.xlsx");
        }

        /// <summary>
        /// 入库
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult StockIn(BuyOrder request)
        {
            var result = Biz.StockIn(UserModel, request, out var msg);
            return ReturnResult(result, msg);
        }

        /// <summary>
        /// 更新订单产品
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult UpdateOrderProduct(BuyUpdateBuyOrderProductRequest request)
        {
            var result = Biz.UpdateOrderProduct(UserModel, request, out var msg);
            return ReturnResult(result, msg);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="buyOrderId"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="categoryId"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetProductSelectList(Guid buyOrderId, int page, int limit, int? categoryId = null, string keyword = null)
        {
            return new UserBuyOrderBiz().GetProductSelectList(UserModel, buyOrderId, page, limit, categoryId, keyword);
        }

        /// <summary>
        /// 删除订单产品
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult DeleteOrderProduct(DeleteBuyOrderProductModel request)
        {
            var result = Biz.DeleteOrderProduct(UserModel, request, out var msg);
            return ReturnResult(result, msg);
        }

        /// <summary>
        /// 添加订单产品
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult AddOrderProduct(BuyOrderProduct request)
        {
            var result = Biz.AddOrderProduct(UserModel, request, out var msg);
            return ReturnResult(result, msg);
        }
    }
}
