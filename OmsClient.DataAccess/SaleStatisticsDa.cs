using Dapper;
using OmsClient.Entity;
using OmsClient.Entity.Enums;
using OmsClient.Entity.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.DataAccess
{
    public class SaleStatisticsDa : RepositoryBase
    {
        /// <summary>
        /// 获取业务员
        /// </summary>
        /// <returns></returns>
        public List<SaleStatisticsUser> GetUserList(UserModel userModel)
        {
            var sqlStr = "SELECT * FROM dbo.[User] u INNER JOIN (SELECT DISTINCT so.UserId FROM dbo.SaleOrder so) y ON u.UserId=y.UserId;";
            var param = new
            {
            };

            var list = Db(db => db.Query<User>(sqlStr, param).ToList());
            return list.Select(p => new SaleStatisticsUser()
            {
                UserId = p.UserId,
                UserName = $"{p.UserChnName}({p.UserNickName})",
            }).OrderBy(p => p.UserName).ToList();
        }

        /// <summary>
        /// 收款统计
        /// </summary>
        /// <returns></returns>
        public List<SaleStatisticsModel> GetSaleStatisticsResults(SaleStatisticsRequest request)
        {
            var param = new
            {
            };
            var paramUser = request.UserId.HasValue
                    ? (object)new
                    {
                        UserId = request.UserId.Value,
                    }
                    : param;
            //状态
            var status = new List<int>()
            {
                (int)SaleOrderStatusEnum.SubmitPay,
                (int)SaleOrderStatusEnum.StockedPay,
                (int)SaleOrderStatusEnum.ShipedPay,
            };

            //订单
            var orderAsynic = Task.Run(() =>
            {
                var list = GetList<SaleOrder>(paramUser);
                return list.Where(p => status.Contains(p.Status) && p.FinancePostDate >= request.BeginDatetime.Date && p.FinancePostDate < request.EndDatetime.Date.AddDays(1)).Select(p => new SaleStatisticsTemp()
                {
                    SaleOrderId = p.SaleOrderId,
                    CurrencyRate = p.CurrencyRate,
                    SaleOrderNumber = p.SaleOrderNumber,
                    UserId = p.UserId,
                    ShipFeight = p.ShipFeight,
                    ShipFee = p.ShipFee,
                    Discount = p.Discount,
                    DateTime = p.FinancePostDate,
                }).ToList();
            });
            //产品
            var productAsynic = Task.Run(() =>
            {
                var list = GetList<SaleOrderProduct>(paramUser);
                return list.GroupBy(p => new { p.SaleOrderId }, (p, q) => new SaleStatisticsCount()
                {
                    SaleOrderId = p.SaleOrderId,
                    Amount = q.Sum(g => g.Price * g.Qty),
                }).ToList();
            });
            //收款
            var receiveAsynic = Task.Run(() =>
            {
                var list = GetList<SaleReceive>(param);
                return list.GroupBy(p => new { p.SaleOrderId }, (p, q) => new SaleStatisticsCount()
                {
                    SaleOrderId = p.SaleOrderId,
                    Amount = q.Sum(g => g.CurrencyRate * g.Amount),
                    Fee = q.Sum(g => g.CurrencyRate * g.TranFee),
                }).ToList();
            });
            //运费
            var trackAsynic = Task.Run(() =>
            {
                var list = GetList<SaleTrack>(param);
                return list.GroupBy(p => new { p.SaleOrderId }, (p, q) => new SaleStatisticsCount()
                {
                    SaleOrderId = p.SaleOrderId,
                    Amount = q.Sum(g => g.CurrencyRate * g.Feight),
                }).ToList();
            });

            Task.WaitAll(orderAsynic, productAsynic, receiveAsynic, trackAsynic);
            var orders = orderAsynic.Result;
            if (orders.Count == 0)
            {
                return new List<SaleStatisticsModel>();
            }

            var products = productAsynic.Result;
            var receives = receiveAsynic.Result;
            var tracks = trackAsynic.Result;

            var userAsynic = GetListByBulkAsync<User>(new
            {
                UserId = orders.Select(p => p.UserId).ToList()
            });

            //产品
            orders.Join(products, p => p.SaleOrderId, p => p.SaleOrderId, (p, q) =>
                {
                    p.Product = q.Amount;
                    return p;
                })
                // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                .ToList();
            //收款
            orders.Join(receives, p => p.SaleOrderId, p => p.SaleOrderId, (p, q) =>
                {
                    p.RealFee = q.Fee;
                    p.RealRecive = q.Amount;
                    return p;
                })
                // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                .ToList();
            //运费
            orders.Join(tracks, p => p.SaleOrderId, p => p.SaleOrderId, (p, q) =>
                {
                    p.RealFeight = q.Amount;
                    return p;
                })
                // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                .ToList();

            Task.WaitAll(userAsynic);
            var users = userAsynic.Result;

            var result = new List<SaleStatisticsModel>();
            foreach (var item in orders)
            {
                var m = new SaleStatisticsModel()
                {
                    UserId = item.UserId,
                    SaleOrderId = item.SaleOrderId,
                    SaleOrderNumber = item.SaleOrderNumber,
                    UserName = users.Find(p => p.UserId == item.UserId)?.UserNickName,

                    ShipFeight = item.CurrencyRate * item.ShipFeight,
                    ShipFee = item.CurrencyRate * item.ShipFee,
                    Discount = item.CurrencyRate * item.Discount,
                    DateTime = item.DateTime,
                    Product = item.CurrencyRate * item.Product,

                    RealRecive = item.RealRecive,
                    RealFee = item.RealFee,
                    RealFeight = item.RealFeight,
                };

                //预估费用
                m.Amount = m.Product + m.ShipFeight + m.ShipFee - m.Discount;
                //实际到账
                m.RealTotal = m.RealRecive - m.RealFee - m.RealFeight;

                result.Add(m);
            }

            return result.OrderBy(p => p.DateTime).ToList();
        }
    }
}
