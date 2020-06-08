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
    public abstract class BaseUserStockOutBiz
    {
        protected static readonly UserStockOutDa Da = new UserStockOutDa();

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual PagenationResult<StockOutResult> GetStockOutList(UserModel userModel, int pageIndex, int pageSize,
            string keyword)
        {
            var list = Da.GetList<StockOut>().OrderByDescending(p => p.StockOutId).ToList();

            //没有搜索关键词
            if (keyword.IsWhiteSpace())
            {
                return GetStockOutList(userModel, list, pageIndex, pageSize);
            }

            //搜索关键词
            var products = Da.GetList<Product>();

            var keys = keyword.ToKeys(StringComparer.OrdinalIgnoreCase).ToList();
            var rows = list.Join(products, p => p.ProductId, p => p.ProductId, (p, q) => new { p, q })
                .Select(m => new StockOutResult()
                {
                    ProductId = m.p.ProductId,
                    Number = m.p.SaleOrderNumber,
                    UserName = m.p.UserNickName,
                    Sku = m.q.ProductSku,
                    Name = m.q.ProductName,
                    Specification = m.q.ProductSpecification,
                    Description = m.q.ProductRemark,
                    ImageQty = m.q.ImageQty,
                    Price = m.p.Price,
                    Qty = m.p.Qty,
                    Remark = m.p.Remark,
                    CreateDate = m.p.CreateDate,
                }).Where(p => $"{p.Number} {p.Sku} {p.UserName} {p.Name} {p.Specification} {p.Description} {p.Remark}".HasSearchKeys(keys, StringComparison.OrdinalIgnoreCase, false)).ToList();

            var result = new PagenationResult<StockOutResult>()
            {
                count = list.Count,
                PageIndex = pageIndex,
                PageSize = pageSize,
                data = rows
            };

            return result;
        }

        /// <summary>
        /// 没有搜索
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="list"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        private PagenationResult<StockOutResult> GetStockOutList(UserModel userModel, List<StockOut> list, int pageIndex, int pageSize)
        {
            var result = new PagenationResult<StockOutResult>()
            {
                count = list.Count,
                PageIndex = pageIndex,
                PageSize = pageSize,
            };

            var rows = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            var productIds = rows.Select(p => p.ProductId).Distinct().ToList();
            var products = Da.GetListByBulk<Product>(new
            {
                ProductId = productIds,
            }).ToList();

            var items = new List<StockOutResult>();
            foreach (var item in rows)
            {
                var m = new StockOutResult()
                {
                    Number = item.SaleOrderNumber,
                    UserName = item.UserNickName,
                    Price = item.Price,
                    Qty = item.Qty,
                    Remark = item.Remark,
                    CreateDate = item.CreateDate,
                };
                var obj = products.Find(p => p.ProductId == item.ProductId);
                if (obj != null)
                {
                    m.ProductId = obj.ProductId;
                    m.Sku = obj.ProductSku;
                    m.Name = obj.ProductName;
                    m.Specification = obj.ProductSpecification;
                    m.Description = obj.ProductRemark;
                    m.ImageQty = obj.ImageQty;
                }

                items.Add(m);
            }

            result.data = items;

            return result;
        }
    }
}
