using System.Collections.Generic;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Entity.Results
{
    public class FinanceAccountCurrencyResult
    {
        /// <summary>
        /// 账号
        /// </summary>
        public List<Account> Accounts { get; set; }

        /// <summary>
        /// 币种
        /// </summary>
        public List<Currency> Currencies { get; set; }
    }

    public class SaleReceiveResult
    {
        public List<SaleReceive> SaleReceives { get; set; }

        /// <summary>
        /// 合计：折合人民币
        /// </summary>
        public decimal TotalRmb { get; set; }

        /// <summary>
        /// 收款状态
        /// </summary>
        public string ReceiveStatusName { get; set; }
        /// <summary>
        /// 收款状态类名
        /// </summary>
        public string ReceiveStatusClass { get; set; }
    }

    public class BuyPayResult
    {
        public List<BuyPay> BuyPays { get; set; }

        /// <summary>
        /// 合计：折合人民币
        /// </summary>
        public decimal TotalRmb { get; set; }

        /// <summary>
        /// 收款状态
        /// </summary>
        public string ReceiveStatusName { get; set; }
        /// <summary>
        /// 收款状态类名
        /// </summary>
        public string ReceiveStatusClass { get; set; }
    }
}
