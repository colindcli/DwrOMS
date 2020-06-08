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
    public abstract class BaseUserAccountBiz
    {
        protected static readonly UserAccountDa Da = new UserAccountDa();

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool AddAccount(UserModel userModel, Account model)
        {
            model.AccountId = SeqGuid.NewGuid();
            model.CreateDate = DateTime.Now;
            Da.Add<Guid, Account>(model);
            return true;
        }

        /// <summary>
        /// 获取一条记录
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public virtual Account GetAccountById(UserModel userModel, Guid accountId)
        {
            return Da.Get<Account>(accountId);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool UpdateAccount(UserModel userModel, Account model)
        {
            var m = Da.Get<Account>(model.AccountId);
            if (m == null)
            {
                return false;
            }

            m.AccountType = model.AccountType;
            m.AccountName = model.AccountName;
            m.AccountBank = model.AccountBank;
            m.AccountNumber = model.AccountNumber;
            m.AccountAddress = model.AccountAddress;
            m.AccountRemark = model.AccountRemark;
            m.Sort = model.Sort;

            return Da.Update(m);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="model"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public virtual bool DeleteAccount(UserModel userModel, Account model, out string msg)
        {
            var m = Da.Get<Account>(model.AccountId);
            if (m == null)
            {
                msg = "账户不存在，删除失败";
                return false;
            }

            var param = new
            {
                m.AccountId
            };

            var list = new List<string>();
            var buyOrders = Da.GetList<SaleReceive>(param);
            var buyPays = Da.GetList<BuyPay>(param);

            if (buyOrders.Count > 0)
            {
                msg = $"账户关联了{buyOrders.Count}个销售单里的收款；";
                list.Add(msg);
            }

            if (buyPays.Count > 0)
            {
                msg = $"账户关联了{buyPays.Count}个采购单里的付款；";
                list.Add(msg);
            }

            if (list.Count > 0)
            {
                msg = $"删除失败，解除以下关联后才能删除。<br>{string.Join("<br>", list)}";
                return false;
            }

            msg = null;
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
        public virtual PagenationResult<Account> GetAccountList(UserModel userModel, int pageIndex, int pageSize, string keyword)
        {
            var list = GetAccountList(userModel);
            if (!keyword.IsWhiteSpace())
            {
                var keys = keyword.ToKeys(StringComparer.OrdinalIgnoreCase).ToList();
                list = list.Where(p => $"{p.AccountName} {p.AccountBank} {p.AccountNumber} {p.AccountAddress} {p.AccountRemark}".HasSearchKeys(keys, StringComparison.OrdinalIgnoreCase, false)).ToList();
            }
            var result = new PagenationResult<Account>()
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
        public virtual List<Account> GetAccountList(UserModel userModel)
        {
            return Da.GetList<Account>().OrderBy(p => p.Sort).ToList();
        }
    }
}
