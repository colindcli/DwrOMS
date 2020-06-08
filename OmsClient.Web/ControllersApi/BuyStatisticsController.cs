using OmsClient.Biz;
using OmsClient.Biz.Finanaces;
using OmsClient.Biz.Users;
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
    public class BuyStatisticsController : BaseUserApiController
    {
        private static readonly BuyStatisticsBiz Biz = new BuyStatisticsBiz();

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
        public IResponseResult GetBuyStatisticsResults(BuyStatisticsRequest request)
        {
            var result = Biz.GetBuyStatisticsResults(request);
            return ReturnResult(result);
        }

        /// <summary>
        /// 收款图表统计
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult GetBuyStatisticsChart(BuyChartRequest request)
        {
            var result = Biz.GetBuyStatisticsChart(request);
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
        public IResponseResult GetBuyProductStatistics(DateTime beginDatetime, DateTime endDatetime, int page, int limit, int? categoryId = null, string keyword = null, Guid? userId = null)
        {
            return new BuyProductStatisticsBiz().GetBuyProductStatistics(UserModel, beginDatetime, endDatetime, userId, page, limit, categoryId, keyword);
        }

        #region 

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

        #endregion
    }
}
