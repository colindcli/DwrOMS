using System;
using System.Collections.Generic;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Entity.Results
{
    public class SaleOrderResult
    {
        /// <summary>
        /// 订单
        /// </summary>
        public SaleOrder SaleOrder { get; set; }
        /// <summary>
        /// 币种
        /// </summary>
        public List<Currency> Currencies { get; set; }

        /// <summary>
        /// 备货信息
        /// </summary>
        public SaleOrderStockingResult StockInfo { get; set; }

        /// <summary>
        /// 统计
        /// </summary>
        public SaleOrderDetailCount Count { get; set; }
    }

    /// <summary>
    /// 订单产品
    /// Product字段
    /// </summary>
    public partial class SaleOrderProductList
    {
        /// <summary>
        /// 商品名称
        /// </summary>
        public virtual string ProductName { get; set; }
        /// <summary>
        /// 商品规格
        /// </summary>
        public virtual string ProductSpecification { get; set; }
        /// <summary>
        /// 商品描述
        /// </summary>
        public virtual string ProductRemark { get; set; }
        /// <summary>
        /// 重量(克)
        /// </summary>
        public virtual int ProductWeight { get; set; }
        /// <summary>
        /// 在途数量
        /// </summary>
        public virtual int InTransitQty { get; set; }
        /// <summary>
        /// 可售数量
        /// </summary>
        public virtual int SaleQty { get; set; }
        /// <summary>
        /// 挂起数量
        /// </summary>
        public virtual int HoldQty { get; set; }
        /// <summary>
        /// 起订数量
        /// </summary>
        public virtual int OrderQty { get; set; }
        /// <summary>
        /// 售价1
        /// </summary>
        public virtual decimal Price1 { get; set; }
        /// <summary>
        /// 售价10
        /// </summary>
        public virtual decimal Price10 { get; set; }
        /// <summary>
        /// 售价100
        /// </summary>
        public virtual decimal Price100 { get; set; }
        /// <summary>
        /// 图片数
        /// </summary>
        public virtual int ImageQty { get; set; }
        /// <summary>
        /// 存仓位置
        /// </summary>
        public virtual string AreaPosition { get; set; }
    }

    /// <summary>
    /// 订单产品
    /// 添加字段
    /// </summary>
    public partial class SaleOrderProductList : SaleOrderProduct
    {
        /// <summary>
        /// 币种符号
        /// </summary>
        public string Symbol { get; set; }
        /// <summary>
        /// 转换成订单的币种
        /// </summary>
        public decimal PriceA { get; set; }
        /// <summary>
        /// 转换成订单的币种
        /// </summary>
        public decimal PriceB { get; set; }
        /// <summary>
        /// 转换成订单的币种
        /// </summary>
        public decimal PriceC { get; set; }

        /// <summary>
        /// 分类
        /// </summary>
        public string CategoryName { get; set; }
    }

    /// <summary>
    /// 选择产品
    /// </summary>
    public partial class SaleOrderProductSelect : Product
    {
        /// <summary>
        /// 币种符号
        /// </summary>
        public string Symbol { get; set; }
        /// <summary>
        /// 转换成订单的币种
        /// </summary>
        public decimal PriceA { get; set; }
        /// <summary>
        /// 转换成订单的币种
        /// </summary>
        public decimal PriceB { get; set; }
        /// <summary>
        /// 转换成订单的币种
        /// </summary>
        public decimal PriceC { get; set; }

        /// <summary>
        /// 已经添加到订单了
        /// </summary>
        public bool IsAdded { get; set; }
    }

    public class SaleOrderDetailCount
    {
        /// <summary>
        /// 币种符号：$
        /// </summary>
        public virtual string CurrencySymbol { get; set; }

        /// <summary>
        /// 产品中重量
        /// </summary>
        public int ProductWeights { get; set; }
        /// <summary>
        /// 总数量
        /// </summary>
        public int ProductQtys { get; set; }
        /// <summary>
        /// 产品小计
        /// </summary>
        public decimal ProductAmount { get; set; }

        /// <summary>
        /// 运费。（订单币种）
        /// </summary>
        public virtual decimal ShipFeight { get; set; }
        /// <summary>
        /// 其他费用。（订单币种）
        /// </summary>
        public virtual decimal ShipFee { get; set; }
        /// <summary>
        /// 折扣费用。（订单币种）
        /// </summary>
        public virtual decimal Discount { get; set; }

        /// <summary>
        /// 合计
        /// </summary>
        public decimal Total { get; set; }
        /// <summary>
        /// 折合人民币
        /// </summary>
        public decimal TotalRmb { get; set; }
    }

    public class SaleUpdateSaleOrderProductRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid SaleOrderId { get; set; }

        public List<SaleOrderProduct> SaleOrderProducts { get; set; }
    }
}
