using DwrUtility;
using DwrUtility.Strings;
using DwrUtility.Trees;
using OmsClient.DataAccess;
using OmsClient.Entity;
using OmsClient.Entity.Results;
using System;
using System.Linq;
using System.Threading.Tasks;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Biz
{
    public class BuyProductStatisticsBiz
    {
        private static readonly BuyStatisticsDa Da = new BuyStatisticsDa();

        /// <summary>
        /// 产品统计
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="categoryId"></param>
        /// <param name="keyword"></param>
        public PagenationResult<StockCountResult> GetBuyProductStatistics(UserModel userModel, DateTime beginDate, DateTime endDate, Guid? userId, int pageIndex, int pageSize, int? categoryId, string keyword)
        {
            //没搜索条件
            if (!categoryId.HasValue && keyword.IsWhiteSpace())
            {
                return GetBuyProductStatistics(userModel, beginDate, endDate, userId, pageIndex, pageSize);
            }

            //有搜索条件
            return GetBuyProductStatisticsKey(userModel, beginDate, endDate, userId, pageIndex, pageSize, categoryId, keyword);
        }

        /// <summary>
        /// 产品统计 (带搜索条件)
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="categoryId"></param>
        /// <param name="keyword"></param>
        public PagenationResult<StockCountResult> GetBuyProductStatisticsKey(UserModel userModel, DateTime beginDate, DateTime endDate, Guid? userId, int pageIndex, int pageSize, int? categoryId, string keyword)
        {
            var param = new
            {
            };
            var paramUser = userId.HasValue
                ? (object)new
                {
                    UserId = userId.Value,
                }
                : param;

            var categoryAsync = Da.GetListAsync<Category>(param);
            var productAsync = Da.GetListAsync<Product>(param);
            var outAsync = Task.Run(() =>
            {
                var end = endDate.Date.AddDays(1);
                var list = Da.GetList<StockIn>(paramUser).Where(p => p.CreateDate >= beginDate && p.CreateDate < end).GroupBy(p => new { p.ProductId },
                    (p, q) =>
                    {
                        var rs = q.ToList();
                        var m = new StockCountResult
                        {
                            ProductId = p.ProductId,
                            Qty = rs.Sum(g => g.InQty),
                            Amount = rs.Sum(g => g.InQty * g.Price),
                        };

                        //只算价格大于0的
                        var qty = rs.Where(g => g.Price > 0).Sum(g => g.InQty);
                        if (qty > 0)
                        {
                            m.Avg = Math.Round(m.Amount / qty, 2);
                        }

                        return m;
                    }).OrderByDescending(p => p.Qty).ToList();

                return list;
            });

            Task.WaitAll(categoryAsync, productAsync, outAsync);
            var categorys = categoryAsync.Result;
            var products = productAsync.Result;
            var items = outAsync.Result;

            if (categoryId.HasValue && categoryId.Value > 0)
            {
                var cids = categorys.GetChildNodes(categoryId.Value, p => p.CategoryId, p => p.CategoryParentId, true);

                //排除产品
                products = products.Join(cids, p => p.CategoryId, p => p.CategoryId, (p, q) => p).ToList();
            }

            var rows = items.Join(products, p => p.ProductId, p => p.ProductId, (p, q) =>
                {
                    var m = MapperHelper.Mapper<Product, StockCountResult>(q);
                    m.Qty = p.Qty;
                    m.Amount = p.Amount;
                    m.Avg = p.Avg;

                    var cates = categorys.GetParentNodes(m.CategoryId, g => g.CategoryId, g => g.CategoryParentId, true);
                    m.CategoryName = string.Join(">", cates.Select(g => g.CategoryName));
                    return m;
                })
                // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                .ToList();

            if (!keyword.IsWhiteSpace())
            {
                var keys = keyword.ToKeys(StringComparer.OrdinalIgnoreCase).ToList();
                rows = rows.Where(p => $"{p.ProductSku} {p.ProductName} {p.ProductSpecification} {p.ProductRemark} {p.CreateName} {p.AreaPosition}".HasSearchKeys(keys, StringComparison.OrdinalIgnoreCase, false)).ToList();
            }

            var result = new PagenationResult<StockCountResult>()
            {
                count = rows.Count,
                data = rows.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList(),
                PageIndex = pageIndex,
                PageSize = pageSize,
            };

            return result;
        }

        /// <summary>
        /// 产品统计 (没有搜索条件)
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        public PagenationResult<StockCountResult> GetBuyProductStatistics(UserModel userModel, DateTime beginDate, DateTime endDate, Guid? userId, int pageIndex, int pageSize)
        {
            var param = new
            {
            };
            var paramUser = userId.HasValue
                ? (object)new
                {
                    UserId = userId.Value,
                }
                : param;

            var categoryAsync = Da.GetListAsync<Category>(param);
            var productAsync = Da.GetListAsync<Product>(param);
            var outAsync = Task.Run(() =>
            {
                var end = endDate.Date.AddDays(1);
                var list = Da.GetList<StockIn>(paramUser).Where(p => p.CreateDate >= beginDate && p.CreateDate < end).GroupBy(p => new { p.ProductId },
                    (p, q) =>
                    {
                        var rs = q.ToList();
                        var m = new StockCountResult
                        {
                            ProductId = p.ProductId,
                            Qty = rs.Sum(g => g.InQty),
                            Amount = rs.Sum(g => g.InQty * g.Price),
                        };

                        //只算价格大于0的
                        var qty = rs.Where(g => g.Price > 0).Sum(g => g.InQty);
                        if (qty > 0)
                        {
                            m.Avg = Math.Round(m.Amount / qty, 2);
                        }

                        return m;
                    }).OrderByDescending(p => p.Qty).ToList();

                var rows = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                return new PagenationResult<StockCountResult>()
                {
                    count = list.Count,
                    data = rows,
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                };
            });

            Task.WaitAll(categoryAsync, productAsync, outAsync);
            var categorys = categoryAsync.Result;
            var products = productAsync.Result;
            var result = outAsync.Result;

            result.data = result.data.Join(products, p => p.ProductId, p => p.ProductId, (p, q) =>
                {
                    var m = MapperHelper.Mapper<Product, StockCountResult>(q);
                    m.Qty = p.Qty;
                    m.Amount = p.Amount;
                    m.Avg = p.Avg;

                    var cates = categorys.GetParentNodes(m.CategoryId, g => g.CategoryId, g => g.CategoryParentId, true);
                    m.CategoryName = string.Join(">", cates.Select(g => g.CategoryName));
                    return m;
                })
                // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                .ToList();

            return result;
        }
    }
}
