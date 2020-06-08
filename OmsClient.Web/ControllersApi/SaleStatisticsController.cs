using OmsClient.Biz;
using OmsClient.Biz.Finanaces;
using OmsClient.Biz.Users;
using OmsClient.Biz.Warehouses;
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
    public class SaleStatisticsController : BaseUserApiController
    {
        private static readonly SaleStatisticsBiz Biz = new SaleStatisticsBiz();

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
        /// 获取业务员
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetUserList()
        {
            var result = Biz.GetUserList(UserModel, true);
            return ReturnResult(result);
        }

        /// <summary>
        /// 收款统计
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult GetSaleStatisticsResults(SaleStatisticsRequest request)
        {
            var result = Biz.GetSaleStatisticsResults(request);
            return ReturnResult(result);
        }

        /// <summary>
        /// 收款图表统计
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult GetSaleStatisticsChart(SaleChartRequest request)
        {
            var result = Biz.GetSaleStatisticsChart(request);
            return ReturnResult(result);
        }

        /// <summary>
        /// 收款图表统计
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult GetSaleProductStatistics(SaleChartRequest request)
        {
            var result = Biz.GetSaleStatisticsChart(request);
            return ReturnResult(result);
        }

        /// <summary>
        /// 产品统计
        /// </summary>
        /// <param name="beginDatetime"></param>
        /// <param name="endDatetime"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="categoryId"></param>
        /// <param name="keyword"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetSaleProductStatistics(DateTime beginDatetime, DateTime endDatetime, int page, int limit, int? categoryId = null, string keyword = null, Guid? userId = null)
        {
            return new SaleProductStatisticsBiz().GetSaleProductStatistics(UserModel, beginDatetime, endDatetime, userId, page, limit, categoryId, keyword);
        }

        #region

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
        /// 物流列表和统计
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetSaleTrackResult(Guid saleOrderId)
        {
            var result = new WarehouseSaleTrackBiz().GetSaleTrackResult(UserModel, saleOrderId);
            return ReturnResult(result);
        }

        #endregion
    }
}
