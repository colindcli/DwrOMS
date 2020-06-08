using DwrUtility;
using OmsClient.Entity.Enums;
using OmsClient.Entity.Statics;
using System;
using System.Collections.Generic;
using System.Configuration;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Common
{
    public class Config
    {
        public static readonly Dictionary<int, string> DictReturnCode = EnumHelper.GetValueDescriptionDict<ReturnCode>();

        /// <summary>
        /// 根目录：不包含“/”
        /// </summary>
        public static readonly string Root = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('/', '\\');

        /// <summary>
        /// 采购单是否需要审核
        /// </summary>
        public static readonly bool IsBuyCheck = GetValue("IsBuyCheck").IsEquals("true");

        private static string GetValue(string name)
        {
            return ConfigurationManager.AppSettings[name] ?? "";
        }

        //结款方式：1收款发货；2自提结款；3送货结款；4邮到结款；5周结；6半月结；7月结；8季结；9半年结；10年结；11其他
        public static List<KeyValue<int, string>> SaleDefaultSettlement => StaticValue.SaleDefaultSettlement;

        //结款方式：1付款后发货；2自提并付款；3收货后付款；4周付；5半月付；6月付；7季付；8半年付；9年付；10其他
        public static List<KeyValue<int, string>> BuyDefaultSettlement => StaticValue.BuyDefaultSettlement;

        /// <summary>
        /// Token
        /// </summary>
        public static string TokenSecret = GetValue("TokenSecret");

        /// <summary>
        /// 地址，不含“/”
        /// </summary>
        public static string WebUrl = GetValue("WebUrl").TrimEnd('/').Trim();

        /// <summary>
        /// 静态文件版本
        /// </summary>
        public static string StaticVersion = GetValue("StaticVersion").Trim();
    }
}
