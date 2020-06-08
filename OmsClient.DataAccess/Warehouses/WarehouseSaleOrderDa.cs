using Dapper;
using ExecuteSqlBulk;
using OmsClient.Entity;
using System;
using System.Linq;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.DataAccess.Warehouses
{
    public class WarehouseSaleOrderDa : RepositoryBase
    {
        /// <summary>
        /// 出库发货
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public bool StockOut(SaleOrder order)
        {
            Db((db, tran) =>
            {
                var list = db.GetList<SaleOrderProduct>(new
                {
                    order.UserId,
                    order.SaleOrderId,
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
                        var qty = list.Where(p => p.ProductId == item.ProductId).Sum(p => p.Qty);
                        if (qty != 0)
                        {
                            item.HoldQty -= qty;
                            db.Update(item, tran);
                        }
                    }

                    //标志为已出库
                    foreach (var item in list)
                    {
                        item.IsStockOut = true;
                        db.Update(item, tran);
                    }

                    //写入出库记录
                    foreach (var item in list)
                    {
                        var m = new StockOut()
                        {
                            SaleOrderId = item.SaleOrderId,
                            UserId = order.UserId,
                            UserNickName = order.UserNickName,
                            SaleOrderProductId = item.SaleOrderProductId,
                            SaleOrderNumber = order.SaleOrderNumber,
                            ProductId = item.ProductId,
                            //转人民币
                            Price = item.Price * order.CurrencyRate,
                            Qty = item.Qty,
                            Remark = item.Remark,
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
    }
}
