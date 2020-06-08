using Dapper;
using ExecuteSqlBulk;
using OmsClient.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.DataAccess.Warehouses
{
    public class WarehouseBuyOrderDa : RepositoryBase
    {
        /// <summary>
        /// 采购入库
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public bool StockIn(BuyOrder order)
        {
            Db((db, tran) =>
            {
                var list = db.GetList<BuyOrderProduct>(new
                {
                    order.UserId,
                    order.BuyOrderId,
                }, tran).ToList();

                if (list.Count > 0)
                {
                    //更新产品挂起数量
                    var products = db.GetListByBulk<Product>(new
                    {
                        ProductId = list.Select(p => p.ProductId).Distinct().ToList()
                    }, tran).ToList();

                    foreach (var item in products)
                    {
                        var rs = list.Where(p => p.ProductId == item.ProductId).ToList();
                        var qty = rs.Sum(p => p.Qty);
                        var inQty = rs.Sum(p => p.InQty);
                        if (qty != 0 || inQty != 0)
                        {
                            item.InTransitQty -= qty;
                            item.SaleQty += inQty;
                            db.Update(item, tran);
                        }
                    }

                    //标志为已出库
                    foreach (var item in list)
                    {
                        item.IsStockIn = true;
                        db.Update(item, tran);
                    }

                    //写入出库记录
                    foreach (var item in list)
                    {
                        var m = new StockIn()
                        {
                            BuyOrderId = item.BuyOrderId,
                            UserId = order.UserId,
                            UserNickName = order.UserNickName,
                            BuyOrderProductId = item.BuyOrderProductId,
                            BuyOrderNumber = order.BuyOrderNumber,
                            ProductId = item.ProductId,
                            //转人民币
                            Price = item.Price * order.CurrencyRate,
                            InQty = item.InQty,
                            Remark = item.Remark,
                            InStockRemark = item.InStockRemark,
                            CreateDate = DateTime.Now,
                        };
                        db.Insert(m, tran);
                    }
                }

                //更新订单
                db.Update(order, tran);
            });
            return true;
        }

        /// <summary>
        /// 更新产品
        /// </summary>
        /// <param name="updateOrderProducts"></param>
        /// <returns></returns>
        public bool UpdateBuyOrderProduct(List<BuyOrderProduct> updateOrderProducts)
        {
            Db((db, tran) =>
            {
                foreach (var item in updateOrderProducts)
                {
                    db.Update(item, tran);
                }
            });

            return true;
        }

        /// <summary>
        /// 删除产品
        /// </summary>
        /// <param name="deleteProducts"></param>
        /// <returns></returns>
        public bool DeleteOrderProduct(List<BuyOrderProduct> deleteProducts)
        {
            Db((db, tran) =>
            {
                foreach (var item in deleteProducts)
                {
                    db.Delete(item, tran);
                }
            });

            return true;
        }
    }
}
