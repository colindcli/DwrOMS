using DwrUtility;
using DwrUtility.Strings;
using OmsClient.DataAccess.Users;
using OmsClient.Entity;
using OmsClient.Entity.Results;
using System;
using System.Collections.Generic;
using System.Linq;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Biz.Users.Bases
{
    public abstract class BaseUserSaleOrderBiz
    {
        protected static readonly UserSaleOrderDa Da = new UserSaleOrderDa();

        /// <summary>
        /// 获取一条记录
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="saleOrderId"></param>
        /// <returns></returns>
        public virtual SaleOrder GetSaleOrderById(UserModel userModel, Guid saleOrderId)
        {
            var m = Da.Get<SaleOrder>(saleOrderId);
            if (m == null || m.UserId != userModel.UserId)
            {
                return null;
            }
            return m;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool UpdateSaleOrder(UserModel userModel, SaleOrder model)
        {
            var m = Da.Get<SaleOrder>(model.SaleOrderId);
            if (m == null || m.UserId != userModel.UserId)
            {
                return false;
            }

            m.SaleOrderNumber = model.SaleOrderNumber;
            m.CurrencyId = model.CurrencyId;
            m.CurrencyName = model.CurrencyName;
            m.CurrencyText = model.CurrencyText;
            m.CurrencySymbol = model.CurrencySymbol;
            m.CurrencyRate = model.CurrencyRate;
            m.Title = model.Title;
            m.ShipDate = model.ShipDate;
            m.ShipFeight = model.ShipFeight;
            m.ShipFee = model.ShipFee;
            m.Discount = model.Discount;
            m.ToConsignee = model.ToConsignee;
            m.ToCompanyName = model.ToCompanyName;
            m.ToTelphone = model.ToTelphone;
            m.ToAddress = model.ToAddress;
            m.ToRemark = model.ToRemark;
            m.Status = model.Status;
            m.PostDate = model.PostDate;
            m.StockStart = model.StockStart;
            m.StockStartName = model.StockStartName;
            m.StockEnd = model.StockEnd;
            m.StockEndName = model.StockEndName;
            m.StockRemark = model.StockRemark;

            return Da.Update(m);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual PagenationResult<SaleOrder> GetSaleOrderList(UserModel userModel, int pageIndex, int pageSize, string keyword)
        {
            var list = GetSaleOrderList(userModel);
            if (!keyword.IsWhiteSpace())
            {
                var keys = keyword.ToKeys(StringComparer.OrdinalIgnoreCase).ToList();
                list = list.Where(p => $"{p.SaleOrderNumber} {p.CurrencyName} {p.CurrencyText} {p.CurrencySymbol} {p.Title} {p.ToConsignee} {p.ToCompanyName} {p.ToTelphone} {p.ToAddress} {p.ToRemark} {p.StockStartName} {p.StockEndName} {p.StockRemark}".HasSearchKeys(keys, StringComparison.OrdinalIgnoreCase, false)).ToList();
            }
            var result = new PagenationResult<SaleOrder>()
            {
                count = list.Count,
                data = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList()
            };
            return result;
        }

        /// <summary>
        /// 获取所有记录
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        public virtual List<SaleOrder> GetSaleOrderList(UserModel userModel)
        {
            return Da.GetList<SaleOrder>(new
            {
                userModel.UserId,
            });
        }
    }
}
