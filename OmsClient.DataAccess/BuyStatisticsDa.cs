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
    public class BuyStatisticsDa : RepositoryBase
    {
        /// <summary>
        /// 获取业务员
        /// </summary>
        /// <returns></returns>
        public List<BuyStatisticsUser> GetUserList(UserModel userModel)
        {
            var sqlStr = "SELECT * FROM dbo.[User] u INNER JOIN (SELECT DISTINCT so.UserId FROM dbo.BuyOrder so) y ON u.UserId=y.UserId;";
            var param = new
            {
            };

            var list = Db(db => db.Query<User>(sqlStr, param).ToList());
            return list.Select(p => new BuyStatisticsUser()
            {
                UserId = p.UserId,
                UserName = $"{p.UserChnName}({p.UserNickName})",
            }).OrderBy(p => p.UserName).ToList();
        }

        /// <summary>
        /// 收款统计
        /// </summary>
        /// <returns></returns>
        public List<BuyStatisticsModel> GetBuyStatisticsResults(BuyStatisticsRequest request)
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

            //订单
            var orderAsynic = Task.Run(() =>
            {
                var list = GetList<BuyOrder>(paramUser);
                return list.Where(p => p.Status == (int)BuyOrderStatusEnum.StockInPay && p.PayFinishDate >= request.BeginDatetime.Date && p.PayFinishDate < request.EndDatetime.Date.AddDays(1)).Select(p => new BuyStatisticsTemp()
                {
                    BuyOrderId = p.BuyOrderId,
                    CurrencyRate = p.CurrencyRate,
                    BuyOrderNumber = p.BuyOrderNumber,
                    UserId = p.UserId,
                    ShipFeight = p.ShipFeight,
                    ShipFee = p.ShipFee,
                    Discount = p.Discount,
                    DateTime = p.PayFinishDate,
                }).ToList();
            });
            //产品
            var productAsynic = Task.Run(() =>
            {
                var list = GetList<BuyOrderProduct>(paramUser);
                return list.GroupBy(p => new { p.BuyOrderId }, (p, q) => new BuyStatisticsCount()
                {
                    BuyOrderId = p.BuyOrderId,
                    Amount = q.Sum(g => g.Price * g.Qty),
                }).ToList();
            });
            //付款
            var payAsynic = Task.Run(() =>
            {
                var list = GetList<BuyPay>(param);
                return list.GroupBy(p => new { p.BuyOrderId }, (p, q) => new BuyStatisticsCount()
                {
                    BuyOrderId = p.BuyOrderId,
                    Amount = q.Sum(g => g.CurrencyRate * g.Amount),
                }).ToList();
            });

            Task.WaitAll(orderAsynic, productAsynic, payAsynic);
            var orders = orderAsynic.Result;
            if (orders.Count == 0)
            {
                return new List<BuyStatisticsModel>();
            }

            var products = productAsynic.Result;
            var pays = payAsynic.Result;

            var userAsynic = GetListByBulkAsync<User>(new
            {
                UserId = orders.Select(p => p.UserId).ToList()
            });

            //产品
            orders.Join(products, p => p.BuyOrderId, p => p.BuyOrderId, (p, q) =>
                {
                    p.Product = q.Amount;
                    return p;
                })
                // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                .ToList();
            //付款
            orders.Join(pays, p => p.BuyOrderId, p => p.BuyOrderId, (p, q) =>
                {
                    p.PayTotal = q.Amount;
                    return p;
                })
                // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                .ToList();

            Task.WaitAll(userAsynic);
            var users = userAsynic.Result;

            var result = new List<BuyStatisticsModel>();
            foreach (var item in orders)
            {
                var m = new BuyStatisticsModel()
                {
                    UserId = item.UserId,
                    BuyOrderId = item.BuyOrderId,
                    BuyOrderNumber = item.BuyOrderNumber,
                    UserName = users.Find(p => p.UserId == item.UserId)?.UserNickName,

                    ShipFeight = item.CurrencyRate * item.ShipFeight,
                    ShipFee = item.CurrencyRate * item.ShipFee,
                    Discount = item.CurrencyRate * item.Discount,
                    DateTime = item.DateTime,
                    Product = item.CurrencyRate * item.Product,

                    PayTotal = item.PayTotal,
                };

                //预估费用
                m.Amount = m.Product + m.ShipFeight + m.ShipFee - m.Discount;

                result.Add(m);
            }

            return result.OrderBy(p => p.DateTime).ToList();
        }
    }
}
