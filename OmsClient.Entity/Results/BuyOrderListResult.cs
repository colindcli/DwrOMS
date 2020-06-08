using System;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Entity.Results
{
    public class BuyOrderListResult
    {
        /// <summary>
        /// 
        /// </summary>
        public virtual Guid BuyOrderId { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public virtual string BuyOrderNumber { get; set; }
        /// <summary>
        /// 币种，如：人民币
        /// </summary>
        public virtual string CurrencyName { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public virtual string Title { get; set; }
        /// <summary>
        /// 收货人
        /// </summary>
        public virtual string SupplierName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateDate { get; set; }
        /// <summary>
        /// 备注。告诉仓库备货信息
        /// </summary>
        public virtual string DefaultRemark { get; set; }

        /// <summary>
        /// 提交审核
        /// </summary>
        public virtual DateTime? PostCheckDate { get; set; }
        /// <summary>
        /// 审核通过或驳回时间
        /// </summary>
        public virtual DateTime? CheckDate { get; set; }
        /// <summary>
        /// 提交采购
        /// </summary>
        public virtual DateTime? PostPurcharseDate { get; set; }

        /// <summary>
        /// 付款时间
        /// </summary>
        public virtual DateTime? PayFinishDate { get; set; }

        /// <summary>
        /// 提交采购
        /// </summary>
        public virtual DateTime? PostDate { get; set; }

        /// <summary>
        /// 状态：1草稿订单；2提交备货未收款；3提交备货已收款；4已备货未收款；5已备货已收款；6已发货未收款；7已发货已收款；
        /// </summary>
        public virtual int Status { get; set; }

        /// <summary>
        /// 采购员
        /// </summary>
        public virtual string UserNickName { get; set; }

        /// <summary>
        /// 入库时间
        /// </summary>
        public virtual DateTime? StockInDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string DefaultSettlementName { get; set; }
    }
}
