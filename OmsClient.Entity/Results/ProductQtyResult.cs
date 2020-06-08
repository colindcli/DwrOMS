using System;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Entity.Results
{
    public class InTransitQtyResult
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid BuyOrderId { get; set; }

        /// <summary>
        /// 采购单号
        /// </summary>
        public string  OrderNumber { get; set; }
        /// <summary>
        /// 采购员
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 采购数量
        /// </summary>
        public int Qty { get; set; }
        /// <summary>
        /// 到达日期
        /// </summary>
        public string ArrivalsDate { get; set; }

        public string StatusName { get; set; }
    }
    public class HoldQtyResult
    {
        public Guid SaleOrderId { get; set; }
        /// <summary>
        /// 采购单号
        /// </summary>
        public string OrderNumber { get; set; }
        /// <summary>
        /// 采购员
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 采购数量
        /// </summary>
        public int Qty { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string StatusName { get; set; }
        /// <summary>
        /// 天数
        /// </summary>
        public int Days { get; set; }
    }
}
