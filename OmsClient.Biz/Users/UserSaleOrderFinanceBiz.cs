using OmsClient.Entity;
using OmsClient.Entity.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Biz.Users
{
    /// <summary>
    /// 财务对销售单收款
    /// </summary>
    public partial class UserSaleOrderBiz
    {
        /// <summary>
        /// 收款记录
        /// </summary>
        /// <returns></returns>
        public List<SaleReceive> GetSaleOrderReceive(UserModel userModel, Guid saleOrderId, out string msg)
        {
            var order = Da.Get<SaleOrder>(saleOrderId);
            var b1 = CheckAuthOrderByUser(userModel, order, out msg);
            if (!b1)
            {
                return null;
            }

            var list = Da.GetList<SaleReceive>(new
            {
                userModel.UserId,
                order.SaleOrderId
            }).OrderBy(p => p.CreateDate).ToList();

            return list;
        }

        /// <summary>
        /// 添加款项
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="request"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool AddSaleOrderReceive(UserModel userModel, SaleReceive request, out string msg)
        {
            var order = Da.Get<SaleOrder>(request.SaleOrderId);
            var b1 = CheckAuthOrder(userModel, order, out msg);
            if (!b1)
            {
                return false;
            }

            if (order.Status == (int)SaleOrderStatusEnum.Draft || order.Status == (int)SaleOrderStatusEnum.ShipedPay)
            {
                msg = "添加失败";
                return false;
            }

            var currency = Da.Get<Currency>(request.CurrencyId);
            var b2 = CheckAuthCurrency(userModel, currency, out msg);
            if (!b2)
            {
                return false;
            }

            var accout = Da.Get<Account>(request.AccountId);
            var b3 = CheckAuthAccount(userModel, accout, out msg);
            if (!b3)
            {
                return false;
            }

            var m = new SaleReceive()
            {
                SaleReceiveId = SeqGuid.NewGuid(),
                SaleOrderId = order.SaleOrderId,
                AccountInfo = $"{accout.AccountBank}[{accout.AccountNumber}]",
                Amount = request.Amount,
                CurrencySymbol = currency.CurrencySymbol,
                CurrencyRate = currency.CurrencyRate,
                Remark = null,
                CreateName = userModel.UserNickName,
                CreateDate = DateTime.Now,
            };

            Da.Add<Guid, SaleReceive>(m);
            return true;
        }

        /// <summary>
        /// 删除款项
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="request"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool DeleteSaleOrderReceive(UserModel userModel, SaleReceive request, out string msg)
        {
            var m = Da.Get<SaleReceive>(request.SaleReceiveId);
            if (m == null)
            {
                msg = "订单不存在，操作失败";
                return false;
            }

            var order = Da.Get<SaleOrder>(m.SaleOrderId);
            var b1 = CheckAuthOrder(userModel, order, out msg);
            if (!b1)
            {
                return false;
            }

            if (order.Status == (int)SaleOrderStatusEnum.Draft || order.Status == (int)SaleOrderStatusEnum.ShipedPay)
            {
                msg = "删除失败";
                return false;
            }

            return Da.Delete(m);
        }
    }
}
