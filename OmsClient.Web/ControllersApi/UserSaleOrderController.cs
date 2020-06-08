using DwrUtility;
using OmsClient.Biz.Finanaces;
using OmsClient.Biz.Users;
using OmsClient.Biz.Warehouses;
using OmsClient.Common;
using OmsClient.Entity;
using OmsClient.Entity.Models;
using OmsClient.Entity.Results;
using OmsClient.Web.ControllersBase;
using System;
using System.IO;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using OmsClient.Biz;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Web.ControllersApi
{
    /// <summary>
    /// 统计分析
    /// </summary>
    public partial class UserSaleOrderController
    {
        /// <summary>
        /// 获取业务员
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetUserList()
        {
            var result = new SaleStatisticsBiz().GetUserList(UserModel, false);
            return ReturnResult(result);
        }

        /// <summary>
        /// 获取订单统计列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult GetSaleStatisticsResults(SaleStatisticsRequest request)
        {
            request.UserId = request.UserId;
            var result = new SaleStatisticsBiz().GetSaleStatisticsResults(request);
            return ReturnResult(result);
        }

        /// <summary>
        /// 收款图表统计
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult GetSaleStatisticsChart(SaleChartRequest request)
        {
            request.UserId = UserModel.UserId;
            var result = new SaleStatisticsBiz().GetSaleStatisticsChart(request);
            return ReturnResult(result);
        }
    }

    public partial class UserSaleOrderController
    {
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
        /// 草稿订单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetSaleOrderListDraft(int page, int limit, string keyword = null)
        {
            return Biz.GetSaleOrderListDraft(UserModel, page, limit, keyword);
        }

        /// <summary>
        /// 待备货订单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetSaleOrderListStocking(int page, int limit, string keyword = null)
        {
            return Biz.GetSaleOrderListStocking(UserModel, page, limit, keyword);
        }

        /// <summary>
        /// 待收款订单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetSaleOrderListUnpay(int page, int limit, string keyword = null)
        {
            return Biz.GetSaleOrderListUnpay(UserModel, page, limit, keyword);
        }

        /// <summary>
        /// 待发货订单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetSaleOrderListUnship(int page, int limit, string keyword = null)
        {
            return Biz.GetSaleOrderListUnship(UserModel, page, limit, keyword);
        }

        /// <summary>
        /// 已发货订单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetSaleOrderListShiped(int page, int limit, string keyword = null)
        {
            return Biz.GetSaleOrderListShiped(UserModel, page, limit, keyword);
        }

        /// <summary>
        /// 已完结订单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetSaleOrderListFinished(int page, int limit, string keyword = null)
        {
            return Biz.GetSaleOrderListFinished(UserModel, page, limit, keyword);
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

    /// <summary>
    /// 草稿销售单
    /// </summary>
    //[MenuApiFilter(MenuEnum.SaleOrder)]
    public partial class UserSaleOrderController : BaseUserApiController
    {
        private static readonly UserSaleOrderBiz Biz = new UserSaleOrderBiz();

        /// <summary>
        /// 获取货币列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetCurrencyList()
        {
            var result = new UserCurrencyBiz().GetCurrencyList(UserModel);
            return ReturnResult(result);
        }

        /// <summary>
        /// 获取编辑数据
        /// </summary>
        /// <param name="saleOrderId"></param>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetSaleOrderDetail(Guid saleOrderId)
        {
            var result = Biz.GetSaleOrderDetail(UserModel, saleOrderId);
            return ReturnResult(result);
        }

        /// <summary>
        /// 获取订单产品列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetSaleOrderDetailProduct(Guid saleOrderId)
        {
            return Biz.GetSaleOrderDetailProduct(UserModel, saleOrderId);
        }
        
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="saleOrderId"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="categoryId"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetProductSelectList(Guid saleOrderId, int page, int limit, int? categoryId = null, string keyword = null)
        {
            return Biz.GetProductSelectList(UserModel, saleOrderId, page, limit, categoryId, keyword);
        }

        #region 草稿订单

        /// <summary>
        /// 更新订单汇率
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult UpdateSaleOrderRate(SaleOrder request)
        {
            var result = Biz.UpdateSaleOrderRate(UserModel, request, out var msg, out var rate);
            if (!result)
            {
                return ReturnResult(false, msg);
            }

            return ReturnResult(rate);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult AddSaleOrder(SaleOrder model)
        {
            var result = Biz.AddSaleOrder(UserModel, model, out var msg, out var saleOrderId);
            if (!result)
            {
                return ReturnResult(false, msg);
            }

            return ReturnResult(saleOrderId);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult DeleteSaleOrder(SaleOrder model)
        {
            var result = Biz.DeleteSaleOrder(UserModel, model, out var msg);
            return ReturnResult(result, msg);
        }

        /// <summary>
        /// 删除订单产品
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult DeleteOrderProduct(DeleteSaleOrderProductModel request)
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
        public IResponseResult AddOrderProduct(SaleOrderProduct request)
        {
            var result = Biz.AddOrderProduct(UserModel, request, out var msg);
            return ReturnResult(result, msg);
        }

        /// <summary>
        /// 更新订单产品
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult UpdateOrderProduct(SaleUpdateSaleOrderProductRequest request)
        {
            var result = Biz.UpdateOrderProduct(UserModel, request, out var msg);
            return ReturnResult(result, msg);
        }

        /// <summary>
        /// 更新订单详情
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult UpdateOrder(SaleOrder request)
        {
            var result = Biz.UpdateOrder(UserModel, request, out var msg);
            return ReturnResult(result, msg);
        }

        /// <summary>
        /// 提交订单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult PostOrder(SaleOrder request)
        {
            var result = Biz.PostOrder(UserModel, request, out var msg);
            return ReturnResult(result, msg);
        }
        #endregion

        /// <summary>
        /// 导入产品
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult ImportProduct()
        {
            var categoryId = HttpContext.Current.Request.Form["SaleOrderId"].ToGuid();
            var files = HttpContext.Current.Request.Files;
            if (files.Count == 0)
            {
                return ReturnResult(false, "请选择附件");
            }

            var item = files[0];
            var tempPath = $"{Config.Root}/Files/Temp/{Guid.NewGuid()}";
            item.SaveAs(tempPath);

            //
            var result = Biz.ImportProduct(UserModel, categoryId, tempPath, out var msg);

            //删除文件
            if (File.Exists(tempPath))
            {
                File.Delete(tempPath);
            }

            return ReturnResult(result, msg);
        }

        /// <summary>
        /// 导出订单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage ExportProduct(Guid saleOrderId)
        {
            var result = Biz.ExportProduct(UserModel, saleOrderId, out var msg, out var bt, out var order);
            if (!result)
            {
                return new HttpResponseMessage()
                {
                    Content = new StringContent(msg)
                };
            }

            return ResponseAttachment(bt, $"PI_{order.SaleOrderNumber}.xlsx");
        }

    }

    /// <summary>
    /// 待备货销售单
    /// </summary>
    public partial class UserSaleOrderController
    {
        /// <summary>
        /// 退回修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult ReturnBackSaleOrder(SaleOrder model)
        {
            var result = Biz.ReturnBackSaleOrder(UserModel, model, out var msg);
            return ReturnResult(result, msg);
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

    }
}
