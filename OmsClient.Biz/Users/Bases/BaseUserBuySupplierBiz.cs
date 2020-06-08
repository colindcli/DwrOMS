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
    public abstract class BaseUserBuySupplierBiz
    {
        protected static readonly UserBuySupplierDa Da = new UserBuySupplierDa();

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool AddBuySupplier(UserModel userModel, BuySupplier model)
        {
            model.UserId = userModel.UserId;
			model.BuySupplierId = SeqGuid.NewGuid();
			model.CreateDate = DateTime.Now;
            Da.Add<Guid, BuySupplier>(model);
            return true;
        }

        /// <summary>
        /// 获取一条记录
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="buySupplierId"></param>
        /// <returns></returns>
        public virtual BuySupplier GetBuySupplierById(UserModel userModel, Guid buySupplierId)
        {
            var m = Da.Get<BuySupplier>(buySupplierId);
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
        public virtual bool UpdateBuySupplier(UserModel userModel, BuySupplier model)
        {
            var m = Da.Get<BuySupplier>(model.BuySupplierId);
            if (m == null || m.UserId != userModel.UserId)
            {
                return false;
            }

			m.SupplierCompany = model.SupplierCompany;
			m.SupplierName = model.SupplierName;
			m.SupplierTel = model.SupplierTel;
			m.SupplierMobilePhone = model.SupplierMobilePhone;
			m.SupplierEmail = model.SupplierEmail;
			m.SupplierQQ = model.SupplierQQ;
			m.SupplierWechat = model.SupplierWechat;
			m.SupplierAddress = model.SupplierAddress;
			m.SupplierRemark = model.SupplierRemark;

            return Da.Update(m);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool DeleteBuySupplier(UserModel userModel, BuySupplier model)
        {
            var m = Da.Get<BuySupplier>(model.BuySupplierId);
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
        public virtual PagenationResult<BuySupplier> GetBuySupplierList(UserModel userModel, int pageIndex, int pageSize, string keyword)
        {
            var list = GetBuySupplierList(userModel);
            if (!keyword.IsWhiteSpace())
            {
                var keys = keyword.ToKeys(StringComparer.OrdinalIgnoreCase).ToList();
                list = list.Where(p => $"{p.SupplierCompany} {p.SupplierName} {p.SupplierTel} {p.SupplierMobilePhone} {p.SupplierEmail} {p.SupplierQQ} {p.SupplierWechat} {p.SupplierAddress} {p.SupplierRemark}".HasSearchKeys(keys, StringComparison.OrdinalIgnoreCase, false)).ToList();
            }
            var result = new PagenationResult<BuySupplier>()
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
        public virtual List<BuySupplier> GetBuySupplierList(UserModel userModel)
        {
            return Da.GetList<BuySupplier>(new
            {
                userModel.UserId,
            });
        }
    }
}
