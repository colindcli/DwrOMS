using System;
using System.Collections.Generic;
// ReSharper disable InconsistentNaming

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Entity.Results
{
    #region 图表

    public class SaleChartDate
    {
        public int Index { get; set; }
        /// <summary>
        /// 开始
        /// </summary>
        public DateTime Begin { get; set; }
        /// <summary>
        /// 结束
        /// </summary>
        public DateTime End { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 结束的下一天
        /// </summary>
        public DateTime EndNextDay { get; set; }
    }

    public class SaleChartRequest
    {
        public Guid? UserId { get; set; }
        /// <summary>
        /// 类型：1按月；2按周；3按日
        /// </summary>
        public int Type { get; set; }
    }

    public class SaleChartResult
    {
        public string Title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> LegendData { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> XaxisData { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<SaleSeriesItem> SeriesItems { get; set; }
    }

    public class SaleSeriesItem
    {
        /// <summary>
        /// 邮件营销
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 总量
        /// </summary>
        public string stack { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<decimal> data { get; set; }
    }
    #endregion

    #region 列表
    public class SaleStatisticsResult
    {
        public List<SaleStatisticsModel> Rows { get; set; }
        /// <summary>
        /// 总计
        /// </summary>
        public SaleStatisticsModel Total { get; set; }
    }

    public class SaleStatisticsModel
    {
        /// <summary>
        /// Index
        /// </summary>
        public int I { get; set; } = -1;
        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// SaleOrderId
        /// </summary>
        public Guid SaleOrderId { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string SaleOrderNumber { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 预估运费
        /// </summary>
        public decimal ShipFeight { get; set; }
        /// <summary>
        /// 预估费用
        /// </summary>
        public decimal ShipFee { get; set; }
        /// <summary>
        /// 折扣
        /// </summary>
        public decimal Discount { get; set; }

        /// <summary>
        /// 结算时间
        /// </summary>
        public DateTime? DateTime { get; set; }

        /// <summary>
        /// 产品总额
        /// </summary>
        public decimal Product { get; set; }
        /// <summary>
        /// 应收合计
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 收款总计
        /// </summary>
        public decimal RealRecive { get; set; }
        /// <summary>
        /// 交易费用
        /// </summary>
        public decimal RealFee { get; set; }
        /// <summary>
        /// 实际运费
        /// </summary>
        public decimal RealFeight { get; set; }
        /// <summary>
        /// 扣除运费和费用后剩余到账
        /// </summary>
        public decimal RealTotal { get; set; }
    }

    public class SaleStatisticsTemp : SaleStatisticsModel
    {

        public decimal CurrencyRate { get; set; }
    }

    public class SaleStatisticsCount
    {
        public Guid SaleOrderId { get; set; }

        public decimal Amount { get; set; }
        public decimal Fee { get; set; }
    }

    public class SaleStatisticsRequest
    {
        public Guid? UserId { get; set; }
        /// <summary>
        /// 包含当天
        /// </summary>
        public DateTime BeginDatetime { get; set; }
        /// <summary>
        /// 包含当天
        /// </summary>
        public DateTime EndDatetime { get; set; }
    }

    public class SaleStatisticsUser
    {
        public Guid UserId { get; set; }

        public string UserName { get; set; }
    }

    public class SaleStatisticsInit
    {
        public List<SaleStatisticsUser> Row { get; set; }

        public List<string> Dates { get; set; }
    }
    #endregion
}
