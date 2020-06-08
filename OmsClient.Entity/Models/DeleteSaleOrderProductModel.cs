using System;
using System.Collections.Generic;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Entity.Models
{
    public class DeleteSaleOrderProductModel
    {
        public Guid SaleOrderId { get; set; }
        public List<Guid> SaleOrderProductIds { get; set; }
    }
}
