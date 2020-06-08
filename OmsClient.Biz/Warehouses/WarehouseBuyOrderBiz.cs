using DwrExcel;
using DwrUtility;
using DwrUtility.Strings;
using OmsClient.Biz.Users;
using OmsClient.Common;
using OmsClient.DataAccess.Warehouses;
using OmsClient.Entity;
using OmsClient.Entity.Enums;
using OmsClient.Entity.Results;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OmsClient.DataAccess.Users;
using OmsClient.Entity.Models;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Biz.Warehouses
{
    /// <summary>
    /// 
    /// </summary>
    public class WarehouseBuyOrderBiz : BaseOrderBiz
    {
        private static readonly WarehouseBuyOrderDa Da = new WarehouseBuyOrderDa();

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <param name="status"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        private static PagenationResult<BuyOrderListResult> GetListByStatus(UserModel userModel, int pageIndex, int pageSize, string keyword, List<int> status, Func<List<BuyOrder>, List<BuyOrder>> orderBy)
        {
            var list = Da.GetListByBulk<BuyOrder>(new
            {
                Status = status
            });

            //排序
            list = orderBy.Invoke(list);

            if (!keyword.IsWhiteSpace())
            {
                var keys = keyword.ToKeys(StringComparer.OrdinalIgnoreCase).ToList();
                list = list.Where(p => $"{p.BuyOrderNumber} {p.Title} {p.DefaultShip} {p.DefaultAccount} {p.DefaultRemark} {p.SupplierCompany} {p.SupplierName} {p.SupplierTel} {p.SupplierMobilePhone} {p.SupplierEmail} {p.SupplierQQ} {p.SupplierWechat} {p.SupplierAddress} {p.StockInRemark}".HasSearchKeys(keys, StringComparison.OrdinalIgnoreCase, false)).ToList();
            }
            var result = new PagenationResult<BuyOrderListResult>()
            {
                count = list.Count,
                data = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToBuyOrderListResult()
            };
            return result;
        }

        /// <summary>
        /// 统计订单数量
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        public List<int> GetMyOrderNumber(UserModel userModel)
        {
            var list = new UserBuyOrderDa().GetSaleOrderNumberList();

            var result = new List<int>();

            //待入库列表
            var a = list.Count(p => new List<int>()
            {
                (int)BuyOrderStatusEnum.Purchase,
                (int)BuyOrderStatusEnum.PurchaseUnpay,
                (int)BuyOrderStatusEnum.PurchasePay,
            }.Contains(p.Status));
            result.Add(a);

            //已入库列表
            var b = list.Count(p => new List<int>()
            {
                (int)BuyOrderStatusEnum.StockInUnpay,
                (int)BuyOrderStatusEnum.StockInPay,
            }.Contains(p.Status));
            result.Add(b);

            return result;
        }

        /// <summary>
        /// 待入库列表
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual PagenationResult<BuyOrderListResult> GetUnstockInOrder(UserModel userModel, int pageIndex, int pageSize, string keyword)
        {
            return GetListByStatus(userModel, pageIndex, pageSize, keyword, new List<int>()
            {
                (int)BuyOrderStatusEnum.Purchase,
                (int)BuyOrderStatusEnum.PurchaseUnpay,
                (int)BuyOrderStatusEnum.PurchasePay,
            }, list => list.OrderByDescending(p => p.PostPurcharseDate).ToList());
        }

        /// <summary>
        /// 已入库列表
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual PagenationResult<BuyOrderListResult> GetStockInOrder(UserModel userModel, int pageIndex, int pageSize, string keyword)
        {
            return GetListByStatus(userModel, pageIndex, pageSize, keyword, new List<int>()
            {
                (int)BuyOrderStatusEnum.StockInUnpay,
                (int)BuyOrderStatusEnum.StockInPay,
            }, list => list.OrderByDescending(p => p.StockInDate).ToList());
        }

        /// <summary>
        /// 导出备货单
        /// </summary>
        /// <param name=""></param>
        /// <param name="userModel"></param>
        /// <param name="saleOrderId"></param>
        /// <param name="msg"></param>
        /// <param name="bt"></param>
        /// <param name="saleOrder"></param>
        /// <returns></returns>
        public bool ExportStocking(UserModel userModel, Guid saleOrderId, out string msg, out byte[] bt, out BuyOrder saleOrder)
        {
            bt = null;

            var order = Da.Get<BuyOrder>(saleOrderId);
            saleOrder = order;
            var b1 = CheckAuthOrder(userModel, order, out msg);
            if (!b1)
            {
                return false;
            }

            var path = $"{Config.Root}/static/files/ExportStockIn.xlsx";
            if (!File.Exists(path))
            {
                msg = "配置错误";
                return false;
            }

            //产品
            var orderProducts = new UserBuyOrderBiz().GetBuyOrderDetailProduct(userModel, saleOrderId);
            var products = orderProducts.data;

            //统计
            var count = new UserBuyOrderBiz().GetBuyOrderDetailCount(userModel, order);

            bt = ExcelHelper.Export(path, wb =>
            {
                var sheet = wb.GetSheetAt(0);

                //
                sheet.GetRow(0).Cells[1].SetCellValue(order.BuyOrderNumber);
                sheet.GetRow(0).Cells[4].SetCellValue(order.UserNickName);
                //sheet.GetRow(1).Cells[1].SetCellValue(Convert.ToDateTime(order.ShipDate).ToString("yyyy/MM/dd"));
                sheet.GetRow(1).Cells[4].SetCellValue(order.DefaultShip);
                sheet.GetRow(2).Cells[1].SetCellValue(order.DefaultRemark);

                //
                for (var i = 0; i < products.Count - 1; i++)
                {
                    sheet.CopyRowSample(i + 10, i + 11);
                }

                var rs = products.Count;
                var index = 0;
                foreach (var item in products)
                {
                    var rows = sheet.GetRow(index + 10);
                    rows.Cells[0].SetCellValue(item.ProductSku);
                    rows.Cells[1].SetCellValue(item.CategoryName);
                    rows.Cells[2].SetCellValue(item.ProductName);
                    rows.Cells[3].SetCellValue(item.ProductSpecification);
                    rows.Cells[4].SetCellValue(item.ProductRemark);
                    rows.Cells[5].SetCellValue(item.Qty);
                    rows.Cells[6].SetCellValue(item.AreaPosition);
                    rows.Cells[7].SetCellValue(item.Remark);

                    index++;
                }

                //数量总计
                sheet.GetRow(11 + rs).Cells[5].SetCellFormula($"SUM(H11:H{rs + 10})");
                sheet.GetRow(11 + rs).Cells[5].SetCellValue(Convert.ToDouble(count.ProductQtys));
            });

            msg = null;
            return true;
        }

        /// <summary>
        /// 入库
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="request"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool StockIn(UserModel userModel, BuyOrder request, out string msg)
        {
            if (request == null || request.BuyOrderId == Guid.Empty)
            {
                msg = "参数错误";
                return false;
            }

            var order = Da.Get<BuyOrder>(request.BuyOrderId);
            var b1 = CheckAuthOrder(userModel, order, out msg);
            if (!b1)
            {
                return false;
            }

            var status = new List<int>()
            {
                (int)BuyOrderStatusEnum.Purchase,
                (int)BuyOrderStatusEnum.PurchaseUnpay,
                (int)BuyOrderStatusEnum.PurchasePay,
            };

            if (!status.Contains(order.Status))
            {
                msg = "操作失败";
                return false;
            }

            if (order.Status == (int)BuyOrderStatusEnum.Purchase || order.Status == (int)BuyOrderStatusEnum.PurchaseUnpay)
            {
                order.Status = (int)BuyOrderStatusEnum.StockInUnpay;
            }
            else if (order.Status == (int)BuyOrderStatusEnum.PurchasePay)
            {
                order.Status = (int)BuyOrderStatusEnum.StockInPay;
            }
            else
            {
                LogHelper.Fatal("其他状态不能操作出库");
                msg = "操作失败";
                return false;
            }

            //
            order.StockInQcUserName = request.StockInQcUserName;
            order.StockInRemark = request.StockInRemark;
            order.StockInUserName = userModel.UserNickName;
            order.StockInDate = DateTime.Now;

            return Da.StockIn(order);
        }

        /// <summary>
        /// 修改订单产品
        /// </summary>
        /// <returns></returns>
        public bool UpdateOrderProduct(UserModel userModel, BuyUpdateBuyOrderProductRequest request, out string msg)
        {
            var order = Da.Get<BuyOrder>(request.BuyOrderId);
            var b2 = CheckAuthOrderByStatusStockIn(userModel, order, out msg);
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

                if (op.InQty != item.InQty || op.Remark != item.Remark)
                {
                    op.InQty = item.InQty;
                    op.InStockRemark = item.InStockRemark;
                    updateOrderProducts.Add(op);
                }
            }

            if (updateOrderProducts.Count == 0)
            {
                return true;
            }

            return Da.UpdateBuyOrderProduct(updateOrderProducts);
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
            var b1 = CheckAuthOrderByStatusStockIn(userModel, order, out msg);
            if (!b1)
            {
                return false;
            }

            var products = Da.GetList<BuyOrderProduct>(new
            {
                order.BuyOrderId,
                IsNewAdd = true,
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
            var b1 = CheckAuthOrderByStatusStockIn(userModel, order, out msg);
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
                UserId = order.UserId,
                BuyOrderId = order.BuyOrderId,
                ProductId = product.ProductId,
                Price = 0,
                Qty = 0,
                InQty = request.InQty,
                Remark = null,
                CreateDate = DateTime.Now,
                IsStockIn = false,
                InStockRemark = request.InStockRemark,
                IsNewAdd = true,
            };

            Da.Add<Guid, BuyOrderProduct>(m);
            msg = null;
            return true;
        }

    }
}
