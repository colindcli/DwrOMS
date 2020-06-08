using OmsClient.Entity;
using OmsClient.Entity.Enums;
using System.Collections.Generic;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Biz.Warehouses
{
    public abstract class BaseOrderBiz
    {
        /// <summary>
        /// 验证权限：公司
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="product"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        protected static bool CheckAuthProduct(UserModel userModel, Product product, out string msg)
        {
            if (product == null)
            {
                msg = "产品不存在，操作失败";
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
        /// 验证权限：公司、入库
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="order"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        protected static bool CheckAuthOrderByStatusStockIn(UserModel userModel, BuyOrder order, out string msg)
        {
            if (order == null)
            {
                msg = "订单不存在，操作失败";
                return false;
            }

            var status = new List<int>()
            {
                (int)BuyOrderStatusEnum.Purchase,
                (int)BuyOrderStatusEnum.PurchasePay,
            };
            if (!status.Contains(order.Status))
            {
                msg = "状态已改变，操作失败";
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
    }
}
