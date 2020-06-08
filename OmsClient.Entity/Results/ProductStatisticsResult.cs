/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Entity.Results
{
    /// <summary>
    /// 
    /// </summary>
    public class StockCountResult : Product
    {
        /// <summary>
        /// 数量
        /// </summary>
        public int Qty { get; set; }
        /// <summary>
        /// 小计
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 均价
        /// </summary>
        public decimal? Avg { get; set; }
    }
}
