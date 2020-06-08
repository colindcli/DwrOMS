/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Entity.Enums
{
    /// <summary>
    /// 状态：1草稿采购单；2提交审核；3审核驳回；4审核通过；5采购中不用付款；6采购中等待付款；7采购中已付款；8已入库未付款；9已入库已付款；
    /// 1 Draft;
    /// 2 SubmitCheck;
    /// 3 SubmitBack;
    /// 4 SubmitPass;
    /// 5 Purchase;
    /// 6 PurchaseUnpay;
    /// 7 PurchasePay;
    /// 8 StockInUnpay;
    /// 9 StockInPay;
    /// </summary>
    public enum BuyOrderStatusEnum
    {
        /// <summary>
        /// 1草稿采购单
        /// </summary>
        Draft = 1,

        /// <summary>
        /// 2提交审核
        /// </summary>
        SubmitCheck = 2,
        /// <summary>
        /// 3审核驳回
        /// </summary>
        SubmitBack = 3,
        /// <summary>
        /// 4审核通过
        /// </summary>
        SubmitPass = 4,

        /// <summary>
        /// 5采购中不用付款
        /// </summary>
        Purchase = 5,
        /// <summary>
        /// 6采购中等待付款
        /// </summary>
        PurchaseUnpay = 6,
        /// <summary>
        /// 7采购中已付款
        /// </summary>
        PurchasePay = 7,

        /// <summary>
        /// 8已入库未付款
        /// </summary>
        StockInUnpay = 8,
        /// <summary>
        /// 9已入库已付款
        /// </summary>
        StockInPay = 9,

        //如果这里添加，需要在UserBuyOrderBiz.GetBuyOrderDetailCount()检查是否已入库
    }
}
