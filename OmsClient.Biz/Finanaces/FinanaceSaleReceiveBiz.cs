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
    public class FinanaceSaleReceiveBiz : BaseOrderBiz
    {
        protected static readonly FinanaceSaleReceiveDa Da = new FinanaceSaleReceiveDa();

        /// <summary>
        /// 币种和账号
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="type">0全部账号；1收款；2付款</param>
        /// <returns></returns>
        public FinanceAccountCurrencyResult GetAccountCurrency(UserModel userModel, int type)
        {
            var currency = Da.GetList<Currency>().OrderBy(p => p.Sort)
                .ToList();
            var account = Da.GetList<Account>();
            if (type == 1)
            {
                account = account.Where(p => p.AccountType == 1 || p.AccountType == 3).OrderBy(p => p.Sort).ToList();
            }
            else if (type == 2)
            {
                account = account.Where(p => p.AccountType == 2 || p.AccountType == 3).OrderBy(p => p.Sort).ToList();
            }

            return new FinanceAccountCurrencyResult()
            {
                Accounts = account,
                Currencies = currency,
            };
        }

        /// <summary>
        /// 获取财务进度记录
        /// </summary>
        /// <returns></returns>
        public SaleReceiveResult GetSaleReceiveRrcord(UserModel userModel, Guid saleOrderId, out string msg)
        {
            var order = Da.Get<SaleOrder>(saleOrderId);
            var b1 = CheckAuthOrder(userModel, order, out msg);
            if (!b1)
            {
                return null;
            }

            var list = GetSaleReceiveList(userModel, saleOrderId);

            string receiveStatusName;
            string receiveStatusClass;
            switch ((SaleOrderStatusEnum)order.Status)
            {
                case SaleOrderStatusEnum.SubmitUnpay:
                case SaleOrderStatusEnum.StockedUnpay:
                    {
                        receiveStatusName = list.Count > 0 ? "未确认(待提交)" : "未收款";
                        receiveStatusClass = "layui-bg-orange";
                        break;
                    }
                case SaleOrderStatusEnum.SubmitAfterPay:
                case SaleOrderStatusEnum.StockedAfterPay:
                case SaleOrderStatusEnum.ShipedAfterPay:
                    {
                        receiveStatusName = order.FinancePostDate.HasValue
                            ? $"{order.FinancePostName}于{order.FinancePostDate.Value:MM/dd HH:mm}已标志为【{order.FinanceRemark}】"
                            : $"{order.FinancePostName}已标志为【{order.FinanceRemark}】";

                        receiveStatusClass = "layui-bg-cyan";
                        break;
                    }
                case SaleOrderStatusEnum.SubmitPay:
                case SaleOrderStatusEnum.StockedPay:
                case SaleOrderStatusEnum.ShipedPay:
                    {
                        receiveStatusName = "已确认完成收款";
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

            var result = new SaleReceiveResult()
            {
                SaleReceives = list,
                TotalRmb = list.Sum(p => (p.Amount - p.TranFee) * p.CurrencyRate),
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
        public virtual bool AddSaleReceive(UserModel userModel, SaleReceive model, out string msg)
        {
            var order = Da.Get<SaleOrder>(model.SaleOrderId);
            var b1 = CheckAuthOrder(userModel, order, out msg);
            if (!b1)
            {
                return false;
            }

            var status = new List<int>()
            {
                (int) SaleOrderStatusEnum.SubmitUnpay,
                (int) SaleOrderStatusEnum.SubmitAfterPay,
                (int) SaleOrderStatusEnum.StockedUnpay,
                (int) SaleOrderStatusEnum.StockedAfterPay,
                (int) SaleOrderStatusEnum.ShipedAfterPay,
            };

            if (!status.Exists(p => p == order.Status))
            {
                msg = "订单已锁定，操作失败";
                return false;
            }

            var account = Da.Get<Account>(model.AccountId);
            var b2 = CheckAuthAccount(userModel, account, out msg);
            if (!b2)
            {
                return false;
            }

            var currency = Da.Get<Currency>(model.CurrencyId);
            var b3 = CheckAuthCurrency(userModel, currency, out msg);
            if (!b3)
            {
                return false;
            }

            var m = new SaleReceive()
            {
                SaleReceiveId = SeqGuid.NewGuid(),
                SaleOrderId = model.SaleOrderId,
                AccountInfo = $"{account.AccountBank}({account.AccountNumber})",
                Amount = model.Amount,
                TranFee = model.TranFee,
                CurrencySymbol = currency.CurrencySymbol,
                CurrencyRate = currency.CurrencyRate,
                TransactionNumber = model.TransactionNumber,
                Remark = model.Remark,
                CreateName = userModel.UserNickName,
                CreateDate = DateTime.Now,
                AccountId = account.AccountId,
                CurrencyId = currency.CurrencyId,
            };

            Da.Add<Guid, SaleReceive>(m);
            return true;
        }

        /// <summary>
        /// 获取一条记录
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="saleReceiveId"></param>
        /// <returns></returns>
        public virtual SaleReceive GetSaleReceiveById(UserModel userModel, Guid saleReceiveId)
        {
            return Da.Get<SaleReceive>(saleReceiveId);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="model"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public virtual bool UpdateSaleReceive(UserModel userModel, SaleReceive model, out string msg)
        {
            var m = Da.Get<SaleReceive>(model.SaleReceiveId);
            if (m == null)
            {
                msg = "收款记录已不存在";
                return false;
            }

            var order = Da.Get<SaleOrder>(m.SaleOrderId);
            var b1 = CheckAuthOrder(userModel, order, out msg);
            if (!b1)
            {
                return false;
            }

            var status = new List<int>()
            {
                (int) SaleOrderStatusEnum.SubmitUnpay,
                (int) SaleOrderStatusEnum.SubmitAfterPay,
                (int) SaleOrderStatusEnum.StockedUnpay,
                (int) SaleOrderStatusEnum.StockedAfterPay,
                (int) SaleOrderStatusEnum.ShipedAfterPay,
            };

            if (!status.Exists(p => p == order.Status))
            {
                msg = "订单已锁定，操作失败";
                return false;
            }

            var account = Da.Get<Account>(model.AccountId);
            var b2 = CheckAuthAccount(userModel, account, out msg);
            if (!b2)
            {
                return false;
            }

            var currency = Da.Get<Currency>(model.CurrencyId);
            var b3 = CheckAuthCurrency(userModel, currency, out msg);
            if (!b3)
            {
                return false;
            }

            m.AccountInfo = $"{account.AccountBank}({account.AccountNumber})";
            m.Amount = model.Amount;

            m.CurrencySymbol = currency.CurrencySymbol;
            m.CurrencyRate = currency.CurrencyRate;

            m.Amount = model.Amount;
            m.TranFee = model.TranFee;
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
        public virtual bool DeleteSaleReceive(UserModel userModel, SaleReceive model, out string msg)
        {
            var m = Da.Get<SaleReceive>(model.SaleReceiveId);
            if (m == null || m.SaleOrderId != model.SaleOrderId)
            {
                msg = "记录不存在";
                return false;
            }

            var order = Da.Get<SaleOrder>(m.SaleOrderId);
            var b1 = CheckAuthOrder(userModel, order, out msg);
            if (!b1)
            {
                return false;
            }

            var status = new List<int>()
            {
                (int) SaleOrderStatusEnum.SubmitUnpay,
                (int) SaleOrderStatusEnum.SubmitAfterPay,
                (int) SaleOrderStatusEnum.StockedUnpay,
                (int) SaleOrderStatusEnum.StockedAfterPay,
                (int) SaleOrderStatusEnum.ShipedAfterPay,
            };

            if (!status.Exists(p => p == order.Status))
            {
                msg = "订单已锁定，操作失败";
                return false;
            }

            msg = null;
            return Da.Delete(m);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="saleOrderId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual PagenationResult<SaleReceive> GetSaleReceiveList(UserModel userModel, Guid saleOrderId, int pageIndex, int pageSize, string keyword)
        {
            var list = GetSaleReceiveList(userModel, saleOrderId);
            if (!keyword.IsWhiteSpace())
            {
                var keys = keyword.ToKeys(StringComparer.OrdinalIgnoreCase).ToList();
                list = list.Where(p => $"{p.AccountInfo} {p.CurrencySymbol} {p.Remark} {p.CreateName}".HasSearchKeys(keys, StringComparison.OrdinalIgnoreCase, false)).ToList();
            }
            var result = new PagenationResult<SaleReceive>()
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
        /// <param name="saleOrderId"></param>
        /// <returns></returns>
        public virtual List<SaleReceive> GetSaleReceiveList(UserModel userModel, Guid saleOrderId)
        {
            return Da.GetList<SaleReceive>(new
            {
                SaleOrderId = saleOrderId,
            }).OrderBy(p => p.CreateDate).ToList();
        }
    }
}
