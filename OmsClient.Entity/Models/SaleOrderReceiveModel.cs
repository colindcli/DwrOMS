using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Entity.Models
{
    public class SaleOrderReceiveModel
    {
        public Guid SaleOrderId { get; set; }
        /// <summary>
        /// 1后付款；2完成付款
        /// </summary>
        public int SelectPay { get; set; }
    }
}
