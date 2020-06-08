using System;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Entity.Models
{
    public class BuyOrderCheckModel
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid BuyOrderId { get; set; }

        /// <summary>
        /// 审核通过
        /// </summary>
        public bool Pass { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
