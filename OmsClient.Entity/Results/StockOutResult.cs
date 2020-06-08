using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Entity.Results
{
    public class StockOutResult
    {
        /// <summary>
        /// 产品Id
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// 销售单号
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// 销售员
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 产品编码
        /// </summary>
        public string Sku { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 产品规格
        /// </summary>
        public string Specification { get; set; }
        /// <summary>
        /// 产品描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 图片数量
        /// </summary>
        public int ImageQty { get; set; }
        /// <summary>
        /// 销售价格，已转人民币
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 销售数量
        /// </summary>
        public int Qty { get; set; }
        /// <summary>
        /// 销售备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 出库时间
        /// </summary>
        public DateTime CreateDate { get; set; }
    }

    public class StockInResult
    {
        /// <summary>
        /// 产品Id
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// 销售单号
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// 销售员
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 产品编码
        /// </summary>
        public string Sku { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 产品规格
        /// </summary>
        public string Specification { get; set; }
        /// <summary>
        /// 产品描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 图片数量
        /// </summary>
        public int ImageQty { get; set; }
        /// <summary>
        /// 销售价格，已转人民币
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 销售数量
        /// </summary>
        public int Qty { get; set; }
        /// <summary>
        /// 销售备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 出库时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 入库备注
        /// </summary>
        public string InStockRemark { get; set; }
    }
}
