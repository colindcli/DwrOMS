using DwrExcel;
using DwrUtility;
using DwrUtility.Maths;
using DwrUtility.Strings;
using DwrUtility.Trees;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using OmsClient.Biz.Users.Bases;
using OmsClient.Biz.Warehouses;
using OmsClient.Common;
using OmsClient.Entity;
using OmsClient.Entity.Enums;
using OmsClient.Entity.Models;
using OmsClient.Entity.Results;
using System;
using System.Collections.Generic;
using System.IO;
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
    public partial class UserSaleOrderBiz
    {
        /// <summary>
        /// 统计订单数量
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        public List<int> GetMyOrderNumber(UserModel userModel)
        {
            var list = Da.GetSaleOrderNumberList(userModel.UserId);

            var result = new List<int>();

            //草稿
            var a = list.Count(p => p.Status == (int)SaleOrderStatusEnum.Draft);
            result.Add(a);

            //待备货
            var b = list.Count(p => new List<int>()
            {
                (int)SaleOrderStatusEnum.SubmitUnpay,
                (int)SaleOrderStatusEnum.SubmitAfterPay,
                (int)SaleOrderStatusEnum.SubmitPay,
            }.Contains(p.Status));
            result.Add(b);

            //待收款
            var c = list.Count(p => p.Status == (int)SaleOrderStatusEnum.StockedUnpay);
            result.Add(c);

            //待发货
            var d = list.Count(p => new List<int>()
            {
                (int)SaleOrderStatusEnum.StockedAfterPay,
                (int)SaleOrderStatusEnum.StockedPay,
            }.Contains(p.Status));
            result.Add(d);

            //已发货
            var e = list.Count(p => p.Status == (int)SaleOrderStatusEnum.ShipedAfterPay);
            result.Add(e);

            //已完成
            var f = list.Count(p => p.Status == (int)SaleOrderStatusEnum.ShipedPay);
            result.Add(f);

            return result;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <param name="status"></param>
        /// <param name="orderBy">排序</param>
        /// <returns></returns>
        private static PagenationResult<SaleOrderListResult> GetListByUserStatus(UserModel userModel, int pageIndex, int pageSize, string keyword, List<int> status, Func<List<SaleOrder>, List<SaleOrder>> orderBy)
        {
            var list = Da.GetListByBulk<SaleOrder>(new
            {
                userModel.UserId,
                Status = status
            });

            //排序
            list = orderBy.Invoke(list);

            if (!keyword.IsWhiteSpace())
            {
                var keys = keyword.ToKeys(StringComparer.OrdinalIgnoreCase).ToList();
                list = list.Where(p => $"{p.SaleOrderNumber} {p.CurrencyName} {p.CurrencyText} {p.CurrencySymbol} {p.Title} {p.ToConsignee} {p.ToCompanyName} {p.ToTelphone} {p.ToAddress} {p.ToRemark} {p.StockStartName} {p.StockEndName} {p.StockRemark}".HasSearchKeys(keys, StringComparison.OrdinalIgnoreCase, false)).ToList();
            }
            var result = new PagenationResult<SaleOrderListResult>()
            {
                count = list.Count,
                data = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToSaleOrderListResult()
            };
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
        public virtual PagenationResult<SaleOrderListResult> GetSaleOrderListDraft(UserModel userModel, int pageIndex, int pageSize, string keyword)
        {
            return GetListByUserStatus(userModel, pageIndex, pageSize, keyword, new List<int>()
            {
                (int)SaleOrderStatusEnum.Draft
            }, list => list.OrderByDescending(p => p.CreateDate).ToList());
        }

        /// <summary>
        /// 待备货订单
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual PagenationResult<SaleOrderListResult> GetSaleOrderListStocking(UserModel userModel, int pageIndex, int pageSize, string keyword)
        {
            return GetListByUserStatus(userModel, pageIndex, pageSize, keyword, new List<int>()
            {
                (int)SaleOrderStatusEnum.SubmitUnpay,
                (int)SaleOrderStatusEnum.SubmitAfterPay,
                (int)SaleOrderStatusEnum.SubmitPay,
            }, list => list.OrderByDescending(p => p.PostDate).ToList());
        }

        /// <summary>
        /// 待收款订单
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual PagenationResult<SaleOrderListResult> GetSaleOrderListUnpay(UserModel userModel, int pageIndex, int pageSize, string keyword)
        {
            return GetListByUserStatus(userModel, pageIndex, pageSize, keyword, new List<int>()
            {
                (int)SaleOrderStatusEnum.StockedUnpay,
            }, list => list.OrderByDescending(p => p.StockEnd).ToList());
        }

        /// <summary>
        /// 待发货订单
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual PagenationResult<SaleOrderListResult> GetSaleOrderListUnship(UserModel userModel, int pageIndex, int pageSize, string keyword)
        {
            return GetListByUserStatus(userModel, pageIndex, pageSize, keyword, new List<int>()
            {
                (int)SaleOrderStatusEnum.StockedAfterPay,
                (int)SaleOrderStatusEnum.StockedPay,
            }, list => list.OrderByDescending(p => p.StockEnd).ToList());
        }

        /// <summary>
        /// 已发货订单
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual PagenationResult<SaleOrderListResult> GetSaleOrderListShiped(UserModel userModel, int pageIndex, int pageSize, string keyword)
        {
            return GetListByUserStatus(userModel, pageIndex, pageSize, keyword, new List<int>()
            {
                (int)SaleOrderStatusEnum.ShipedAfterPay,
            }, list => list.OrderByDescending(p => p.StockOutDate).ToList());
        }

        /// <summary>
        /// 已完结订单
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual PagenationResult<SaleOrderListResult> GetSaleOrderListFinished(UserModel userModel, int pageIndex, int pageSize, string keyword)
        {
            return GetListByUserStatus(userModel, pageIndex, pageSize, keyword, new List<int>()
            {
                (int)SaleOrderStatusEnum.ShipedPay,
            }, list => list.OrderByDescending(p => p.StockOutDate).ToList());
        }
    }

    /// <summary>
    /// 验证权限
    /// </summary>
    public partial class UserSaleOrderBiz
    {
        /// <summary>
        /// 验证权限：公司、订单用户、采购订单
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="order"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private static bool CheckAuthOrderByUserDraft(UserModel userModel, SaleOrder order, out string msg)
        {
            if (order == null)
            {
                msg = "订单不存在，操作失败";
                return false;
            }

            if (order.UserId != userModel.UserId)
            {
                msg = "不是你的订单，操作失败";
                return false;
            }

            if (order.Status != (int)SaleOrderStatusEnum.Draft)
            {
                msg = "订单状态已经改变，操作失败";
                return false;
            }

            msg = null;
            return true;
        }

        /// <summary>
        /// 验证权限：公司、订单用户
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="order"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private static bool CheckAuthOrderByUser(UserModel userModel, SaleOrder order, out string msg)
        {
            if (order == null)
            {
                msg = "订单不存在，操作失败";
                return false;
            }

            if (order.UserId != userModel.UserId)
            {
                msg = "不是你的订单，操作失败";
                return false;
            }

            msg = null;
            return true;
        }

        /// <summary>
        /// 验证权限：公司
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="order"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private static bool CheckAuthOrder(UserModel userModel, SaleOrder order, out string msg)
        {
            if (order == null)
            {
                msg = "订单不存在，操作失败";
                return false;
            }

            msg = null;
            return true;
        }

        /// <summary>
        /// 验证权限：公司
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="product"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private static bool CheckAuthProduct(UserModel userModel, Product product, out string msg)
        {
            if (product == null)
            {
                msg = "产品不存在，操作失败";
                return false;
            }

            msg = null;
            return true;
        }

        /// <summary>
        /// 验证权限：公司
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="currency"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private static bool CheckAuthCurrency(UserModel userModel, Currency currency, out string msg)
        {
            if (currency == null)
            {
                msg = "币种不存在，操作失败";
                return false;
            }

            msg = null;
            return true;
        }

        /// <summary>
        /// 验证权限：公司
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="account"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private static bool CheckAuthAccount(UserModel userModel, Account account, out string msg)
        {
            if (account == null)
            {
                msg = "账户不存在，操作失败";
                return false;
            }

            msg = null;
            return true;
        }
    }

    /// <summary>
    /// 公共方法
    /// </summary>
    public partial class UserSaleOrderBiz : BaseUserSaleOrderBiz
    {
        /// <summary>
        /// 获取编辑数据
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="saleOrderId"></param>
        /// <returns></returns>
        public virtual SaleOrderResult GetSaleOrderDetail(UserModel userModel, Guid saleOrderId)
        {
            var currency = Da.GetListAsync<Currency>();
            var order = Da.Get<SaleOrder>(saleOrderId);
            var b1 = CheckAuthOrderByUser(userModel, order, out var _);
            if (!b1)
            {
                return null;
            }

            Task.WaitAll(currency);
            return new SaleOrderResult()
            {
                SaleOrder = order,
                Currencies = currency.Result.OrderBy(p => p.Sort).ToList(),
                StockInfo = WarehouseSaleOrderBiz.GetSaleOrderStockingResult(order),
                Count = GetSaleOrderDetailCount(userModel, order),
            };
        }

        /// <summary>
        /// 获取订单产品列表
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="saleOrderId"></param>
        /// <returns></returns>
        public virtual PagenationResult<SaleOrderProductList> GetSaleOrderDetailProduct(UserModel userModel, Guid saleOrderId)
        {
            var cates = Da.GetListAsync<Category>();
            var order = Da.Get<SaleOrder>(saleOrderId);
            var b1 = CheckAuthOrderByUser(userModel, order, out var _);
            if (!b1)
            {
                return new PagenationResult<SaleOrderProductList>()
                {
                    count = 0,
                    data = new List<SaleOrderProductList>()
                };
            }

            var list = Da.GetList<SaleOrderProduct>().OrderBy(p => p.SaleOrderProductId.ToSqlGuid()).ToList();

            if (list.Count == 0)
            {
                return new PagenationResult<SaleOrderProductList>()
                {
                    count = 0,
                    data = new List<SaleOrderProductList>()
                };
            }

            var products = Da.GetListByBulk<Product>(new
            {
                ProductId = list.Select(p => p.ProductId).Distinct().ToList()
            });

            Task.WaitAll(cates);

            var rows = list.ToSaleOrderProduct(order, products, cates.Result);
            return new PagenationResult<SaleOrderProductList>()
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
        public virtual SaleOrderDetailCount GetSaleOrderDetailCount(UserModel userModel, SaleOrder order)
        {
            var orderProducts = Da.GetList<SaleOrderProduct>(new
            {
                order.UserId,
                order.SaleOrderId,
            });

            var products = Da.GetListByBulk<Product>(new
            {
                ProductId = orderProducts.Select(p => p.ProductId).Distinct().ToList(),
            });

            var list = orderProducts.ToSaleOrderProduct(order, products, new List<Category>());

            var result = new SaleOrderDetailCount()
            {
                CurrencySymbol = order.CurrencySymbol,
                ProductWeights = list.Sum(p => p.ProductWeight),
                ProductQtys = list.Sum(p => p.Qty),
                ProductAmount = list.Sum(p => p.Qty * p.Price),
                ShipFeight = order.ShipFeight,
                ShipFee = order.ShipFee,
                Discount = order.Discount,
            };

            result.Total = result.ProductAmount + result.ShipFeight + result.ShipFee - result.Discount;
            result.TotalRmb = (result.Total * order.CurrencyRate).ToRoundDown(2);

            return result;
        }
    }

    /// <summary>
    /// 草稿订单
    /// </summary>
    public partial class UserSaleOrderBiz
    {
        /// <summary>
        /// 获取产品列表
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="saleOrderId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="categoryId"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual PagenationResult<SaleOrderProductSelect> GetProductSelectList(UserModel userModel, Guid saleOrderId, int pageIndex, int pageSize, int? categoryId, string keyword)
        {
            var order = Da.Get<SaleOrder>(saleOrderId);
            var b1 = CheckAuthOrderByUser(userModel, order, out var _);
            if (!b1)
            {
                return new PagenationResult<SaleOrderProductSelect>()
                {
                    data = new List<SaleOrderProductSelect>()
                };
            }
            var products = Da.GetListAsync<SaleOrderProduct>(new
            {
                order.SaleOrderId,
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
                return new PagenationResult<SaleOrderProductSelect>()
                {
                    data = new List<SaleOrderProductSelect>()
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
            var rows = items.ToSaleOrderProductSelect(order, products.Result);

            return new PagenationResult<SaleOrderProductSelect>()
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
        public virtual bool UpdateSaleOrderRate(UserModel userModel, SaleOrder request, out string msg, out decimal rate)
        {
            rate = 0;
            var m = Da.Get<SaleOrder>(request.SaleOrderId);
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
        /// 添加订单
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="model"></param>
        /// <param name="msg"></param>
        /// <param name="saleOrderId"></param>
        /// <returns></returns>
        public virtual bool AddSaleOrder(UserModel userModel, SaleOrder model, out string msg, out Guid saleOrderId)
        {
            saleOrderId = Guid.Empty;

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
            var orderNumber = Da.GetSaleNumber(userModel);

            model.SaleOrderId = SeqGuid.NewGuid();
            model.UserId = userModel.UserId;
            model.UserNickName = userModel.UserNickName;
            //订单号
            model.SaleOrderNumber = orderNumber;
            //币种
            model.CurrencyId = currency.CurrencyId;
            model.CurrencyName = currency.CurrencyName;
            model.CurrencyText = currency.CurrencyText;
            model.CurrencySymbol = currency.CurrencySymbol;
            model.CurrencyRate = currency.CurrencyRate;
            //标题
            model.Title = model.Title;
            //状态
            model.Status = (int)SaleOrderStatusEnum.Draft;

            model.ShipDate = DateTime.Today.AddDays(1);
            model.CreateDate = DateTime.Now;
            Da.Add<Guid, SaleOrder>(model);

            msg = null;
            saleOrderId = model.SaleOrderId;
            return true;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="model"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public virtual bool DeleteSaleOrder(UserModel userModel, SaleOrder model, out string msg)
        {
            var m = Da.Get<SaleOrder>(model.SaleOrderId);
            var b1 = CheckAuthOrderByUserDraft(userModel, m, out msg);
            if (!b1)
            {
                return false;
            }

            var list = Da.GetList<SaleOrderProduct>(new
            {
                m.SaleOrderId,
                m.UserId
            });
            if (list.Count > 0)
            {
                msg = "请先删除订单产品，再删除订单";
                return false;
            }

            msg = null;
            return Da.Delete(model);
        }

        /// <summary>
        /// 删除订单产品
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="request"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool DeleteOrderProduct(UserModel userModel, DeleteSaleOrderProductModel request, out string msg)
        {
            if (request == null || request.SaleOrderId == Guid.Empty)
            {
                msg = "参数错误";
                return false;
            }

            if (request.SaleOrderProductIds.Count == 0)
            {
                msg = "请选择要删除的产品";
                return false;
            }

            var order = Da.Get<SaleOrder>(request.SaleOrderId);
            var b1 = CheckAuthOrderByUserDraft(userModel, order, out msg);
            if (!b1)
            {
                return false;
            }

            var products = Da.GetList<SaleOrderProduct>(new
            {
                order.SaleOrderId
            });

            var deleteProducts = products.Join(request.SaleOrderProductIds, p => p.SaleOrderProductId, p => p, (p, q) => p)
                .ToList();
            if (deleteProducts.Count == 0)
            {
                msg = "删除失败";
                return false;
            }

            msg = null;
            return Da.DeleteOrderProduct(deleteProducts);
        }

        /// <summary>
        /// 添加订单产品
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="request"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool AddOrderProduct(UserModel userModel, SaleOrderProduct request, out string msg)
        {
            var order = Da.Get<SaleOrder>(request.SaleOrderId);
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

            //最低起订量
            if (product.OrderQty > request.Qty)
            {
                msg = $"最低起订量为{product.SaleQty}";
                return false;
            }

            var m = new SaleOrderProduct()
            {
                SaleOrderProductId = SeqGuid.NewGuid(),
                UserId = userModel.UserId,
                SaleOrderId = order.SaleOrderId,
                ProductId = product.ProductId,
                Price = request.Price,
                Qty = request.Qty,
                Remark = request.Remark,
                CreateDate = DateTime.Now,
                IsStockOut = false,
            };

            msg = null;
            return Da.AddOrderProduct(m);
        }

        /// <summary>
        /// 修改订单产品
        /// </summary>
        /// <returns></returns>
        public bool UpdateOrderProduct(UserModel userModel, SaleUpdateSaleOrderProductRequest request, out string msg)
        {
            var order = Da.Get<SaleOrder>(request.SaleOrderId);
            var b2 = CheckAuthOrderByUserDraft(userModel, order, out msg);
            if (!b2)
            {
                return false;
            }

            var orderProducts = Da.GetList<SaleOrderProduct>(new
            {
                order.SaleOrderId,
                order.UserId
            });
            var products = Da.GetListByBulk<Product>(new
            {
                ProductId = orderProducts.Select(p => p.ProductId).Distinct().ToList(),
                Status = (int)ProductStatusEnum.Ok,
            });

            var updateOrderProducts = new List<SaleOrderProduct>();
            var updateProducts = new List<Product>();
            foreach (var item in request.SaleOrderProducts)
            {
                var op = orderProducts.Find(p => p.SaleOrderProductId == item.SaleOrderProductId);
                if (op == null)
                {
                    msg = "产品列表已发生变化，请刷新再试";
                    return false;
                }

                //必须存在
                var pro = products.Find(p => p.ProductId == op.ProductId);
                if (pro == null)
                {
                    LogHelper.Fatal($"产品[{item.SaleOrderProductId}]不存在，这类是bug！");
                    msg = "操作失败";
                    return false;
                }

                if (item.Qty < pro.OrderQty)
                {
                    msg = $"产品编码为[{pro.ProductSku}]的最少起订量：{pro.OrderQty}";
                    return false;
                }

                if (op.Qty != item.Qty || op.Price != item.Price || op.Remark != item.Remark)
                {
                    //产品
                    var addTotal = item.Qty - op.Qty;
                    if (addTotal != 0)
                    {
                        pro.HoldQty += addTotal;
                        pro.SaleQty -= addTotal;
                        updateProducts.Add(pro);
                    }

                    //订单产品
                    op.Qty = item.Qty;
                    op.Price = item.Price;
                    op.Remark = item.Remark;
                    updateOrderProducts.Add(op);
                }
            }

            if (updateProducts.Count == 0 && updateOrderProducts.Count == 0)
            {
                return true;
            }

            return Da.UpdateSaleOrderProduct(updateProducts, updateOrderProducts);
        }

        /// <summary>
        /// 修改订单详情
        /// </summary>
        /// <returns></returns>
        public bool UpdateOrder(UserModel userModel, SaleOrder request, out string msg)
        {
            if (request == null || request.SaleOrderId == Guid.Empty)
            {
                msg = "参数不正确";
                return false;
            }

            var order = Da.Get<SaleOrder>(request.SaleOrderId);
            var b1 = CheckAuthOrderByUserDraft(userModel, order, out msg);
            if (!b1)
            {
                return false;
            }

            if (!Config.SaleDefaultSettlement.Exists(p => p.Key == request.DefaultSettlement))
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
            order.ShipDate = request.ShipDate;
            order.ShipFeight = request.ShipFeight;
            order.ShipFee = request.ShipFee;
            order.Discount = request.Discount;

            //
            order.DefaultSettlement = request.DefaultSettlement;
            order.DefaultShip = request.DefaultShip;
            order.DefaultAccount = request.DefaultAccount;
            order.DefaultRemark = request.DefaultRemark;

            //
            order.ToConsignee = request.ToConsignee;
            order.ToCompanyName = request.ToCompanyName;
            order.ToTelphone = request.ToTelphone;
            order.ToZipcode = request.ToZipcode;
            order.ToAddress = request.ToAddress;
            order.ToRemark = request.ToRemark;

            Da.Update(order);
            return true;
        }

        /// <summary>
        /// 提交订单
        /// </summary>
        /// <returns></returns>
        public bool PostOrder(UserModel userModel, SaleOrder request, out string msg)
        {
            if (request == null || request.SaleOrderId == Guid.Empty)
            {
                msg = "参数不正确";
                return false;
            }

            var order = Da.Get<SaleOrder>(request.SaleOrderId);
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

            if (!order.ShipDate.HasValue)
            {
                msg = "请输入发货日期";
                return false;
            }

            if (order.DefaultShip.IsWhiteSpace())
            {
                msg = "请输入发货方式";
                return false;
            }

            var settlement = Config.SaleDefaultSettlement.Find(p => p.Key == order.DefaultSettlement);
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

            if (order.ToConsignee.IsWhiteSpace())
            {
                msg = "请输入收货人";
                return false;
            }

            if (order.ToTelphone.IsWhiteSpace())
            {
                msg = "请输入收货人电话";
                return false;
            }

            if (order.ToAddress.IsWhiteSpace())
            {
                msg = "请输入收货地址";
                return false;
            }

            //结款方式：1先收款后发货；2提货结款；3送货结；4邮到结；5周结；6半月结；7月结；8季结；9半年结；10年结；
            if (order.DefaultSettlement >= 5 && order.DefaultSettlement <= 10)
            {
                order.FinancePostDate = DateTime.Now;
                order.FinancePostName = userModel.UserNickName;
                order.FinanceRemark = settlement.Value;
                //修改状态
                order.Status = (int)SaleOrderStatusEnum.SubmitAfterPay;
            }
            else
            {
                //修改状态
                order.Status = (int)SaleOrderStatusEnum.SubmitUnpay;
            }

            order.PostDate = DateTime.Now;
            Da.Update(order);

            return true;
        }

        /// <summary>
        /// 导入产品
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="saleOrderId"></param>
        /// <param name="fullPath"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool ImportProduct(UserModel userModel, Guid saleOrderId, string fullPath, out string msg)
        {
            if (saleOrderId == Guid.Empty)
            {
                msg = "销售单号为空";
                return false;
            }

            if (!File.Exists(fullPath))
            {
                msg = "上传失败";
                return false;
            }

            var order = Da.Get<SaleOrder>(saleOrderId);
            var b1 = CheckAuthOrderByUserDraft(userModel, order, out msg);
            if (!b1)
            {
                return false;
            }

            var rows = ExcelHelper.ReadSheetColumn26(fullPath);

            var list = new List<SaleOrderProduct>();
            var errors = new List<string>();
            for (var i = 0; i < rows.Count; i++)
            {
                if (i == 0)
                {
                    //首行标题
                    continue;
                }

                var model = rows[i];
                var sku = model.A?.ToString();
                var b = model.B?.ToString();
                var c = model.C?.ToString();
                var remark = model.D?.ToString();

                var qty = 0;
                decimal price = 0;

                if (sku.IsWhiteSpace())
                {
                    errors.Add($"第{i + 1}行的编码为空");
                }
                if (b.IsWhiteSpace())
                {
                    errors.Add($"第{i + 1}行的数量为空");
                }
                else
                {
                    if (!int.TryParse(b, out qty))
                    {
                        errors.Add($"第{i + 1}行的数量数据类型错误");
                    }
                }
                if (c.IsWhiteSpace())
                {
                    errors.Add($"第{i + 1}行的价格为空");
                }
                else
                {
                    if (!decimal.TryParse(b, out price))
                    {
                        errors.Add($"第{i + 1}行的价格数据类型错误");
                    }
                }

                var m = new SaleOrderProduct()
                {
                    SaleOrderProductId = SeqGuid.NewGuid(),
                    UserId = userModel.UserId,
                    SaleOrderId = order.SaleOrderId,
                    ProductSku = sku,
                    Price = price,
                    Qty = qty,
                    Remark = remark,
                    CreateDate = DateTime.Now,
                    IsStockOut = false,
                };

                list.Add(m);
            }

            if (errors.Count > 0)
            {
                msg = string.Join(";", errors);
                return false;
            }

            //编码转productId
            var products = Da.GetList<Product>(new
            {
                Status = (int)ProductStatusEnum.Ok,
            });

            //验证编码是否都正确
            foreach (var item in list)
            {
                var obj = products.Find(p => p.ProductSku.IsEquals(item.ProductSku));
                if (obj == null)
                {
                    errors.Add(item.ProductSku);
                    continue;
                }

                item.ProductId = obj.ProductId;
            }

            if (errors.Count > 0)
            {
                msg = $"编码不存在：{string.Join(";", errors)}";
                return false;
            }

            msg = null;
            return Da.ImportProduct(list);
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name=""></param>
        /// <param name="userModel"></param>
        /// <param name="saleOrderId"></param>
        /// <param name="msg"></param>
        /// <param name="bt"></param>
        /// <param name="saleOrder"></param>
        /// <returns></returns>
        public bool ExportProduct(UserModel userModel, Guid saleOrderId, out string msg, out byte[] bt, out SaleOrder saleOrder)
        {
            bt = null;

            var order = Da.Get<SaleOrder>(saleOrderId);
            saleOrder = order;
            var b1 = CheckAuthOrderByUser(userModel, order, out msg);
            if (!b1)
            {
                return false;
            }


            var sb = "";
            switch (order.CurrencyText.ToUpper())
            {
                case "RMB":
                case "CNY":
                    {
                        sb = "Rmb";
                        break;
                    }
                case "USD":
                    {
                        sb = "Usd";
                        break;
                    }
                case "EUR":
                    {
                        sb = "Eur";
                        break;
                    }
                case "GBP":
                    {
                        sb = "Gbp";
                        break;
                    }
                case "AUD":
                    {
                        sb = "Aud";
                        break;
                    }
            }
            var path = $"{Config.Root}/static/files/Pi{sb}.xlsx";
            if (!File.Exists(path))
            {
                msg = "配置错误";
                return false;
            }

            var info = Da.GetList<SaleConfig>(new
            {
                userModel.UserId,
            }).FirstOrDefault() ?? new SaleConfig();

            if (info.PiCompanyName.IsWhiteSpace())
            {
                info.PiCompanyName = "[公司名称] 没有配置，请到【个人设置】配置";
            }
            if (info.PiCompanyAddress.IsWhiteSpace())
            {
                info.PiCompanyAddress = "[公司地址] 没有配置，请到【个人设置】配置";
            }
            if (info.PiMyContact.IsWhiteSpace())
            {
                info.PiMyContact = "[联系方式] 没有配置，请到【个人设置】配置";
            }

            //产品
            var orderProducts = GetSaleOrderDetailProduct(userModel, saleOrderId);
            var products = orderProducts.data;

            //统计
            var count = GetSaleOrderDetailCount(userModel, order);

            bt = ExcelHelper.Export(path, wb =>
            {
                var sheet = wb.GetSheetAt(0);

                sheet.GetRow(0).Cells[0].SetCellValue(info.PiCompanyName);
                sheet.GetRow(1).Cells[0].SetCellValue(info.PiCompanyAddress);
                sheet.GetRow(2).Cells[0].SetCellValue(info.PiMyContact);
                //
                sheet.GetRow(4).Cells[1].SetCellValue(order.SaleOrderNumber);
                sheet.GetRow(4).Cells[6].SetCellValue(Convert.ToDateTime(order.CreateDate).ToString("yyyy/MM/dd"));
                sheet.GetRow(5).Cells[1].SetCellValue(order.ToConsignee);
                sheet.GetRow(5).Cells[6].SetCellValue(order.ToCompanyName);
                sheet.GetRow(6).Cells[1].SetCellValue(order.ToAddress);
                sheet.GetRow(7).Cells[1].SetCellValue(order.ToZipcode);
                sheet.GetRow(8).Cells[1].SetCellValue(order.ToTelphone);
                //
                for (var i = 0; i < products.Count - 1; i++)
                {
                    sheet.CopyRowSample(i + 11, i + 12);
                    sheet.AddMergedRegion(new CellRangeAddress(i + 12, i + 12, 1, 2));
                    sheet.AddMergedRegion(new CellRangeAddress(i + 12, i + 12, 3, 4));
                }
                var rs = products.Count;
                var index = 0;
                foreach (var item in products)
                {
                    var rows = sheet.GetRow(index + 11);
                    rows.Cells[0].SetCellValue(item.ProductSku);
                    rows.Cells[1].SetCellValue(item.CategoryName);
                    //单元格2合并到1
                    rows.Cells[3].SetCellValue(item.ProductName);
                    //单元格4合并到3
                    rows.Cells[5].SetCellValue(item.ProductSpecification);
                    if (item.ImageQty > 0)
                    {
                        rows.Cells[6].SetCellValue("Picture");
                        var hssfHyperlink = new XSSFHyperlink(HyperlinkType.Url)
                        {
                            Address = $"{Config.WebUrl}/ProductImage?id={item.ProductId}",
                        };
                        rows.Cells[6].Hyperlink = hssfHyperlink;
                    }
                    else
                    {
                        rows.Cells[6].SetCellValue("");
                    }
                    rows.Cells[7].SetCellValue(item.Qty);
                    rows.Cells[8].SetCellType(CellType.Numeric);
                    rows.Cells[8].SetCellValue(Convert.ToDouble(item.Price));
                    rows.Cells[9].SetCellType(CellType.Numeric);
                    rows.Cells[9].SetCellValue(Convert.ToDouble(item.Price * item.Qty));
                    rows.Cells[10].SetCellValue(item.Remark);
                    rows.Cells[11].SetCellValue(item.ProductWeight * item.Qty);

                    index++;
                }
                //数量总计
                sheet.GetRow(11 + rs).Cells[7].SetCellFormula($"SUM(H12:H{rs + 11})");
                sheet.GetRow(11 + rs).Cells[7].SetCellValue(Convert.ToDouble(count.ProductQtys));
                //产品总计
                sheet.GetRow(11 + rs).Cells[9].SetCellFormula($"SUM(J12:J{rs + 11})");
                sheet.GetRow(11 + rs).Cells[9].SetCellValue(Convert.ToDouble(count.ProductAmount));
                //产品总计
                sheet.GetRow(12 + rs).Cells[8].SetCellFormula($"SUM(J12:J{rs + 11})");
                sheet.GetRow(12 + rs).Cells[8].SetCellValue(Convert.ToDouble(count.ProductAmount));

                //Shipping Cost
                sheet.GetRow(13 + rs).Cells[8].SetCellType(CellType.Numeric);
                sheet.GetRow(13 + rs).Cells[8].SetCellValue(Convert.ToDouble(count.ShipFeight));
                //Other Fee
                sheet.GetRow(14 + rs).Cells[8].SetCellType(CellType.Numeric);
                sheet.GetRow(14 + rs).Cells[8].SetCellValue(Convert.ToDouble(count.ShipFee));
                //Discount
                sheet.GetRow(15 + rs).Cells[8].SetCellType(CellType.Numeric);
                sheet.GetRow(15 + rs).Cells[8].SetCellValue(Convert.ToDouble(count.Discount));
                //Total
                sheet.GetRow(16 + rs).Cells[8].SetCellType(CellType.Numeric);
                sheet.GetRow(16 + rs).Cells[8].SetCellFormula($"SUM(I{(13 + rs)}+I{(14 + rs)}+I{(15 + rs)}-I{(16 + rs)})");
                sheet.GetRow(16 + rs).Cells[8].SetCellValue(Convert.ToDouble(count.Total));

                //Shipping Method
                sheet.GetRow(19 + rs).Cells[2].SetCellValue(order.DefaultShip);
                //Payment
                sheet.GetRow(20 + rs).Cells[2].SetCellValue(order.DefaultAccount);
                //Seller
                sheet.GetRow(23 + rs).Cells[0].SetCellValue(userModel.UserNickName);
                //Buyer
                sheet.GetRow(23 + rs).Cells[5].SetCellValue(order.ToCompanyName.IsWhiteSpace() ? order.ToConsignee : order.ToCompanyName);
            });

            msg = null;
            return true;
        }
    }

    /// <summary>
    /// 待备货订单
    /// </summary>
    public partial class UserSaleOrderBiz
    {
        /// <summary>
        /// 退回修改
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="model"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public virtual bool ReturnBackSaleOrder(UserModel userModel, SaleOrder model, out string msg)
        {
            var m = Da.Get<SaleOrder>(model.SaleOrderId);
            var b1 = CheckAuthOrderByUser(userModel, m, out msg);
            if (!b1)
            {
                return false;
            }

            if (m.Status != (int)SaleOrderStatusEnum.SubmitUnpay)
            {
                msg = "当前订单已开始备货或者已收款，不能退回修改了";
                return false;
            }

            if (m.StockStart.HasValue)
            {
                msg = "当前订单已备货，不能退回修改了";
                return false;
            }

            m.Status = (int)SaleOrderStatusEnum.Draft;

            msg = null;
            return Da.Update(m);
        }
    }
}