using OmsClient.Biz.Bases;
using OmsClient.DataAccess.Finances;
using OmsClient.Entity;
using OmsClient.Entity.Enums;
using OmsClient.Entity.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using OmsClient.DataAccess.Users;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Biz.Finanaces
{
    public class FinanceBuyOrderBiz : BaseBuyBiz
    {
        private static readonly FinanceBuyOrderDa Da = new FinanceBuyOrderDa();

        /// <summary>
        /// 统计订单数量
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        public List<int> GetMyOrderNumber(UserModel userModel)
        {
            var list = new UserBuyOrderDa().GetSaleOrderNumberList();

            var result = new List<int>();

            //未付款采购单
            var a = list.Count(p => new List<int>()
            {
                (int)BuyOrderStatusEnum.PurchaseUnpay,
                (int)BuyOrderStatusEnum.StockInUnpay,
            }.Contains(p.Status));
            result.Add(a);

            //已付款采购单
            var b = list.Count(p => new List<int>()
            {
                (int)BuyOrderStatusEnum.PurchasePay,
                (int)BuyOrderStatusEnum.StockInPay,
            }.Contains(p.Status));
            result.Add(b);
            
            return result;
        }

        /// <summary>
        /// 未付款采购单
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual PagenationResult<BuyOrderListResult> GetUnpayOrder(UserModel userModel, int pageIndex, int pageSize, string keyword)
        {
            return GetListByUserStatus(userModel, pageIndex, pageSize, keyword, new List<int>()
            {
                (int)BuyOrderStatusEnum.PurchaseUnpay,
                (int)BuyOrderStatusEnum.StockInUnpay,
            }, list => list.OrderByDescending(p => p.CreateDate).ToList());
        }

        /// <summary>
        /// 已付款采购单
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual PagenationResult<BuyOrderListResult> GetPaidOrder(UserModel userModel, int pageIndex, int pageSize, string keyword)
        {
            return GetListByUserStatus(userModel, pageIndex, pageSize, keyword, new List<int>()
            {
                (int)BuyOrderStatusEnum.PurchasePay,
                (int)BuyOrderStatusEnum.StockInPay,
            }, list => list.OrderByDescending(p => p.CreateDate).ToList());
        }


        /// <summary>
        /// 完成付款
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="request"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool PostFininshPay(UserModel userModel, BuyOrder request, out string msg)
        {
            var order = Da.Get<BuyOrder>(request.BuyOrderId);
            var b1 = CheckAuthOrder(userModel, order, out msg);
            if (!b1)
            {
                return false;
            }

            if (order.Status == (int)BuyOrderStatusEnum.Draft)
            {
                msg = "订单已退回修改，操作失败";
                return false;
            }

            if (order.Status != (int)BuyOrderStatusEnum.PurchaseUnpay &&
                order.Status != (int)BuyOrderStatusEnum.StockInUnpay)
            {
                msg = "订单可能已提交，操作失败";
                return false;
            }

            if (order.Status == (int)BuyOrderStatusEnum.PurchaseUnpay)
            {
                order.Status = (int)BuyOrderStatusEnum.PurchasePay;
            }
            else if (order.Status == (int)BuyOrderStatusEnum.StockInUnpay)
            {
                order.Status = (int)BuyOrderStatusEnum.StockInPay;
            }
            else
            {
                throw new Exception("操作失败");
            }

            order.PayFinishDate = DateTime.Now;
            order.PayFinishUserName = userModel.UserNickName;
            return Da.Update(order);
        }
    }
}
