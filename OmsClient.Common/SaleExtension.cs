using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DwrUtility;
using DwrUtility.Maths;
using DwrUtility.Trees;
using OmsClient.Entity;
using OmsClient.Entity.Results;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Common
{
    public static class SaleExtension
    {
        /// <summary>
        /// 添加与订单相同的价格
        /// </summary>
        /// <param name="list"></param>
        /// <param name="order"></param>
        /// <param name="products"></param>
        /// <returns></returns>
        public static List<SaleOrderProductSelect> ToSaleOrderProductSelect(this List<Product> list, SaleOrder order, List<SaleOrderProduct> products)
        {
            var rate = order.CurrencyRate;
            var rows = list.Select(p =>
            {
                var m = MapperHelper.Mapper<Product, SaleOrderProductSelect>(p);
                m.Symbol = order.CurrencySymbol;
                if (order.CurrencyRate == 1)
                {
                    m.PriceA = m.Price1;
                    m.PriceB = m.Price10;
                    m.PriceC = m.Price100;
                }
                else
                {
                    m.PriceA = (m.Price1 / rate).ToRoundUp(2);
                    m.PriceB = (m.Price10 / rate).ToRoundUp(2);
                    m.PriceC = (m.Price100 / rate).ToRoundUp(2);
                }

                //已添加到订单
                m.IsAdded = products.Exists(g => g.ProductId == m.ProductId);

                return m;
            }).ToList();

            return rows;
        }

        /// <summary>
        /// 计算
        /// </summary>
        /// <param name="list"></param>
        /// <param name="order"></param>
        /// <param name="products"></param>
        /// <param name="categories"></param>
        /// <returns></returns>
        public static List<SaleOrderProductList> ToSaleOrderProduct(this List<SaleOrderProduct> list, SaleOrder order, List<Product> products, List<Category> categories)
        {
            var rows = new List<SaleOrderProductList>();
            foreach (var item in list)
            {
                var m = MapperHelper.Mapper<SaleOrderProduct, SaleOrderProductList>(item);
                rows.Add(m);

                var product = products.Find(p => p.ProductId == item.ProductId);
                if (product == null)
                {
                    continue;
                }

                m.Symbol = order.CurrencySymbol;

                //转换成订单的币种
                m.PriceA = (product.Price1 / order.CurrencyRate).ToRoundUp(2);
                m.PriceB = (product.Price10 / order.CurrencyRate).ToRoundUp(2);
                m.PriceC = (product.Price100 / order.CurrencyRate).ToRoundUp(2);

                //categoryName
                if (categories.Count > 0)
                {
                    var cates = categories.GetParentNodes(product.CategoryId, p => p.CategoryId, p => p.CategoryParentId, true);
                    if (cates != null)
                    {
                        m.CategoryName = string.Join("|", cates.Select(p => p.CategoryName));
                    }
                }

                //product
                m.ProductSku = product.ProductSku;
                m.ProductName = product.ProductName;
                m.ProductSpecification = product.ProductSpecification;
                m.ProductRemark = product.ProductRemark;
                m.ProductWeight = product.ProductWeight;
                m.InTransitQty = product.InTransitQty;
                m.SaleQty = product.SaleQty;
                m.HoldQty = product.HoldQty;
                m.OrderQty = product.OrderQty;
                m.Price1 = product.Price1;
                m.Price10 = product.Price10;
                m.Price100 = product.Price100;
                m.ImageQty = product.ImageQty;
                m.AreaPosition = product.AreaPosition;
            }

            return rows;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="saleOrders"></param>
        /// <returns></returns>
        public static List<SaleOrderListResult> ToSaleOrderListResult(this IEnumerable<SaleOrder> saleOrders)
        {
            return saleOrders.Select(p =>
            {
                var m = MapperHelper.Mapper<SaleOrder, SaleOrderListResult>(p);

                return m;
            }).ToList();
        }


    }
}
