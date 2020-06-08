using DwrUtility;
using DwrUtility.Strings;
using OmsClient.Common;
using OmsClient.DataAccess.Users;
using OmsClient.Entity;
using OmsClient.Entity.Enums;
using OmsClient.Entity.Results;
using System;
using System.Collections.Generic;
using System.Linq;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Biz.Bases
{
    public abstract class BaseBuyBiz
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <param name="status"></param>
        /// <param name="orderBy">排序</param>
        /// <returns></returns>
        protected static PagenationResult<BuyOrderListResult> GetListByUserStatus(UserModel userModel, int pageIndex, int pageSize, string keyword, List<int> status, Func<List<BuyOrder>, List<BuyOrder>> orderBy)
        {
            var list = new UserBuyOrderDa().GetListByBulk<BuyOrder>(new
            {
                Status = status
            });

            //排序
            list = orderBy.Invoke(list);

            if (!keyword.IsWhiteSpace())
            {
                var keys = keyword.ToKeys(StringComparer.OrdinalIgnoreCase).ToList();
                list = list.Where(p => $"{p.BuyOrderNumber} {p.Title} {p.DefaultShip} {p.DefaultAccount} {p.DefaultRemark} {p.SupplierCompany} {p.SupplierName} {p.SupplierTel} {p.SupplierMobilePhone} {p.SupplierEmail} {p.SupplierQQ} {p.SupplierWechat} {p.SupplierAddress} {p.StockInRemark}".HasSearchKeys(keys, StringComparison.OrdinalIgnoreCase, false)).ToList();
            }

            var result = new PagenationResult<BuyOrderListResult>()
            {
                count = list.Count,
                data = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToBuyOrderListResult()
            };
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
        /// <param name="orderBy">排序</param>
        /// <returns></returns>
        protected static PagenationResult<BuyOrderListResult> GetListByStatus(UserModel userModel, int pageIndex, int pageSize, string keyword, List<int> status, Func<List<BuyOrder>, List<BuyOrder>> orderBy)
        {
            var list = new UserBuyOrderDa().GetListByBulk<BuyOrder>(new
            {
                Status = status
            });

            //排序
            list = orderBy.Invoke(list);

            if (!keyword.IsWhiteSpace())
            {
                var keys = keyword.ToKeys(StringComparer.OrdinalIgnoreCase).ToList();
                list = list.Where(p => $"{p.BuyOrderNumber} {p.Title} {p.DefaultShip} {p.DefaultAccount} {p.DefaultRemark} {p.SupplierCompany} {p.SupplierName} {p.SupplierTel} {p.SupplierMobilePhone} {p.SupplierEmail} {p.SupplierQQ} {p.SupplierWechat} {p.SupplierAddress} {p.StockInRemark}".HasSearchKeys(keys, StringComparison.OrdinalIgnoreCase, false)).ToList();
            }

            var result = new PagenationResult<BuyOrderListResult>()
            {
                count = list.Count,
                data = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToBuyOrderListResult()
            };

            return result;
        }

        /// <summary>
        /// 验证权限：公司、订单用户、采购订单
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="order"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        protected static bool CheckAuthOrderByUserDraft(UserModel userModel, BuyOrder order, out string msg)
        {
            if (order == null)
            {
                msg = "订单不存在，操作失败";
                return false;
            }

            if (order.UserId != userModel.UserId)
            {
                msg = "不是你的订单，操作失败";
                return false;
            }

            if (order.Status != (int)BuyOrderStatusEnum.Draft && order.Status != (int)BuyOrderStatusEnum.SubmitBack)
            {
                msg = "订单状态已经改变，操作失败";
                return false;
            }

            msg = null;
            return true;
        }

        /// <summary>
        /// 验证权限：公司、订单用户
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="order"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        protected static bool CheckAuthOrderByUser(UserModel userModel, BuyOrder order, out string msg)
        {
            if (order == null)
            {
                msg = "订单不存在，操作失败";
                return false;
            }

            if (order.UserId != userModel.UserId)
            {
                msg = "不是你的订单，操作失败";
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
        /// <param name="account"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        protected static bool CheckAuthAccount(UserModel userModel, Account account, out string msg)
        {
            if (account == null)
            {
                msg = "账户不存在，操作失败";
                return false;
            }

            msg = null;
            return true;
        }
    }
}
