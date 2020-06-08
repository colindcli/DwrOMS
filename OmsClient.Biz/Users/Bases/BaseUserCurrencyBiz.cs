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
    public abstract class BaseUserCurrencyBiz
    {
        protected static readonly UserCurrencyDa Da = new UserCurrencyDa();

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool AddCurrency(UserModel userModel, Currency model)
        {
            model.CurrencyId = SeqGuid.NewGuid();
            model.CreateDate = DateTime.Now;
            Da.Add<Guid, Currency>(model);
            return true;
        }

        /// <summary>
        /// 获取一条记录
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="currencyId"></param>
        /// <returns></returns>
        public virtual Currency GetCurrencyById(UserModel userModel, Guid currencyId)
        {
            return Da.Get<Currency>(currencyId);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool UpdateCurrency(UserModel userModel, Currency model)
        {
            var m = Da.Get<Currency>(model.CurrencyId);
            if (m == null)
            {
                return false;
            }

            m.CurrencyName = model.CurrencyName;
            m.CurrencyText = model.CurrencyText;
            m.CurrencySymbol = model.CurrencySymbol;
            m.CurrencyRate = model.CurrencyRate;
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
        public virtual bool DeleteCurrency(UserModel userModel, Currency model, out string msg)
        {
            var m = Da.Get<Currency>(model.CurrencyId);
            if (m == null)
            {
                msg = "币种不存在，操作失败";
                return false;
            }

            var param = new
            {
                m.CurrencyId
            };

            var list = new List<string>();
            var buyOrders = Da.GetList<BuyOrder>(param);
            var buyPays = Da.GetList<BuyPay>(param);
            var saleOrders = Da.GetList<SaleOrder>(param);
            var saleReceives = Da.GetList<SaleReceive>(param);
            var saleTrack = Da.GetList<SaleTrack>(param);

            if (buyOrders.Count > 0)
            {
                msg = $"币种关联了{buyOrders.Count}个采购单；";
                list.Add(msg);
            }

            if (buyPays.Count > 0)
            {
                msg = $"币种关联了{buyPays.Count}个采购单里的付款；";
                list.Add(msg);
            }

            if (saleOrders.Count > 0)
            {
                msg = $"币种关联了{saleOrders.Count}个销售单；";
                list.Add(msg);
            }

            if (saleReceives.Count > 0)
            {
                msg = $"币种关联了{saleReceives.Count}个销售单里的收款；";
                list.Add(msg);
            }

            if (saleTrack.Count > 0)
            {
                msg = $"币种关联了{saleTrack.Count}个销售单里的运费；";
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
        public virtual PagenationResult<Currency> GetCurrencyList(UserModel userModel, int pageIndex, int pageSize, string keyword)
        {
            var list = GetCurrencyList(userModel);
            if (!keyword.IsWhiteSpace())
            {
                var keys = keyword.ToKeys(StringComparer.OrdinalIgnoreCase).ToList();
                list = list.Where(p => $"{p.CurrencyName} {p.CurrencyText} {p.CurrencySymbol}".HasSearchKeys(keys, StringComparison.OrdinalIgnoreCase, false)).ToList();
            }
            var result = new PagenationResult<Currency>()
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
        public virtual List<Currency> GetCurrencyList(UserModel userModel)
        {
            return Da.GetList<Currency>().OrderBy(p => p.Sort).ToList();
        }
    }
}
