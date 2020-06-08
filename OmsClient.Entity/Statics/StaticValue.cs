using DwrUtility;
using System.Collections.Generic;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Entity.Statics
{
    public class StaticValue
    {
        /// <summary>
        /// 销售
        /// </summary>
        public static List<KeyValue<int, string>> SaleDefaultSettlement = new List<KeyValue<int, string>>()
        {
            new KeyValue<int, string>(){Key = 1, Value = "收款发货"},
            new KeyValue<int, string>(){Key = 2, Value = "自提结款"},
            new KeyValue<int, string>(){Key = 3, Value = "送货结款"},
            new KeyValue<int, string>(){Key = 4, Value = "邮到结款"},
            new KeyValue<int, string>(){Key = 5, Value = "周结"},
            new KeyValue<int, string>(){Key = 6, Value = "半月结"},
            new KeyValue<int, string>(){Key = 7, Value = "月结"},
            new KeyValue<int, string>(){Key = 8, Value = "季结"},
            new KeyValue<int, string>(){Key = 9, Value = "半年结"},
            new KeyValue<int, string>(){Key = 10, Value = "年结"},
            new KeyValue<int, string>(){Key = 11, Value = "其他"},
        };

        /// <summary>
        /// 采购：结款方式：1付款后发货；2自提并付款；3收货后付款；4周付；5半月付；6月付；7季付；8半年付；9年付；10其他
        /// </summary>
        public static List<KeyValue<int, string>> BuyDefaultSettlement = new List<KeyValue<int, string>>()
        {
            new KeyValue<int, string>(){Key = 1, Value = "付款后发货"},
            new KeyValue<int, string>(){Key = 2, Value = "自提并付款"},
            new KeyValue<int, string>(){Key = 3, Value = "收货后付款"},
            new KeyValue<int, string>(){Key = 4, Value = "周结"},
            new KeyValue<int, string>(){Key = 5, Value = "半月结"},
            new KeyValue<int, string>(){Key = 6, Value = "月结"},
            new KeyValue<int, string>(){Key = 7, Value = "季结"},
            new KeyValue<int, string>(){Key = 8, Value = "半年结"},
            new KeyValue<int, string>(){Key = 9, Value = "年结"},
            new KeyValue<int, string>(){Key = 10, Value = "其他"},
        };
    }
}
