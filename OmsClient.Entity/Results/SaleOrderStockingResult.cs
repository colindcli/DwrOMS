/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Entity.Results
{
    public class SaleOrderStockingResult
    {
        /// <summary>
        /// 
        /// </summary>
        public string StockStartInfo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string StockEndInfo { get; set; }

        /// <summary>
        /// 是否开始
        /// </summary>
        public bool ShowStartBtn { get; set; }
        /// <summary>
        /// 是否完成
        /// </summary>
        public bool ShowEndBtn { get; set; }
    }
}
