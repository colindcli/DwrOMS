using OmsClient.Entity.Statics;
using System.ComponentModel.DataAnnotations.Schema;
using OmsClient.Entity.Results;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Entity
{
    /// <summary>
    /// 
    /// </summary>
    public partial class SaleOrder
    {
        /// <summary>
        /// ��ʽ
        /// </summary>
        [NotMapped]
        public string DefaultSettlementName
        {
            get
            {
                return StaticValue.SaleDefaultSettlement.Find(p => p.Key == DefaultSettlement)?.Value;
            }
        }
    }
}