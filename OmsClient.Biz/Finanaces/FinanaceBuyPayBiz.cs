using DwrUtility;
using DwrUtility.Strings;
using OmsClient.DataAccess.Finances;
using OmsClient.Entity;
using OmsClient.Entity.Enums;
using OmsClient.Entity.Results;
using System;
using System.Collections.Generic;
using System.Linq;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Biz.Finanaces
{
    public class FinanaceBuyPayBiz : BaseOrderBiz
    {
        protected static readonly FinanaceBuyPayDa Da = new FinanaceBuyPayDa();

        /// <summary>
        /// 币种和账号
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="type">0全部账号；1收款；2付款</param>
        /// <returns></returns>
        public FinanceAccountCurrencyResult GetAccountCurrency(UserModel userModel, int type)
        {
            return new FinanaceSaleReceiveBiz().GetAccountCurrency(userModel, type);
        }

        /// <summary>
        /// 获取财务进度记录
        /// </summary>
        /// <returns></returns>
        public BuyPayResult GetBuyPayRrcord(UserModel userModel, Guid buyOrderId, out string msg)
        {
            var order = Da.Get<BuyOrder>(buyOrderId);
            var b1 = CheckAuthOrder(userModel, order, out msg);
            if (!b1)
            {
                return null;
            }

            var list = GetBuyPayList(userModel, buyOrderId);

            string receiveStatusName;
            string receiveStatusClass;
            switch ((BuyOrderStatusEnum)order.Status)
            {
                case BuyOrderStatusEnum.SubmitCheck:
                case BuyOrderStatusEnum.SubmitBack:
                    {
                        receiveStatusName = "-";
                        receiveStatusClass = "layui-bg-gray";
                        break;
                    }
                case BuyOrderStatusEnum.SubmitPass:
                    {
                        receiveStatusName = "验货后付款";
                        receiveStatusClass = "layui-bg-cyan";
                        break;
                    }
                case BuyOrderStatusEnum.Purchase:
                    {
                        receiveStatusName = "验货后付款";
                        receiveStatusClass = "layui-bg-cyan";
                        break;
                    }
                case BuyOrderStatusEnum.PurchaseUnpay:
                case BuyOrderStatusEnum.StockInUnpay:
                    {
                        receiveStatusName = list.Count > 0 ? "未确认付款" : "等待付款";
                        receiveStatusClass = "layui-bg-orange";
                        break;
                    }
                case BuyOrderStatusEnum.PurchasePay:
                case BuyOrderStatusEnum.StockInPay:
                    {
                        receiveStatusName = "已确认完成付款";
                        receiveStatusClass = "layui-bg-blue";
                        break;
                    }
                default:
                    {
                        receiveStatusName = "-";
                        receiveStatusClass = "";
                        break;
                    }
            }

            var result = new BuyPayResult()
            {
                BuyPays = list,
                TotalRmb = list.Sum(p => p.Amount * p.CurrencyRate),
                ReceiveStatusName = receiveStatusName,
                ReceiveStatusClass = receiveStatusClass,
            };

            return result;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="model"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public virtual bool AddBuyPay(UserModel userModel, BuyPay model, out string msg)
        {
            var order = Da.Get<BuyOrder>(model.BuyOrderId);
            var b1 = CheckAuthOrder(userModel, order, out msg);
            if (!b1)
            {
                return false;
            }

            var b2 = CheckAuthFinanceOrder(userModel, order, out msg);
            if (!b2)
            {
                return false;
            }

            var account = Da.Get<Account>(model.AccountId);
            var b3 = CheckAuthAccount(userModel, account, out msg);
            if (!b3)
            {
                return false;
            }

            var currency = Da.Get<Currency>(model.CurrencyId);
            var b4 = CheckAuthCurrency(userModel, currency, out msg);
            if (!b4)
            {
                return false;
            }

            var m = new BuyPay()
            {
                BuyPayId = SeqGuid.NewGuid(),
                BuyOrderId = model.BuyOrderId,
                AccountInfo = $"{account.AccountBank}({account.AccountNumber})",
                Amount = model.Amount,
                CurrencySymbol = currency.CurrencySymbol,
                CurrencyRate = currency.CurrencyRate,
                TransactionNumber = model.TransactionNumber,
                Remark = model.Remark,
                CreateName = userModel.UserNickName,
                CreateDate = DateTime.Now,
                AccountId = account.AccountId,
                CurrencyId = currency.CurrencyId,
            };

            Da.Add<Guid, BuyPay>(m);
            return true;
        }

        /// <summary>
        /// 获取一条记录
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="buyReceiveId"></param>
        /// <returns></returns>
        public virtual BuyPay GetBuyPayById(UserModel userModel, Guid buyReceiveId)
        {
            return Da.Get<BuyPay>(buyReceiveId);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="model"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public virtual bool UpdateBuyPay(UserModel userModel, BuyPay model, out string msg)
        {
            var m = Da.Get<BuyPay>(model.BuyPayId);
            if (m == null)
            {
                msg = "收款记录已不存在";
                return false;
            }

            var order = Da.Get<BuyOrder>(m.BuyOrderId);
            var b1 = CheckAuthOrder(userModel, order, out msg);
            if (!b1)
            {
                return false;
            }

            var b2 = CheckAuthFinanceOrder(userModel, order, out msg);
            if (!b2)
            {
                return false;
            }

            var account = Da.Get<Account>(model.AccountId);
            var b3 = CheckAuthAccount(userModel, account, out msg);
            if (!b3)
            {
                return false;
            }

            var currency = Da.Get<Currency>(model.CurrencyId);
            var b4 = CheckAuthCurrency(userModel, currency, out msg);
            if (!b4)
            {
                return false;
            }

            m.AccountInfo = $"{account.AccountBank}({account.AccountNumber})";
            m.Amount = model.Amount;

            m.CurrencySymbol = currency.CurrencySymbol;
            m.CurrencyRate = currency.CurrencyRate;

            m.Amount = model.Amount;
            m.TransactionNumber = model.TransactionNumber;
            m.Remark = model.Remark;

            m.CreateName = userModel.UserNickName;
            m.CreateDate = DateTime.Now;
            m.CurrencyId = model.CurrencyId;
            m.AccountId = model.AccountId;

            return Da.Update(m);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="model"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public virtual bool DeleteBuyPay(UserModel userModel, BuyPay model, out string msg)
        {
            var m = Da.Get<BuyPay>(model.BuyPayId);
            if (m == null || m.BuyOrderId != model.BuyOrderId)
            {
                msg = "记录不存在";
                return false;
            }

            var order = Da.Get<BuyOrder>(m.BuyOrderId);
            var b1 = CheckAuthOrder(userModel, order, out msg);
            if (!b1)
            {
                return false;
            }

            var b2 = CheckAuthFinanceOrder(userModel, order, out msg);
            if (!b2)
            {
                return false;
            }

            msg = null;
            return Da.Delete(m);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="buyOrderId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual PagenationResult<BuyPay> GetBuyPayList(UserModel userModel, Guid buyOrderId, int pageIndex, int pageSize, string keyword)
        {
            var list = GetBuyPayList(userModel, buyOrderId);
            if (!keyword.IsWhiteSpace())
            {
                var keys = keyword.ToKeys(StringComparer.OrdinalIgnoreCase).ToList();
                list = list.Where(p => $"{p.AccountInfo} {p.CurrencySymbol} {p.Remark} {p.CreateName}".HasSearchKeys(keys, StringComparison.OrdinalIgnoreCase, false)).ToList();
            }
            var result = new PagenationResult<BuyPay>()
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
        /// <param name="buyOrderId"></param>
        /// <returns></returns>
        public virtual List<BuyPay> GetBuyPayList(UserModel userModel, Guid buyOrderId)
        {
            return Da.GetList<BuyPay>(new
            {
                BuyOrderId = buyOrderId,
            }).OrderBy(p => p.CreateDate).ToList();
        }
    }
}
