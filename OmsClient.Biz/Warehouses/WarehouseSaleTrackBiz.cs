using DwrUtility;
using DwrUtility.Strings;
using OmsClient.DataAccess.Warehouses;
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
namespace OmsClient.Biz.Warehouses
{
    public class WarehouseSaleTrackBiz : BaseOrderBiz
    {
        protected static readonly WarehouseSaleTrackDa Da = new WarehouseSaleTrackDa();

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="model"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public virtual bool AddSaleTrack(UserModel userModel, SaleTrack model, out string msg)
        {
            var order = Da.Get<SaleOrder>(model.SaleOrderId);
            var b1 = CheckAuthOrder(userModel, order, out msg);
            if (!b1)
            {
                return false;
            }

            var status = new List<int>()
            {
                (int)SaleOrderStatusEnum.StockedAfterPay,
                (int)SaleOrderStatusEnum.StockedPay,
                (int)SaleOrderStatusEnum.ShipedAfterPay,
                (int)SaleOrderStatusEnum.ShipedPay,
            };
            if (!status.Contains(order.Status))
            {
                msg = "状态已改变，操作失败";
                return false;
            }

            var currency = Da.Get<Currency>(model.CurrencyId);
            var b2 = CheckAuthCurrency(userModel, currency, out msg);
            if (!b2)
            {
                return false;
            }

            var m = new SaleTrack()
            {
                SaleTrackId = SeqGuid.NewGuid(),
                SaleOrderId = model.SaleOrderId,
                TrackName = model.TrackName,
                TrackNumber = model.TrackNumber,
                TrackRemark = model.TrackRemark,
                Feight = model.Feight,
                CurrencyId = currency.CurrencyId,
                CurrencyName = currency.CurrencyName,
                CurrencyText = currency.CurrencyText,
                CurrencySymbol = currency.CurrencySymbol,
                CurrencyRate = currency.CurrencyRate,
                CreateName = userModel.UserNickName,
                CreateDate = DateTime.Now,
            };

            Da.Add<Guid, SaleTrack>(m);
            return true;
        }

        /// <summary>
        /// 获取一条记录
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="saleTrackId"></param>
        /// <returns></returns>
        public virtual SaleTrack GetSaleTrackById(UserModel userModel, Guid saleTrackId)
        {
            return Da.Get<SaleTrack>(saleTrackId);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="model"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public virtual bool UpdateSaleTrack(UserModel userModel, SaleTrack model, out string msg)
        {
            var m = Da.Get<SaleTrack>(model.SaleTrackId);
            if (m == null)
            {
                msg = "记录不存在，操作失败";
                return false;
            }

            if (m.SaleOrderId != model.SaleOrderId)
            {
                msg = "记录不存在，操作失败";
                return false;
            }

            var order = Da.Get<SaleOrder>(m.SaleOrderId);
            var b1 = CheckAuthOrder(userModel, order, out msg);
            if (!b1)
            {
                return false;
            }

            var status = new List<int>()
            {
                (int)SaleOrderStatusEnum.StockedAfterPay,
                (int)SaleOrderStatusEnum.StockedPay,
                (int)SaleOrderStatusEnum.ShipedAfterPay,
                (int)SaleOrderStatusEnum.ShipedPay,
            };
            if (!status.Contains(order.Status))
            {
                msg = "状态已改变，操作失败";
                return false;
            }

            var currency = Da.Get<Currency>(model.CurrencyId);
            var b2 = CheckAuthCurrency(userModel, currency, out msg);
            if (!b2)
            {
                return false;
            }

            m.TrackName = model.TrackName;
            m.TrackNumber = model.TrackNumber;
            m.TrackRemark = model.TrackRemark;
            m.Feight = model.Feight;

            m.CurrencyId = currency.CurrencyId;
            m.CurrencyName = currency.CurrencyName;
            m.CurrencyText = currency.CurrencyText;
            m.CurrencySymbol = currency.CurrencySymbol;
            m.CurrencyRate = currency.CurrencyRate;

            m.CreateName = userModel.UserNickName;

            return Da.Update(m);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool DeleteSaleTrack(UserModel userModel, SaleTrack model)
        {
            var m = Da.Get<SaleTrack>(model.SaleTrackId);
            if (m == null)
            {
                return false;
            }

            return Da.Delete(model);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="saleOrderId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual PagenationResult<SaleTrack> GetSaleTrackList(UserModel userModel, Guid saleOrderId, int pageIndex, int pageSize, string keyword)
        {
            var list = GetSaleTrackList(userModel, saleOrderId);
            if (!keyword.IsWhiteSpace())
            {
                var keys = keyword.ToKeys(StringComparer.OrdinalIgnoreCase).ToList();
                list = list.Where(p => $"{p.TrackName} {p.TrackNumber} {p.TrackRemark} {p.CurrencyName} {p.CurrencyText} {p.CurrencySymbol} {p.CurrencyRate} {p.CreateName}".HasSearchKeys(keys, StringComparison.OrdinalIgnoreCase, false)).ToList();
            }

            var result = new PagenationResult<SaleTrack>()
            {
                count = list.Count,
                data = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList(),
            };
            return result;
        }

        /// <summary>
        /// 获取所有记录
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="saleOrderId"></param>
        /// <returns></returns>
        public virtual List<SaleTrack> GetSaleTrackList(UserModel userModel, Guid saleOrderId)
        {
            return Da.GetList<SaleTrack>(new
            {
                SaleOrderId = saleOrderId,
            }).OrderBy(p => p.CreateDate).ToList();
        }

        /// <summary>
        /// 物流列表和统计
        /// </summary>
        /// <returns></returns>
        public SaleTrackResult GetSaleTrackResult(UserModel userModel, Guid saleOrderId)
        {
            var list = GetSaleTrackList(userModel, saleOrderId);
            var total = list.Sum(p => p.Feight * p.CurrencyRate);

            var result = new SaleTrackResult()
            {
                Tracks = list,
                TotalRmb = total
            };

            return result;
        }
    }
}
