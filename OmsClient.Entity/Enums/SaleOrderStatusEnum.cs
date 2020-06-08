/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Entity.Enums
{
    /// <summary>
    /// 状态：1草稿订单；2提交备货未收款；3提交备货后收款；4提交备货已收款；5已备货未收款；6已备货后收款；7已备货已收款；8已发货后收款；9已发货已收款；
    /// 1 Draft;
    /// 2 SubmitUnpay;
    /// 3 SubmitAfterPay;
    /// 4 SubmitPay;
    /// 5 StockedUnpay;
    /// 6 StockedAfterPay;
    /// 7 StockedPay;
    /// 8 ShipedAfterPay;
    /// 9 ShipedPay;
    /// </summary>
    public enum SaleOrderStatusEnum
    {
        /// <summary>
        /// 1草稿订单
        /// </summary>
        Draft = 1,

        /// <summary>
        /// 2提交备货未收款
        /// </summary>
        SubmitUnpay = 2,
        /// <summary>
        /// 3提交备货后收款
        /// </summary>
        SubmitAfterPay = 3,
        /// <summary>
        /// 4提交备货已收款
        /// </summary>
        SubmitPay = 4,
        /// <summary>
        /// 5已备货未收款
        /// </summary>
        StockedUnpay = 5,

        /// <summary>
        /// 6已备货后收款
        /// </summary>
        StockedAfterPay = 6,
        /// <summary>
        /// 7已备货已收款
        /// </summary>
        StockedPay = 7,

        /// <summary>
        /// 8已发货后收款
        /// </summary>
        ShipedAfterPay = 8,
        /// <summary>
        /// 9已发货已收款
        /// </summary>
        ShipedPay = 9,
    }
}
