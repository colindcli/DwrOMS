using DwrUtility.Lists;
using OmsClient.Common;
using OmsClient.DataAccess;
using OmsClient.Entity;
using OmsClient.Entity.Results;
using System;
using System.Collections.Generic;
using System.Linq;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Biz
{
    public class SaleStatisticsBiz
    {
        private static readonly SaleStatisticsDa Da = new SaleStatisticsDa();

        /// <summary>
        /// 获取业务员
        /// </summary>
        /// <returns></returns>
        public SaleStatisticsInit GetUserList(UserModel userModel, bool getUser)
        {
            var result = new SaleStatisticsInit()
            {
                Dates = new List<string>()
                {
                    DateTime.Today.AddMonths(-1).ToString("yyyy-MM-01"),
                    Convert.ToDateTime(DateTime.Today.ToString("yyyy-MM-01")).AddDays(-1).ToString("yyyy-MM-dd"),
                    DateTime.Today.ToString("yyyy-MM-01"),
                    Convert.ToDateTime(DateTime.Today.ToString("yyyy-MM-01")).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd"),
                }
            };

            if (getUser)
            {
                result.Row = Da.GetUserList(userModel);
            }

            return result;
        }

        /// <summary>
        /// 收款统计
        /// </summary>
        /// <returns></returns>
        public SaleStatisticsResult GetSaleStatisticsResults(SaleStatisticsRequest request)
        {
            var list = Da.GetSaleStatisticsResults(request);

            var total = new SaleStatisticsModel();
            var result = new SaleStatisticsResult()
            {
                Rows = list,
                Total = total,
            };

            if (list.Count == 0)
            {
                return result;
            }

            total.ShipFeight = list.Sum(p => p.ShipFeight);
            total.ShipFee = list.Sum(p => p.ShipFee);
            total.Discount = list.Sum(p => p.Discount);
            total.Amount = list.Sum(p => p.Amount);
            total.Product = list.Sum(p => p.Product);
            total.RealRecive = list.Sum(p => p.RealRecive);
            total.RealFee = list.Sum(p => p.RealFee);
            total.RealFeight = list.Sum(p => p.RealFeight);
            total.RealTotal = list.Sum(p => p.RealTotal);

            return result;
        }

        /// <summary>
        /// 收款图表统计
        /// </summary>
        /// <returns></returns>
        public SaleChartResult GetSaleStatisticsChart(SaleChartRequest request)
        {
            if (!new[] { 1, 2, 3 }.Contains(request.Type))
            {
                return null;
            }

            var title = string.Empty;
            switch (request.Type)
            {
                case 1:
                    {
                        title = "按月统计";
                        break;
                    }
                case 2:
                    {
                        title = "按周统计";
                        break;
                    }
                case 3:
                    {
                        title = "按天统计";
                        break;
                    }
            }

            var date = CommonMethods.GetDateType(request.Type);

            var list = Da.GetSaleStatisticsResults(new SaleStatisticsRequest()
            {
                UserId = request.UserId,
                BeginDatetime = date.First().Begin,
                EndDatetime = date.Last().End,
            });

            //按结算时间标记序号
            foreach (var item in list)
            {
                foreach (var d in date)
                {
                    var b = item.DateTime.HasValue && item.DateTime.Value >= d.Begin &&
                              item.DateTime.Value < d.EndNextDay;
                    if (!b)
                    {
                        continue;
                    }

                    item.I = d.Index;
                    break;
                }
            }

            //按人和序号汇总
            var items = list.GroupBy(p => new { p.UserId, p.I }, (p, q) => new { p.UserId, Index = p.I, Total = q.Sum(g => g.RealTotal) }).OrderBy(p => p.UserId).ThenBy(p => p.Index).ToList();

            var users = list.ToDict(p => p.UserId, p => p.UserName, true);
            var rows = new List<SaleSeriesItem>();
            foreach (var item in users)
            {
                var totals = date.Select(d => items.Find(p => p.UserId == item.Key && p.Index == d.Index)?.Total ?? 0).ToList();
                var m = new SaleSeriesItem()
                {
                    name = item.Value,
                    type = "line",
                    stack = "销售额",
                    data = totals,
                };
                rows.Add(m);
            }

            var result = new SaleChartResult()
            {
                LegendData = users.Select(p => p.Value).ToList(),
                XaxisData = date.Select(p => p.Name).ToList(),
                SeriesItems = rows,
                Title = title,
            };

            return result;
        }
    }
}
