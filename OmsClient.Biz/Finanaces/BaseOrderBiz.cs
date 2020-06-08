using OmsClient.Entity;
using OmsClient.Entity.Enums;
using System.Collections.Generic;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Biz.Finanaces
{
    public abstract class BaseOrderBiz
    {
        /// <summary>
        /// 验证权限：公司
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="order"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        protected static bool CheckAuthOrder(UserModel userModel, SaleOrder order, out string msg)
        {
            if (order == null)
            {
                msg = "订单不存在，操作失败";
                return false;
            }

            msg = null;
            return true;
        }
        /// <summary>
        /// 验证权限：公司
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="order"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        protected static bool CheckAuthOrder(UserModel userModel, BuyOrder order, out string msg)
        {
            if (order == null)
            {
                msg = "订单不存在，操作失败";
                return false;
            }

            msg = null;
            return true;
        }

        /// <summary>
        /// 验证权限：公司
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="account"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        protected static bool CheckAuthAccount(UserModel userModel, Account account, out string msg)
        {
            if (account == null)
            {
                msg = "账号不存在，操作失败";
                return false;
            }

            msg = null;
            return true;
        }

        /// <summary>
        /// 验证权限：公司
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="currency"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        protected static bool CheckAuthCurrency(UserModel userModel, Currency currency, out string msg)
        {
            if (currency == null)
            {
                msg = "币种不存在，操作失败";
                return false;
            }

            msg = null;
            return true;
        }

        /// <summary>
        /// 验证权限：公司
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="order"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        protected static bool CheckAuthFinanceOrder(UserModel userModel, BuyOrder order, out string msg)
        {
            var statusFinished = new List<int>()
            {
                (int) BuyOrderStatusEnum.PurchasePay,
                (int) BuyOrderStatusEnum.StockInPay,
            };

            if (statusFinished.Exists(p => p == order.Status))
            {
                msg = "订单已付款完结，操作失败";
                return false;
            }

            var status = new List<int>()
            {
                (int) BuyOrderStatusEnum.PurchaseUnpay,
                (int) BuyOrderStatusEnum.StockInUnpay,
            };

            if (!status.Exists(p => p == order.Status))
            {
                msg = "订单已锁定，操作失败";
                return false;
            }

            msg = null;
            return true;
        }


    }
}
