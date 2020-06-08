using Dapper;
using DwrUtility;
using DwrUtility.Lists;
using ExecuteSqlBulk;
using OmsClient.Common;
using OmsClient.DataAccess.Users.Bases;
using OmsClient.Entity;
using OmsClient.Entity.Results;
using System;
using System.Collections.Generic;
using System.Linq;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.DataAccess.Users
{
    /// <summary>
    /// 
    /// </summary>
    public class UserSaleOrderDa : BaseUserSaleOrderDa
    {
        private const string Sql1 = "SELECT so.Status,so.UserId FROM dbo.SaleOrder so;";

        private const string Sql2 = "SELECT so.Status,so.UserId FROM dbo.SaleOrder so WHERE so.UserId=@UserId;";

        /// <summary>
        /// 获取销售单
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<OrderNumberResult> GetSaleOrderNumberList(Guid? userId = null)
        {
            var sqlStr = !userId.HasValue ? Sql1 : Sql2;

            var param = new
            {
                UserId = userId,
            };

            return Db(db => db.Query<OrderNumberResult>(sqlStr, param).ToList());
        }

        /// <summary>
        /// 获取订单号
        /// </summary>
        /// <returns></returns>
        public string GetSaleNumber(UserModel userModel)
        {
            var param = new
            {
                DateTime.Today
            };

            var list = Db(db => db.Query<SaleOrder>(@"SELECT * FROM dbo.SaleOrder so WHERE so.CreateDate>=@Today;", param).ToList());

            var orderNumbers = list.Select(p => p.SaleOrderNumber.ToInt()).ToList();
            var orderNumber = CommonMethods.GetOrderNumber(orderNumbers, DateTime.Today);

            return orderNumber;
        }

        /// <summary>
        /// 删除订单产品
        /// </summary>
        /// <param name="saleOrderProducts"></param>
        /// <returns></returns>
        public bool DeleteOrderProduct(List<SaleOrderProduct> saleOrderProducts)
        {
            Db((db, tran) =>
            {
                var products = db.GetListByBulk<Product>(new
                {
                    ProductId = saleOrderProducts.Select(p => p.ProductId).Distinct().ToList()
                }, tran).ToList();

                foreach (var product in products)
                {
                    var total = saleOrderProducts.Where(p => p.ProductId == product.ProductId).Sum(p => p.Qty);
                    product.HoldQty -= total;
                    product.SaleQty += total;

                    db.Update(product, tran);
                }

                foreach (var saleOrderProduct in saleOrderProducts)
                {
                    db.Delete<SaleOrderProduct>(saleOrderProduct.SaleOrderProductId, tran);
                }
            });
            return true;
        }

        /// <summary>
        /// 添加订单产品
        /// </summary>
        /// <returns></returns>
        public bool AddOrderProduct(SaleOrderProduct m)
        {
            Db((db, tran) =>
            {
                var product = db.Get<Product>(m.ProductId, tran);
                product.HoldQty += m.Qty;
                product.SaleQty -= m.Qty;

                //添加产品
                db.Update(product, tran);
                db.Insert<Guid, SaleOrderProduct>(m, tran);
            });
            return true;
        }

        /// <summary>
        /// 修改订单产品
        /// </summary>
        /// <returns></returns>
        public bool UpdateSaleOrderProduct(List<Product> products, List<SaleOrderProduct> orderProducts)
        {
            Db((db, tran) =>
            {
                foreach (var item in products)
                {
                    db.Update(item, tran);
                }
                foreach (var item in orderProducts)
                {
                    db.Update(item, tran);
                }
            });

            return true;
        }

        /// <summary>
        /// 导入产品
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool ImportProduct(List<SaleOrderProduct> list)
        {
            Db((db, tran) =>
            {
                var products = db.GetListByBulk<Product>(new
                {
                    ProductId = list.Select(p => p.ProductId).Distinct().ToList()
                }, tran).ToList();

                foreach (var product in products)
                {
                    var total = list.Where(p => p.ProductId == product.ProductId).Sum(p => p.Qty);
                    product.HoldQty += total;
                    product.SaleQty -= total;

                    db.Update(product, tran);
                }

                foreach (var item in list)
                {
                    db.Insert<Guid, SaleOrderProduct>(item, tran);
                }
            });

            return true;
        }
    }
}