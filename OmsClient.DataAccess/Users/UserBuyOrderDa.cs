using Dapper;
using ExecuteSqlBulk;
using OmsClient.Common;
using OmsClient.DataAccess.Users.Bases;
using OmsClient.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using OmsClient.Entity.Results;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.DataAccess.Users
{
    /// <summary>
    /// 
    /// </summary>
    public partial class UserBuyOrderDa : BaseUserBuyOrderDa
    {
        private const string Sql1 = "SELECT bo.Status,bo.UserId FROM dbo.BuyOrder bo;";

        private const string Sql2 = "SELECT bo.Status,bo.UserId FROM dbo.BuyOrder bo WHERE bo.UserId=@UserId;";

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
        public string GetBuyNumber(UserModel userModel)
        {
            var param = new
            {
                DateTime.Today
            };

            var list = Db(db => db.Query<BuyOrder>(@"SELECT * FROM dbo.BuyOrder so WHERE so.CreateDate>=@Today;", param).ToList());

            var orderNumbers = list.Select(p => p.BuyOrderNumber).ToList();
            var orderNumber = CommonMethods.GetBuyOrderNumber(orderNumbers, DateTime.Today);

            return orderNumber;
        }

        /// <summary>
        /// 删除订单产品
        /// </summary>
        /// <param name="buyOrderProducts"></param>
        /// <returns></returns>
        public bool DeleteOrderProduct(List<BuyOrderProduct> buyOrderProducts)
        {
            Db((db, tran) =>
            {
                var products = db.GetListByBulk<Product>(new
                {
                    ProductId = buyOrderProducts.Select(p => p.ProductId).Distinct().ToList()
                }, tran).ToList();

                foreach (var product in products)
                {
                    var total = buyOrderProducts.Where(p => p.ProductId == product.ProductId).Sum(p => p.Qty);
                    product.InTransitQty -= total;

                    db.Update(product, tran);
                }

                foreach (var buyOrderProduct in buyOrderProducts)
                {
                    db.Delete<BuyOrderProduct>(buyOrderProduct.BuyOrderProductId, tran);
                }
            });
            return true;
        }

        /// <summary>
        /// 添加订单产品
        /// </summary>
        /// <returns></returns>
        public bool AddOrderProduct(BuyOrderProduct m)
        {
            Db((db, tran) =>
            {
                var product = db.Get<Product>(m.ProductId, tran);
                product.InTransitQty += m.Qty;

                //添加产品
                db.Update(product, tran);
                db.Insert<Guid, BuyOrderProduct>(m, tran);
            });
            return true;
        }

        /// <summary>
        /// 修改订单产品
        /// </summary>
        /// <returns></returns>
        public bool UpdateBuyOrderProduct(List<Product> products, List<BuyOrderProduct> orderProducts)
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
    }
}