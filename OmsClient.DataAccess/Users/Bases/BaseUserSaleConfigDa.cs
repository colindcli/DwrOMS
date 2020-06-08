using Dapper;
using OmsClient.Entity;
using System;
using System.Linq;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.DataAccess.Users.Bases
{
    public abstract class BaseUserSaleConfigDa : RepositoryBase
    {
        /// <summary>
        /// 获取记录
        /// </summary>
        /// <returns></returns>
        public virtual SaleConfig GetSaleConfig(UserModel userModel)
        {
            return Db(db =>
            {
                var list = db.GetList<SaleConfig>(new
                {
                    userModel.UserId,
                }).ToList();

                //1条
                if (list.Count == 1)
                {
                    return list.First();
                }

                //0条
                if (list.Count == 0)
                {
                    var model = new SaleConfig()
                    {
                        UserId = userModel.UserId,
                    };
                    db.Insert<Guid, SaleConfig>(model);
                    return model;
                }

                //多条
                var m = list.First();
                var rows = list.Where(p => p != m);
                foreach (var row in rows)
                {
                    db.Delete(row);
                }
                return m;
            });
        }
    }
}