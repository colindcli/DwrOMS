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
    public abstract class BaseUserSaleOrderProductBiz
    {
        protected static readonly UserSaleOrderProductDa Da = new UserSaleOrderProductDa();

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool AddSaleOrderProduct(UserModel userModel, SaleOrderProduct model)
        {
            model.UserId = userModel.UserId;
			model.SaleOrderProductId = SeqGuid.NewGuid();
			model.CreateDate = DateTime.Now;
            Da.Add<Guid, SaleOrderProduct>(model);
            return true;
        }

        /// <summary>
        /// 获取一条记录
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="saleOrderProductId"></param>
        /// <returns></returns>
        public virtual SaleOrderProduct GetSaleOrderProductById(UserModel userModel, Guid saleOrderProductId)
        {
            var m = Da.Get<SaleOrderProduct>(saleOrderProductId);
            if (m == null || m.UserId != userModel.UserId)
            {
                return null;
            }
            return m;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool UpdateSaleOrderProduct(UserModel userModel, SaleOrderProduct model)
        {
            var m = Da.Get<SaleOrderProduct>(model.SaleOrderProductId);
            if (m == null || m.UserId != userModel.UserId)
            {
                return false;
            }

			m.SaleOrderId = model.SaleOrderId;
			m.ProductId = model.ProductId;
			m.Price = model.Price;
			m.Qty = model.Qty;
			m.Remark = model.Remark;

            return Da.Update(m);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool DeleteSaleOrderProduct(UserModel userModel, SaleOrderProduct model)
        {
            var m = Da.Get<SaleOrderProduct>(model.SaleOrderProductId);
            if (m == null || m.UserId != userModel.UserId)
            {
                return false;
            }

            return Da.Delete(model);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual PagenationResult<SaleOrderProduct> GetSaleOrderProductList(UserModel userModel, int pageIndex, int pageSize, string keyword)
        {
            var list = GetSaleOrderProductList(userModel);
            if (!keyword.IsWhiteSpace())
            {
                var keys = keyword.ToKeys(StringComparer.OrdinalIgnoreCase).ToList();
                list = list.Where(p => $"{p.Remark}".HasSearchKeys(keys, StringComparison.OrdinalIgnoreCase, false)).ToList();
            }
            var result = new PagenationResult<SaleOrderProduct>()
            {
                count = list.Count,
                data = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList()
            };
            return result;
        }

        /// <summary>
        /// 获取所有记录
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        public virtual List<SaleOrderProduct> GetSaleOrderProductList(UserModel userModel)
        {
            return Da.GetList<SaleOrderProduct>(new
            {
                userModel.UserId,
            });
        }
    }
}
