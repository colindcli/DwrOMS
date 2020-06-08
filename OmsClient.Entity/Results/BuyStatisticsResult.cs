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

    public class BuyChartDate
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

    public class BuyChartRequest
    {
        public Guid? UserId { get; set; }
        /// <summary>
        /// 类型：1按月；2按周；3按日
        /// </summary>
        public int Type { get; set; }
    }

    public class BuyChartResult
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
        public List<BuySeriesItem> SeriesItems { get; set; }
    }

    public class BuySeriesItem
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
    public class BuyStatisticsResult
    {
        public List<BuyStatisticsModel> Rows { get; set; }
        /// <summary>
        /// 总计
        /// </summary>
        public BuyStatisticsModel Total { get; set; }
    }

    public class BuyStatisticsModel
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
        /// BuyOrderId
        /// </summary>
        public Guid BuyOrderId { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string BuyOrderNumber { get; set; }
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
        /// 应收总计
        /// </summary>
        public decimal Amount { get; set; }
        
        /// <summary>
        /// 实付总计
        /// </summary>
        public decimal PayTotal { get; set; }
    }

    public class BuyStatisticsTemp : BuyStatisticsModel
    {

        public decimal CurrencyRate { get; set; }
    }

    public class BuyStatisticsCount
    {
        public Guid BuyOrderId { get; set; }

        public decimal Amount { get; set; }
    }

    public class BuyStatisticsRequest
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

    public class BuyStatisticsUser
    {
        public Guid UserId { get; set; }

        public string UserName { get; set; }
    }

    public class BuyStatisticsInit
    {
        public List<BuyStatisticsUser> Row { get; set; }

        public List<string> Dates { get; set; }
    }
    #endregion
}
