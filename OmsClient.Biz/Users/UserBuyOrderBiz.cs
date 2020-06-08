using DwrUtility;
using DwrUtility.Maths;
using DwrUtility.Strings;
using DwrUtility.Trees;
using OmsClient.Biz.Bases;
using OmsClient.Common;
using OmsClient.DataAccess.Users;
using OmsClient.Entity;
using OmsClient.Entity.Enums;
using OmsClient.Entity.Models;
using OmsClient.Entity.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Biz.Users
{
    /// <summary>
    /// 列表
    /// </summary>
    public partial class UserBuyOrderBiz : BaseBuyBiz
    {
        private static readonly UserBuyOrderDa Da = new UserBuyOrderDa();

        /// <summary>
        /// 统计订单数量
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="hasCheck"></param>
        /// <returns></returns>
        public List<int> GetMyOrderNumber(UserModel userModel, bool hasCheck)
        {
            var list = Da.GetSaleOrderNumberList(userModel.UserId);

            var result = new List<int>();

            //草稿
            var a = list.Count(p => p.Status == (int)BuyOrderStatusEnum.Draft);
            result.Add(a);

            //已驳回
            var b = list.Count(p => p.Status == (int)BuyOrderStatusEnum.SubmitBack); ;
            result.Add(b);

            //待审核
            var c = list.Count(p => p.Status == (int)BuyOrderStatusEnum.SubmitCheck);
            result.Add(c);

            //待提交
            var d = list.Count(p => p.Status == (int)BuyOrderStatusEnum.SubmitPass); ;
            result.Add(d);

            //待付款
            var e = list.Count(p => p.Status == (int)BuyOrderStatusEnum.PurchaseUnpay);
            result.Add(e);

            //待入库
            var f = list.Count(p => new List<int>()
            {
                (int)BuyOrderStatusEnum.Purchase,
                (int)BuyOrderStatusEnum.PurchasePay,
            }.Contains(p.Status));
            result.Add(f);

            //待完结
            var g = list.Count(p => p.Status == (int)BuyOrderStatusEnum.StockInUnpay);
            result.Add(g);

            //已完结
            var h = list.Count(p => p.Status == (int)BuyOrderStatusEnum.StockInPay);
            result.Add(h);

            return result;
        }

        /// <summary>
        /// 草稿单列表
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual PagenationResult<BuyOrderListResult> GetBuyOrderListDraft(UserModel userModel, int pageIndex, int pageSize, string keyword)
        {
            return GetListByUserStatus(userModel, pageIndex, pageSize, keyword, new List<int>()
            {
                (int)BuyOrderStatusEnum.Draft,
            }, list => list.OrderByDescending(p => p.CreateDate).ToList());
        }

        /// <summary>
        /// 待审核采购单
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual PagenationResult<BuyOrderListResult> GetBuyOrderListChecking(UserModel userModel, int pageIndex, int pageSize, string keyword)
        {
            return GetListByUserStatus(userModel, pageIndex, pageSize, keyword, new List<int>()
            {
                (int)BuyOrderStatusEnum.SubmitCheck,
            }, list => list.OrderByDescending(p => p.PostCheckDate).ToList());
        }

        /// <summary>
        /// 已退回采购单
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual PagenationResult<BuyOrderListResult> GetBuyOrderListBack(UserModel userModel, int pageIndex, int pageSize, string keyword)
        {
            return GetListByUserStatus(userModel, pageIndex, pageSize, keyword, new List<int>()
            {
                (int)BuyOrderStatusEnum.SubmitBack,
            }, list => list.OrderByDescending(p => p.CheckDate).ToList());
        }

        /// <summary>
        /// 已审核采购单
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual PagenationResult<BuyOrderListResult> GetBuyOrderListChecked(UserModel userModel, int pageIndex, int pageSize, string keyword)
        {
            return GetListByUserStatus(userModel, pageIndex, pageSize, keyword, new List<int>()
            {
                (int)BuyOrderStatusEnum.SubmitPass,
            }, list => list.OrderByDescending(p => p.CheckDate).ToList());
        }

        /// <summary>
        /// 待付款采购单
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual PagenationResult<BuyOrderListResult> GetBuyOrderListUnpay(UserModel userModel, int pageIndex, int pageSize, string keyword)
        {
            return GetListByUserStatus(userModel, pageIndex, pageSize, keyword, new List<int>()
            {
                (int)BuyOrderStatusEnum.PurchaseUnpay,
            }, list => list.OrderByDescending(p => p.PostPurcharseDate).ToList());
        }

        /// <summary>
        /// 待入库采购单
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual PagenationResult<BuyOrderListResult> GetBuyOrderListStocking(UserModel userModel, int pageIndex, int pageSize, string keyword)
        {
            return GetListByUserStatus(userModel, pageIndex, pageSize, keyword, new List<int>()
            {
                (int)BuyOrderStatusEnum.Purchase,
                (int)BuyOrderStatusEnum.PurchasePay,
            }, list => list.OrderByDescending(p => p.PostPurcharseDate).ToList());
        }

        /// <summary>
        ///  待完结采购单
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual PagenationResult<BuyOrderListResult> GetBuyOrderListUnfinished(UserModel userModel, int pageIndex, int pageSize, string keyword)
        {
            return GetListByUserStatus(userModel, pageIndex, pageSize, keyword, new List<int>()
            {
                (int)BuyOrderStatusEnum.StockInUnpay,
            }, list => list.OrderByDescending(p => p.StockInDate).ToList());
        }

        /// <summary>
        /// 已完结采购单
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual PagenationResult<BuyOrderListResult> GetBuyOrderListFinished(UserModel userModel, int pageIndex, int pageSize, string keyword)
        {
            return GetListByUserStatus(userModel, pageIndex, pageSize, keyword, new List<int>()
            {
                (int)BuyOrderStatusEnum.StockInPay,
            }, list => list.OrderByDescending(p => p.StockInDate).ToList());
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class UserBuyOrderBiz
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="model"></param>
        /// <param name="msg"></param>
        /// <param name="buyOrderId"></param>
        /// <returns></returns>
        public virtual bool AddBuyOrder(UserModel userModel, BuyOrder model, out string msg, out Guid buyOrderId)
        {
            buyOrderId = Guid.Empty;

            if (model == null)
            {
                msg = "参数错误";
                return false;
            }

            if (model.Title.IsWhiteSpace())
            {
                msg = "请输入标题";
                return false;
            }

            var currency = Da.Get<Currency>(model.CurrencyId);
            var b1 = CheckAuthCurrency(userModel, currency, out msg);
            if (!b1)
            {
                return false;
            }

            //生成订单号
            var orderNumber = Da.GetBuyNumber(userModel);

            model.BuyOrderId = SeqGuid.NewGuid();
            model.UserId = userModel.UserId;
            //订单号
            model.BuyOrderNumber = orderNumber;
            //币种
            model.CurrencyId = currency.CurrencyId;
            model.CurrencyName = currency.CurrencyName;
            model.CurrencyText = currency.CurrencyText;
            model.CurrencySymbol = currency.CurrencySymbol;
            model.CurrencyRate = currency.CurrencyRate;
            //标题
            model.Title = model.Title;

            //状态
            model.Status = (int)BuyOrderStatusEnum.Draft;

            model.UserNickName = userModel.UserNickName;
            model.CreateDate = DateTime.Now;
            Da.Add<Guid, BuyOrder>(model);

            msg = null;
            buyOrderId = model.BuyOrderId;
            return true;
        }

        /// <summary>
        /// 获取编辑数据
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="buyOrderId"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public virtual BuyOrderResult GetBuyOrderDetail(UserModel userModel, Guid buyOrderId, out string msg)
        {
            var currency = Da.GetListAsync<Currency>();
            var order = Da.Get<BuyOrder>(buyOrderId);
            var b1 = CheckAuthOrder(userModel, order, out msg);
            if (!b1)
            {
                return null;
            }

            order.DefaultSettlementName = Config.BuyDefaultSettlement.Find(p => p.Key == order.DefaultSettlement)
                ?.Value;

            Task.WaitAll(currency);
            var result = new BuyOrderResult()
            {
                BuyOrder = order,
                Currencies = currency.Result.OrderBy(p => p.Sort).ToList(),
                Count = GetBuyOrderDetailCount(userModel, order),
            };


            return result;
        }

        /// <summary>
        /// 获取订单产品列表
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="buyOrderId"></param>
        /// <returns></returns>
        public virtual PagenationResult<BuyOrderProductList> GetBuyOrderDetailProduct(UserModel userModel, Guid buyOrderId)
        {
            var cates = Da.GetListAsync<Category>();
            var order = Da.Get<BuyOrder>(buyOrderId);
            var b1 = CheckAuthOrderByUser(userModel, order, out var _);
            if (!b1)
            {
                return new PagenationResult<BuyOrderProductList>()
                {
                    count = 0,
                    data = new List<BuyOrderProductList>()
                };
            }

            var list = Da.GetList<BuyOrderProduct>(new
            {
                order.BuyOrderId,
            }).OrderBy(p => p.BuyOrderProductId.ToSqlGuid()).ToList();

            if (list.Count == 0)
            {
                return new PagenationResult<BuyOrderProductList>()
                {
                    count = 0,
                    data = new List<BuyOrderProductList>()
                };
            }

            var products = Da.GetListByBulk<Product>(new
            {
                ProductId = list.Select(p => p.ProductId).Distinct().ToList()
            });

            Task.WaitAll(cates);

            var rows = list.ToBuyOrderProduct(order, products, cates.Result);
            return new PagenationResult<BuyOrderProductList>()
            {
                count = rows.Count,
                data = rows
            };
        }

        /// <summary>
        /// 获取订单详情统计
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public virtual BuyOrderDetailCount GetBuyOrderDetailCount(UserModel userModel, BuyOrder order)
        {
            var orderProducts = Da.GetList<BuyOrderProduct>(new
            {
                order.UserId,
                order.BuyOrderId,
            });

            var products = Da.GetListByBulk<Product>(new
            {
                ProductId = orderProducts.Select(p => p.ProductId).Distinct().ToList(),
            });

            var list = orderProducts.ToBuyOrderProduct(order, products, new List<Category>());

            var result = new BuyOrderDetailCount()
            {
                CurrencySymbol = order.CurrencySymbol,
                ProductWeights = list.Sum(p => p.ProductWeight),
                ProductQtys = list.Sum(p => p.Qty),
                ProductAmount = list.Sum(p => p.Qty * p.Price),
                ShipFeight = order.ShipFeight,
                ShipFee = order.ShipFee,
                Discount = order.Discount,
                InStockTotal = list.Sum(p => p.InQty),
                InProductAmount = list.Sum(p => p.InQty * p.Price),
            };

            result.Total = result.ProductAmount + result.ShipFeight + result.ShipFee - result.Discount;
            result.TotalRmb = (result.Total * order.CurrencyRate).ToRoundDown(2);

            result.InTotal = result.InProductAmount + result.ShipFeight + result.ShipFee - result.Discount;
            result.InTotalRmb = (result.InTotal * order.CurrencyRate).ToRoundDown(2);

            //是否已入库
            var stockIns = new List<int>()
            {
                (int)BuyOrderStatusEnum.StockInUnpay,
                (int)BuyOrderStatusEnum.StockInPay,
            };
            result.IsStockIn = stockIns.Contains(order.Status);

            return result;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="model"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public virtual bool DeleteBuyOrder(UserModel userModel, BuyOrder model, out string msg)
        {
            var m = Da.Get<BuyOrder>(model.BuyOrderId);
            var b1 = CheckAuthOrderByUserDraft(userModel, m, out msg);
            if (!b1)
            {
                return false;
            }

            var list = Da.GetList<BuyOrderProduct>(new
            {
                m.BuyOrderId,
            });

            if (list.Count > 0)
            {
                msg = "请先删除产品再删除订单，操作失败";
                return false;
            }

            return Da.Delete(model);
        }

        /// <summary>
        /// 删除订单产品
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="request"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool DeleteOrderProduct(UserModel userModel, DeleteBuyOrderProductModel request, out string msg)
        {
            if (request == null || request.BuyOrderId == Guid.Empty)
            {
                msg = "参数错误";
                return false;
            }

            if (request.BuyOrderProductIds.Count == 0)
            {
                msg = "请选择要删除的产品";
                return false;
            }

            var order = Da.Get<BuyOrder>(request.BuyOrderId);
            var b1 = CheckAuthOrderByUserDraft(userModel, order, out msg);
            if (!b1)
            {
                return false;
            }

            var products = Da.GetList<BuyOrderProduct>(new
            {
                order.BuyOrderId
            });

            var deleteProducts = products.Join(request.BuyOrderProductIds, p => p.BuyOrderProductId, p => p, (p, q) => p)
                .ToList();
            if (deleteProducts.Count == 0)
            {
                msg = "删除失败";
                return false;
            }

            msg = null;
            return Da.DeleteOrderProduct(deleteProducts);
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public partial class UserBuyOrderBiz
    {
        /// <summary>
        /// 获取产品列表
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="buyOrderId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="categoryId"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual PagenationResult<BuyOrderProductSelect> GetProductSelectList(UserModel userModel, Guid buyOrderId, int pageIndex, int pageSize, int? categoryId, string keyword)
        {
            var order = Da.Get<BuyOrder>(buyOrderId);
            var b1 = CheckAuthOrderByUser(userModel, order, out var _);
            if (!b1)
            {
                return new PagenationResult<BuyOrderProductSelect>()
                {
                    data = new List<BuyOrderProductSelect>()
                };
            }
            var products = Da.GetListAsync<BuyOrderProduct>(new
            {
                order.BuyOrderId,
            });

            var cate = Da.GetListAsync<Category>();
            var list = Da.GetList<Product>(new
            {
                Status = (int)ProductStatusEnum.Ok
            })
                .OrderByDescending(p => p.ProductId.ToSqlGuid())
                .ToList();
            if (categoryId.HasValue && categoryId.Value > 0)
            {
                Task.WaitAll(cate);
                var childs = cate.Result.GetChildNodes(categoryId.Value, p => p.CategoryId, p => p.CategoryParentId, true) ?? new List<Category>();
                var categoryIds = childs.Select(p => p.CategoryId).ToList();
                if (string.IsNullOrWhiteSpace(keyword))
                {
                    list = list.Where(p => categoryIds.Contains(p.CategoryId)).ToList();
                }
                else
                {
                    var keys = keyword.ToKeys(StringComparer.OrdinalIgnoreCase).ToList();
                    list = list.Where(p => $"{p.ProductSku} {p.ProductName} {p.ProductSpecification} {p.ProductRemark} {p.CreateName} {p.AreaPosition}".HasSearchKeys(keys, StringComparison.OrdinalIgnoreCase, false) && categoryIds.Contains(p.CategoryId)).ToList();
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    var keys = keyword.ToKeys(StringComparer.OrdinalIgnoreCase).ToList();
                    list = list.Where(p => $"{p.ProductSku} {p.ProductName} {p.ProductSpecification} {p.ProductRemark} {p.CreateName} {p.AreaPosition}".HasSearchKeys(keys, StringComparison.OrdinalIgnoreCase, false)).ToList();
                }
                Task.WaitAll(cate);
            }

            var items = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            if (items.Count == 0)
            {
                return new PagenationResult<BuyOrderProductSelect>()
                {
                    data = new List<BuyOrderProductSelect>()
                };
            }

            items.ForEach(item =>
            {
                var parentCates = cate.Result.GetParentNodes(item.CategoryId, p => p.CategoryId, p => p.CategoryParentId, true);
                if (parentCates != null)
                {
                    item.CategoryName = string.Join("|", parentCates.Select(p => p.CategoryName));
                }
            });

            Task.WaitAll(products);
            var rows = items.ToBuyOrderProductSelect(order, products.Result);

            return new PagenationResult<BuyOrderProductSelect>()
            {
                count = list.Count,
                data = rows,
                PageIndex = pageIndex,
                PageSize = pageSize,
            };
        }

        /// <summary>
        /// 更新订单汇率
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="request"></param>
        /// <param name="msg"></param>
        /// <param name="rate"></param>
        /// <returns></returns>
        public virtual bool UpdateBuyOrderRate(UserModel userModel, BuyOrder request, out string msg, out decimal rate)
        {
            rate = 0;
            var m = Da.Get<BuyOrder>(request.BuyOrderId);
            var b1 = CheckAuthOrderByUserDraft(userModel, m, out msg);
            if (!b1)
            {
                return false;
            }

            var currency = Da.Get<Currency>(m.CurrencyId);
            if (currency == null)
            {
                return false;
            }

            if (m.CurrencyRate != currency.CurrencyRate)
            {
                m.CurrencyRate = currency.CurrencyRate;
                Da.Update(m);
            }

            rate = m.CurrencyRate;
            return true;
        }

        /// <summary>
        /// 添加订单产品
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="request"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool AddOrderProduct(UserModel userModel, BuyOrderProduct request, out string msg)
        {
            var order = Da.Get<BuyOrder>(request.BuyOrderId);
            var b1 = CheckAuthOrderByUserDraft(userModel, order, out msg);
            if (!b1)
            {
                return false;
            }

            var product = Da.Get<Product>(request.ProductId);
            var b2 = CheckAuthProduct(userModel, product, out msg);
            if (!b2)
            {
                return false;
            }

            var m = new BuyOrderProduct()
            {
                BuyOrderProductId = SeqGuid.NewGuid(),
                UserId = userModel.UserId,
                BuyOrderId = order.BuyOrderId,
                ProductId = product.ProductId,
                Price = request.Price,
                Qty = request.Qty,
                InQty = request.Qty,
                Remark = request.Remark,
                CreateDate = DateTime.Now,
                IsStockIn = false,
                IsNewAdd = false,
            };

            msg = null;
            return Da.AddOrderProduct(m);
        }

        /// <summary>
        /// 修改订单产品
        /// </summary>
        /// <returns></returns>
        public bool UpdateOrderProduct(UserModel userModel, BuyUpdateBuyOrderProductRequest request, out string msg)
        {
            var order = Da.Get<BuyOrder>(request.BuyOrderId);
            var b2 = CheckAuthOrderByUserDraft(userModel, order, out msg);
            if (!b2)
            {
                return false;
            }

            var orderProducts = Da.GetList<BuyOrderProduct>(new
            {
                order.BuyOrderId,
                order.UserId
            });
            var products = Da.GetListByBulk<Product>(new
            {
                ProductId = orderProducts.Select(p => p.ProductId).Distinct().ToList(),
                Status = (int)ProductStatusEnum.Ok,
            });

            var updateOrderProducts = new List<BuyOrderProduct>();
            var updateProducts = new List<Product>();
            foreach (var item in request.BuyOrderProducts)
            {
                var op = orderProducts.Find(p => p.BuyOrderProductId == item.BuyOrderProductId);
                if (op == null)
                {
                    msg = "产品列表已发生变化，请刷新再试";
                    return false;
                }

                //必须存在
                var pro = products.Find(p => p.ProductId == op.ProductId);
                if (pro == null)
                {
                    LogHelper.Fatal($"产品[{item.BuyOrderProductId}]不存在，这类是bug！");
                    msg = "操作失败";
                    return false;
                }

                if (op.Qty != item.Qty || op.Price != item.Price || op.Remark != item.Remark)
                {
                    //产品
                    var addTotal = item.Qty - op.Qty;
                    if (addTotal != 0)
                    {
                        pro.InTransitQty += addTotal;
                        updateProducts.Add(pro);
                    }

                    //订单产品
                    op.Qty = item.Qty;
                    op.InQty = item.Qty;
                    op.Price = item.Price;
                    op.Remark = item.Remark;
                    updateOrderProducts.Add(op);
                }
            }

            if (updateProducts.Count == 0 && updateOrderProducts.Count == 0)
            {
                return true;
            }

            return Da.UpdateBuyOrderProduct(updateProducts, updateOrderProducts);
        }

        /// <summary>
        /// 修改订单详情
        /// </summary>
        /// <returns></returns>
        public bool UpdateOrder(UserModel userModel, BuyOrder request, out string msg)
        {
            if (request == null || request.BuyOrderId == Guid.Empty)
            {
                msg = "参数不正确";
                return false;
            }

            var order = Da.Get<BuyOrder>(request.BuyOrderId);
            var b1 = CheckAuthOrderByUserDraft(userModel, order, out msg);
            if (!b1)
            {
                return false;
            }

            if (!Config.BuyDefaultSettlement.Exists(p => p.Key == request.DefaultSettlement))
            {
                msg = "请选择结款方式";
                return false;
            }

            //修改了币种
            if (order.CurrencyId != request.CurrencyId)
            {
                var currency = Da.Get<Currency>(request.CurrencyId);
                var b2 = CheckAuthCurrency(userModel, currency, out msg);
                if (!b2)
                {
                    return false;
                }

                //修改币种
                order.CurrencyId = currency.CurrencyId;
                order.CurrencyName = currency.CurrencyName;
                order.CurrencyText = currency.CurrencyText;
                order.CurrencySymbol = currency.CurrencySymbol;
                order.CurrencyRate = currency.CurrencyRate;
            }

            //修改标题
            order.Title = request.Title;

            //
            order.ArrivalsDate = request.ArrivalsDate;
            order.ShipFeight = request.ShipFeight;
            order.ShipFee = request.ShipFee;
            order.Discount = request.Discount;

            //
            order.DefaultSettlement = request.DefaultSettlement;
            order.DefaultShip = request.DefaultShip;
            order.DefaultAccount = request.DefaultAccount;
            order.DefaultRemark = request.DefaultRemark;

            //
            order.SupplierCompany = request.SupplierCompany;
            order.SupplierName = request.SupplierName;
            order.SupplierTel = request.SupplierTel;
            order.SupplierMobilePhone = request.SupplierMobilePhone;
            order.SupplierEmail = request.SupplierEmail;
            order.SupplierQQ = request.SupplierQQ;
            order.SupplierWechat = request.SupplierWechat;
            order.SupplierAddress = request.SupplierAddress;

            Da.Update(order);
            return true;
        }

        /// <summary>
        /// 草稿提交订单
        /// </summary>
        /// <returns></returns>
        public bool PostOrder(UserModel userModel, BuyOrder request, out string msg)
        {
            if (request == null || request.BuyOrderId == Guid.Empty)
            {
                msg = "参数不正确";
                return false;
            }

            var order = Da.Get<BuyOrder>(request.BuyOrderId);
            var b1 = CheckAuthOrderByUserDraft(userModel, order, out msg);
            if (!b1)
            {
                return false;
            }

            if (order.Title.IsWhiteSpace())
            {
                msg = "请输入标题";
                return false;
            }

            if (!order.ArrivalsDate.HasValue)
            {
                msg = "请输入预计到货日期";
                return false;
            }

            if (order.DefaultShip.IsWhiteSpace())
            {
                msg = "请输入发货方式";
                return false;
            }

            var settlement = Config.BuyDefaultSettlement.Find(p => p.Key == order.DefaultSettlement);
            if (settlement == null)
            {
                msg = "请选择结款方式";
                return false;
            }

            if (order.DefaultAccount.IsWhiteSpace())
            {
                msg = "请输入收款账号";
                return false;
            }

            //需要审核
            if (Config.IsBuyCheck)
            {
                order.Status = (int)BuyOrderStatusEnum.SubmitCheck;
                order.PostCheckDate = DateTime.Now;
            }
            //不需要审核
            else
            {
                //结款方式：1付款后发货；2自提并付款；3收货后付款；4周付；5半月付；6月付；7季付；8半年付；9年付；10其他
                if (order.DefaultSettlement <= 2)
                {
                    //先付款
                    order.Status = (int)BuyOrderStatusEnum.PurchaseUnpay;
                    order.PostPurcharseDate = DateTime.Now;
                }
                else
                {
                    //入库后付款
                    order.Status = (int)BuyOrderStatusEnum.Purchase;
                    order.PostPurcharseDate = DateTime.Now;
                }
            }

            Da.Update(order);

            return true;
        }

        /// <summary>
        /// 审核通过提交订单
        /// </summary>
        /// <returns></returns>
        public bool PostCheckedOrder(UserModel userModel, BuyOrder request, out string msg)
        {
            if (request == null || request.BuyOrderId == Guid.Empty)
            {
                msg = "参数不正确";
                return false;
            }

            var order = Da.Get<BuyOrder>(request.BuyOrderId);
            var b1 = CheckAuthOrderByUser(userModel, order, out msg);
            if (!b1)
            {
                return false;
            }

            if (order.Status != (int)BuyOrderStatusEnum.SubmitPass)
            {
                msg = "订单状态已改变，提交失败";
                return false;
            }

            //结款方式：1付款后发货；2自提并付款；3收货后付款；4周付；5半月付；6月付；7季付；8半年付；9年付；10其他
            if (order.DefaultSettlement <= 2)
            {
                //先付款
                order.Status = (int)BuyOrderStatusEnum.PurchaseUnpay;
                order.PostPurcharseDate = DateTime.Now;
            }
            else
            {
                //入库后付款
                order.Status = (int)BuyOrderStatusEnum.Purchase;
                order.PostPurcharseDate = DateTime.Now;
            }

            Da.Update(order);

            return true;
        }

        /// <summary>
        /// 退回修改
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="model"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public virtual bool ReturnBackBuyOrder(UserModel userModel, BuyOrder model, out string msg)
        {
            var m = Da.Get<BuyOrder>(model.BuyOrderId);
            var b1 = CheckAuthOrderByUser(userModel, m, out msg);
            if (!b1)
            {
                return false;
            }

            var status = new List<int>()
            {
                (int)BuyOrderStatusEnum.PurchasePay,
                (int)BuyOrderStatusEnum.StockInUnpay,
                (int)BuyOrderStatusEnum.StockInPay,
            };

            if (status.Exists(p => p == m.Status))
            {
                msg = "当前订单不能退回修改，操作失败";
                return false;
            }

            m.Status = (int)BuyOrderStatusEnum.Draft;

            msg = null;
            return Da.Update(m);
        }

        /// <summary>
        /// 获取急需采购产品
        /// </summary>
        public PagenationResult<Product> GetUrgentPurchase(UserModel userModel, int pageIndex, int pageSize, int? categoryId, string keyword)
        {
            var cate = Da.GetListAsync<Category>();

            var list = Da.GetList<Product>();

            //分类筛选
            if (categoryId.HasValue && categoryId.Value > 0)
            {
                Task.WaitAll(cate);
                var childs = cate.Result.GetChildNodes(categoryId.Value, p => p.CategoryId, p => p.CategoryParentId, true) ?? new List<Category>();
                var categoryIds = childs.Select(p => p.CategoryId).ToList();
                list = list.Where(p => categoryIds.Contains(p.CategoryId)).ToList();
            }

            //关键词搜索
            if (!keyword.IsWhiteSpace())
            {
                var keys = keyword.ToKeys(StringComparer.OrdinalIgnoreCase).ToList();
                list = list.Where(p => $"{p.ProductSku} {p.ProductName} {p.ProductSpecification} {p.ProductRemark} {p.AreaPosition}".HasSearchKeys(keys, StringComparison.OrdinalIgnoreCase, false)).ToList();
            }

            //已经缺货数量
            list = list.Where(p => p.InTransitQty + p.SaleQty < 0).OrderBy(p => p.InTransitQty + p.SaleQty).ToList();

            var result = new PagenationResult<Product>()
            {
                count = list.Count,
                data = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList(),
                PageIndex = pageIndex,
                PageSize = pageSize,
            };

            //分类
            if (result.data.Count > 0)
            {
                Task.WaitAll(cate);
                result.data.ForEach(item =>
                {
                    var parentCates = cate.Result.GetParentNodes(item.CategoryId, p => p.CategoryId, p => p.CategoryParentId, true);
                    if (parentCates != null)
                    {
                        item.CategoryName = string.Join("|", parentCates.Select(p => p.CategoryName));
                    }
                });
            }

            return result;
        }
    }
}