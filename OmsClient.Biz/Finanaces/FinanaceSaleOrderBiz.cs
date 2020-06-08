using DwrUtility;
using DwrUtility.Strings;
using OmsClient.Common;
using OmsClient.DataAccess.Finances;
using OmsClient.Entity;
using OmsClient.Entity.Enums;
using OmsClient.Entity.Models;
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
    public class FinanaceSaleOrderBiz : BaseOrderBiz
    {
        private static readonly FinanaceSaleOrderDa Da = new FinanaceSaleOrderDa();

        /// <summary>
        /// 统计订单数量
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        public List<int> GetMyOrderNumber(UserModel userModel)
        {
            var list = new UserSaleOrderDa().GetSaleOrderNumberList();

            var result = new List<int>();

            //待收款销售单
            var a = list.Count(p => new List<int>()
            {
                (int)SaleOrderStatusEnum.SubmitUnpay,
                (int)SaleOrderStatusEnum.StockedUnpay,
            }.Contains(p.Status));
            result.Add(a);

            //后收款销售单
            var b = list.Count(p => new List<int>()
            {
                (int)SaleOrderStatusEnum.SubmitAfterPay,
                (int)SaleOrderStatusEnum.StockedAfterPay,
                (int)SaleOrderStatusEnum.ShipedAfterPay,
            }.Contains(p.Status));
            result.Add(b);

            //已收款销售单
            var c = list.Count(p => new List<int>()
            {
                (int)SaleOrderStatusEnum.SubmitPay,
                (int)SaleOrderStatusEnum.StockedPay,
                (int)SaleOrderStatusEnum.ShipedPay,
            }.Contains(p.Status));
            result.Add(c);

            return result;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <param name="status"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        private static PagenationResult<SaleOrderListResult> GetListByStatus(UserModel userModel, int pageIndex, int pageSize, string keyword, List<int> status, Func<List<SaleOrder>, List<SaleOrder>> orderBy)
        {
            var list = Da.GetListByBulk<SaleOrder>(new
            {
                Status = status
            });

            //排序
            list = orderBy.Invoke(list);

            if (!keyword.IsWhiteSpace())
            {
                var keys = keyword.ToKeys(StringComparer.OrdinalIgnoreCase).ToList();
                list = list.Where(p => $"{p.SaleOrderNumber} {p.CurrencyName} {p.CurrencyText} {p.CurrencySymbol} {p.Title} {p.ToConsignee} {p.ToCompanyName} {p.ToTelphone} {p.ToAddress} {p.ToRemark} {p.StockStartName} {p.StockEndName} {p.StockRemark}".HasSearchKeys(keys, StringComparison.OrdinalIgnoreCase, false)).ToList();
            }
            var result = new PagenationResult<SaleOrderListResult>()
            {
                count = list.Count,
                data = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToSaleOrderListResult()
            };
            return result;
        }

        /// <summary>
        /// 待收款销售单
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual PagenationResult<SaleOrderListResult> GetNotReceiveOrder(UserModel userModel, int pageIndex, int pageSize, string keyword)
        {
            return GetListByStatus(userModel, pageIndex, pageSize, keyword, new List<int>()
            {
                (int)SaleOrderStatusEnum.SubmitUnpay,
                (int)SaleOrderStatusEnum.StockedUnpay,
            }, list => list.OrderByDescending(p => p.PostDate).ToList());
        }

        /// <summary>
        /// 后收款销售单
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual PagenationResult<SaleOrderListResult> GetAfterOrder(UserModel userModel, int pageIndex, int pageSize, string keyword)
        {
            return GetListByStatus(userModel, pageIndex, pageSize, keyword, new List<int>()
            {
                (int)SaleOrderStatusEnum.SubmitAfterPay,
                (int)SaleOrderStatusEnum.StockedAfterPay,
                (int)SaleOrderStatusEnum.ShipedAfterPay,
            }, list => list.OrderByDescending(p => p.FinancePostDate).ToList());
        }

        /// <summary>
        /// 已收款销售单
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual PagenationResult<SaleOrderListResult> GetHadReceiveOrder(UserModel userModel, int pageIndex, int pageSize, string keyword)
        {
            return GetListByStatus(userModel, pageIndex, pageSize, keyword, new List<int>()
            {
                (int)SaleOrderStatusEnum.SubmitPay,
                (int)SaleOrderStatusEnum.StockedPay,
                (int)SaleOrderStatusEnum.ShipedPay,
            }, list => list.OrderByDescending(p => p.FinancePostDate).ToList());
        }

        /// <summary>
        /// 待收款提交
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="request"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool PostUnpayReceive(UserModel userModel, SaleOrderReceiveModel request, out string msg)
        {
            if (request.SelectPay != 1 && request.SelectPay != 2)
            {
                msg = "没选择结算方式，操作失败";
                return false;
            }

            var order = Da.Get<SaleOrder>(request.SaleOrderId);
            var b1 = CheckAuthOrder(userModel, order, out msg);
            if (!b1)
            {
                return false;
            }

            if (order.Status == (int)SaleOrderStatusEnum.Draft)
            {
                msg = "订单已退回修改，操作失败";
                return false;
            }

            if (order.Status != (int)SaleOrderStatusEnum.SubmitUnpay &&
                order.Status != (int)SaleOrderStatusEnum.StockedUnpay)
            {
                msg = "订单可能已提交，操作失败";
                return false;
            }

            if (order.Status == (int)SaleOrderStatusEnum.SubmitUnpay)
            {
                if (request.SelectPay == 1)
                {
                    order.Status = (int)SaleOrderStatusEnum.SubmitAfterPay;
                }
                else if (request.SelectPay == 2)
                {
                    order.Status = (int)SaleOrderStatusEnum.SubmitPay;
                }
                else
                {
                    throw new Exception("操作失败");
                }
            }
            else if (order.Status == (int)SaleOrderStatusEnum.StockedUnpay)
            {
                if (request.SelectPay == 1)
                {
                    order.Status = (int)SaleOrderStatusEnum.StockedAfterPay;
                }
                else if (request.SelectPay == 2)
                {
                    order.Status = (int)SaleOrderStatusEnum.StockedPay;
                }
                else
                {
                    throw new Exception("操作失败");
                }
            }
            else
            {
                throw new Exception("操作失败");
            }

            order.FinancePostDate = DateTime.Now;
            order.FinancePostName = userModel.UserNickName;
            order.FinanceRemark = "后收款";
            return Da.Update(order);
        }

        /// <summary>
        /// 后收款提交
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="request"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool PostPayReceive(UserModel userModel, SaleOrderReceiveModel request, out string msg)
        {
            var order = Da.Get<SaleOrder>(request.SaleOrderId);
            var b1 = CheckAuthOrder(userModel, order, out msg);
            if (!b1)
            {
                return false;
            }

            if (order.Status == (int)SaleOrderStatusEnum.Draft)
            {
                msg = "订单已退回修改，操作失败";
                return false;
            }

            if (order.Status != (int)SaleOrderStatusEnum.SubmitAfterPay &&
                order.Status != (int)SaleOrderStatusEnum.StockedAfterPay &&
                order.Status != (int)SaleOrderStatusEnum.ShipedAfterPay)
            {
                msg = "订单可能已提交，操作失败";
                return false;
            }

            if (order.Status == (int)SaleOrderStatusEnum.SubmitAfterPay)
            {
                order.Status = (int)SaleOrderStatusEnum.SubmitPay;
            }
            else if (order.Status == (int)SaleOrderStatusEnum.StockedAfterPay)
            {
                order.Status = (int)SaleOrderStatusEnum.StockedPay;
            }
            else if (order.Status == (int)SaleOrderStatusEnum.ShipedAfterPay)
            {
                order.Status = (int)SaleOrderStatusEnum.ShipedPay;
            }
            else
            {
                throw new Exception("操作失败");
            }

            order.FinancePostDate = DateTime.Now;
            order.FinancePostName = userModel.UserNickName;
            return Da.Update(order);
        }
    }
}
