using Dapper;
using OmsClient.DataAccess.Users.Bases;
using OmsClient.Entity;
using OmsClient.Entity.Enums;
using OmsClient.Entity.Results;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    public class UserProductDa : BaseUserProductDa
    {
        /// <summary>
        /// 更新产品数量
        /// </summary>
        /// <param name="db"></param>
        /// <param name="m"></param>
        private static void UpdateProductImageQty(SqlConnection db, ProductImage m)
        {
            db.Execute(
                @"UPDATE dbo.Product SET ImageQty=(SELECT COUNT(1) FROM dbo.ProductImage pi WHERE pi.ProductId=@ProductId) WHERE ProductId=@ProductId;",
                new
                {
                    m.ProductId
                });
        }

        /// <summary>
        /// 添加产品图片
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public bool AddProductImage(ProductImage m)
        {
            Db(db =>
            {
                db.Insert<Guid, ProductImage>(m);
                //更新产品数量
                UpdateProductImageQty(db, m);
            });

            return true;
        }

        /// <summary>
        /// 删除产品图片
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public bool DeleteProductImage(ProductImage m)
        {
            Db(db =>
            {
                db.Delete(m);
                //更新产品数量
                UpdateProductImageQty(db, m);
            });

            return true;
        }

        /// <summary>
        /// 导入产品
        /// </summary>
        /// <param name="products"></param>
        /// <returns></returns>
        public bool ImportProduct(List<Product> products)
        {
            Db((db, tran) =>
            {
                foreach (var item in products)
                {
                    db.Insert<Guid, Product>(item, tran);
                }
            });

            return true;
        }
    }
}