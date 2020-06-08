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
    public abstract class BaseUserBuyOrderProductBiz
    {
        protected static readonly UserBuyOrderProductDa Da = new UserBuyOrderProductDa();

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool AddBuyOrderProduct(UserModel userModel, BuyOrderProduct model)
        {
            model.UserId = userModel.UserId;
			model.BuyOrderProductId = SeqGuid.NewGuid();
			model.CreateDate = DateTime.Now;
            Da.Add<Guid, BuyOrderProduct>(model);
            return true;
        }

        /// <summary>
        /// 获取一条记录
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="buyOrderProductId"></param>
        /// <returns></returns>
        public virtual BuyOrderProduct GetBuyOrderProductById(UserModel userModel, Guid buyOrderProductId)
        {
            var m = Da.Get<BuyOrderProduct>(buyOrderProductId);
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
        public virtual bool UpdateBuyOrderProduct(UserModel userModel, BuyOrderProduct model)
        {
            var m = Da.Get<BuyOrderProduct>(model.BuyOrderProductId);
            if (m == null || m.UserId != userModel.UserId)
            {
                return false;
            }

			m.BuyOrderId = model.BuyOrderId;
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
        public virtual bool DeleteBuyOrderProduct(UserModel userModel, BuyOrderProduct model)
        {
            var m = Da.Get<BuyOrderProduct>(model.BuyOrderProductId);
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
        public virtual PagenationResult<BuyOrderProduct> GetBuyOrderProductList(UserModel userModel, int pageIndex, int pageSize, string keyword)
        {
            var list = GetBuyOrderProductList(userModel);
            if (!keyword.IsWhiteSpace())
            {
                var keys = keyword.ToKeys(StringComparer.OrdinalIgnoreCase).ToList();
                list = list.Where(p => $"{p.Remark}".HasSearchKeys(keys, StringComparison.OrdinalIgnoreCase, false)).ToList();
            }
            var result = new PagenationResult<BuyOrderProduct>()
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
        public virtual List<BuyOrderProduct> GetBuyOrderProductList(UserModel userModel)
        {
            return Da.GetList<BuyOrderProduct>(new
            {
                userModel.UserId,
            });
        }
    }
}
