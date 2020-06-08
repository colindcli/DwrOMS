using OmsClient.Biz;
using OmsClient.Biz.Finanaces;
using OmsClient.Biz.Users;
using OmsClient.Entity;
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
    /// 统计分析
    /// </summary>
    public partial class UserBuyOrderController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetCategoryList()
        {
            var result = new UserCategoryBiz().GetCategoryList(UserModel);
            return ReturnResult(result);
        }

        /// <summary>
        /// 获取采购员
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetUserList()
        {
            var result = new BuyStatisticsBiz().GetUserList(UserModel, false);
            return ReturnResult(result);
        }

        /// <summary>
        /// 获取订单统计列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult GetBuyStatisticsResults(BuyStatisticsRequest request)
        {
            request.UserId = UserModel.UserId;
            var result = new BuyStatisticsBiz().GetBuyStatisticsResults(request);
            return ReturnResult(result);
        }

        /// <summary>
        /// 收款图表统计
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult GetBuyStatisticsChart(BuyChartRequest request)
        {
            request.UserId = UserModel.UserId;
            var result = new BuyStatisticsBiz().GetBuyStatisticsChart(request);
            return ReturnResult(result);
        }
    }

    /// <summary>
    /// 列表
    /// </summary>
    public partial class UserBuyOrderController
    {
        /// <summary>
        /// 统计订单数量
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetMyOrderNumber(bool hasCheck)
        {
            var result = Biz.GetMyOrderNumber(UserModel, hasCheck);
            return ReturnResult(result);
        }

        /// <summary>
        /// 草稿单列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetBuyOrderListDraft(int page, int limit, string keyword = null)
        {
            return Biz.GetBuyOrderListDraft(UserModel, page, limit, keyword);
        }
        /// <summary>
        /// 待审核采购单
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetBuyOrderListChecking(int page, int limit, string keyword = null)
        {
            return Biz.GetBuyOrderListChecking(UserModel, page, limit, keyword);
        }
        /// <summary>
        /// 已退回采购单
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetBuyOrderListBack(int page, int limit, string keyword = null)
        {
            return Biz.GetBuyOrderListBack(UserModel, page, limit, keyword);
        }
        /// <summary>
        /// 已审核采购单
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetBuyOrderListChecked(int page, int limit, string keyword = null)
        {
            return Biz.GetBuyOrderListChecked(UserModel, page, limit, keyword);
        }
        /// <summary>
        /// 待付款采购单
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetBuyOrderListUnpay(int page, int limit, string keyword = null)
        {
            return Biz.GetBuyOrderListUnpay(UserModel, page, limit, keyword);
        }
        /// <summary>
        /// 待入库采购单
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetBuyOrderListStocking(int page, int limit, string keyword = null)
        {
            return Biz.GetBuyOrderListStocking(UserModel, page, limit, keyword);
        }
        /// <summary>
        /// 待完结采购单
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetBuyOrderListUnfinished(int page, int limit, string keyword = null)
        {
            return Biz.GetBuyOrderListUnfinished(UserModel, page, limit, keyword);
        }
        /// <summary>
        /// 已完结采购单
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetBuyOrderListFinished(int page, int limit, string keyword = null)
        {
            return Biz.GetBuyOrderListFinished(UserModel, page, limit, keyword);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    //[MenuApiFilter(MenuEnum.BuyOrder)]
    public partial class UserBuyOrderController : BaseUserApiController
    {
        private static readonly UserBuyOrderBiz Biz = new UserBuyOrderBiz();

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult AddBuyOrder(BuyOrder model)
        {
            var result = Biz.AddBuyOrder(UserModel, model, out var msg, out var buyOrderId);
            if (!result)
            {
                return ReturnResult(false, msg);
            }

            return ReturnResult(buyOrderId);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult DeleteBuyOrder(BuyOrder model)
        {
            var result = Biz.DeleteBuyOrder(UserModel, model, out var msg);
            return ReturnResult(result, msg);
        }

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
        /// <param name="buyOrderId"></param>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetBuyOrderDetail(Guid buyOrderId)
        {
            var result = Biz.GetBuyOrderDetail(UserModel, buyOrderId, out var msg);
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
            return Biz.GetBuyOrderDetailProduct(UserModel, buyOrderId);
        }
    }

    /// <summary>
    /// 草稿采购单
    /// </summary>
    //[MenuApiFilter(MenuEnum.BuyOrder)]
    public partial class UserBuyOrderController
    {
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
            return Biz.GetProductSelectList(UserModel, buyOrderId, page, limit, categoryId, keyword);
        }

        #region 草稿订单

        /// <summary>
        /// 更新订单汇率
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult UpdateBuyOrderRate(BuyOrder request)
        {
            var result = Biz.UpdateBuyOrderRate(UserModel, request, out var msg, out var rate);
            if (!result)
            {
                return ReturnResult(false, msg);
            }

            return ReturnResult(rate);
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
        /// 更新订单详情
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult UpdateOrder(BuyOrder request)
        {
            var result = Biz.UpdateOrder(UserModel, request, out var msg);
            return ReturnResult(result, msg);
        }

        /// <summary>
        /// 草稿提交订单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult PostOrder(BuyOrder request)
        {
            var result = Biz.PostOrder(UserModel, request, out var msg);
            return ReturnResult(result, msg);
        }

        /// <summary>
        /// 审核通过提交订单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult PostCheckedOrder(BuyOrder request)
        {
            var result = Biz.PostCheckedOrder(UserModel, request, out var msg);
            return ReturnResult(result, msg);
        }
        #endregion
    }

    /// <summary>
    /// 待备货采购单
    /// </summary>
    public partial class UserBuyOrderController
    {
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
        /// 退回修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult ReturnBackBuyOrder(BuyOrder model)
        {
            var result = Biz.ReturnBackBuyOrder(UserModel, model, out var msg);
            return ReturnResult(result, msg);
        }

        /// <summary>
        /// 获取急需采购产品
        /// </summary>
        [HttpGet]
        public IResponseResult GetUrgentPurchase(int page, int limit, int? categoryId = null, string keyword = null)
        {
            return Biz.GetUrgentPurchase(UserModel, page, limit, categoryId, keyword);
        }
    }
}
