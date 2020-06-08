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

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Biz.Warehouses
{
    /// <summary>
    /// 
    /// </summary>
    public class WarehouseSaleOrderBiz : BaseOrderBiz
    {
        private static readonly WarehouseSaleOrderDa Da = new WarehouseSaleOrderDa();

        /// <summary>
        /// 统计订单数量
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        public List<int> GetMyOrderNumber(UserModel userModel)
        {
            var list = new UserSaleOrderDa().GetSaleOrderNumberList();

            var result = new List<int>();

            //待备货列表
            var a = list.Count(p => new List<int>()
            {
                (int)SaleOrderStatusEnum.SubmitUnpay,
                (int)SaleOrderStatusEnum.SubmitAfterPay,
                (int)SaleOrderStatusEnum.SubmitPay,
            }.Contains(p.Status));
            result.Add(a);

            //已备货列表
            var b = list.Count(p => new List<int>()
            {
                (int)SaleOrderStatusEnum.StockedUnpay,
            }.Contains(p.Status));
            result.Add(b);

            //待发货列表
            var c = list.Count(p => new List<int>()
            {
                (int)SaleOrderStatusEnum.StockedAfterPay,
                (int)SaleOrderStatusEnum.StockedPay,
            }.Contains(p.Status));
            result.Add(c);

            //已发货列表
            var d = list.Count(p => new List<int>()
            {
                (int)SaleOrderStatusEnum.ShipedAfterPay,
                (int)SaleOrderStatusEnum.ShipedPay,
            }.Contains(p.Status));
            result.Add(d);

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
        /// <param name="orderBy"></param>
        /// <returns></returns>
        private static PagenationResult<SaleOrderListResult> GetListByStatus(UserModel userModel, int pageIndex, int pageSize, string keyword, List<int> status, Func<List<SaleOrder>, List<SaleOrder>> orderBy)
        {
            var list = Da.GetListByBulk<SaleOrder>(new
            {
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
        /// 待备货列表
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual PagenationResult<SaleOrderListResult> GetStockingOrder(UserModel userModel, int pageIndex, int pageSize, string keyword)
        {
            return GetListByStatus(userModel, pageIndex, pageSize, keyword, new List<int>()
            {
                (int)SaleOrderStatusEnum.SubmitUnpay,
                (int)SaleOrderStatusEnum.SubmitAfterPay,
                (int)SaleOrderStatusEnum.SubmitPay,
            }, list => list.OrderByDescending(p => p.PostDate).ToList());
        }

        /// <summary>
        /// 已备货列表
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual PagenationResult<SaleOrderListResult> GetStockedOrder(UserModel userModel, int pageIndex, int pageSize, string keyword)
        {
            return GetListByStatus(userModel, pageIndex, pageSize, keyword, new List<int>()
            {
                (int)SaleOrderStatusEnum.StockedUnpay,
            }, list => list.OrderByDescending(p => p.StockEnd).ToList());
        }

        /// <summary>
        /// 待发货列表
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual PagenationResult<SaleOrderListResult> GetShipmentOrder(UserModel userModel, int pageIndex, int pageSize, string keyword)
        {
            return GetListByStatus(userModel, pageIndex, pageSize, keyword, new List<int>()
            {
                (int)SaleOrderStatusEnum.StockedAfterPay,
                (int)SaleOrderStatusEnum.StockedPay,
            }, list => list.OrderByDescending(p => p.StockEnd).ToList());
        }

        /// <summary>
        /// 已发货列表
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual PagenationResult<SaleOrderListResult> GetFinishedOrder(UserModel userModel, int pageIndex, int pageSize, string keyword)
        {
            return GetListByStatus(userModel, pageIndex, pageSize, keyword, new List<int>()
            {
                (int)SaleOrderStatusEnum.ShipedAfterPay,
                (int)SaleOrderStatusEnum.ShipedPay,
            }, list => list.OrderByDescending(p => p.StockOutDate).ToList());
        }

        /// <summary>
        /// 获取备货信息
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="saleOrderId"></param>
        /// <param name="msg"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public virtual SaleOrderStockingResult GetStockingResult(UserModel userModel, Guid saleOrderId, out string msg, out SaleOrder order)
        {
            order = Da.Get<SaleOrder>(saleOrderId);
            var b1 = CheckAuthOrder(userModel, order, out msg);
            if (!b1)
            {
                return null;
            }

            return GetSaleOrderStockingResult(order);
        }

        /// <summary>
        /// 获取备货信息
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public static SaleOrderStockingResult GetSaleOrderStockingResult(SaleOrder order)
        {
            var isStart = false;
            string startInfo;
            if (order.StockStart.HasValue && !order.StockStartName.IsWhiteSpace())
            {
                startInfo = $"{order.StockStartName}({order.StockStart.Value:MM/dd HH:mm})";
                isStart = true;
            }
            else
            {
                startInfo = "";
            }

            var isEnd = false;
            string endInfo;
            if (order.StockEnd.HasValue && !order.StockEndName.IsWhiteSpace())
            {
                endInfo = $"{order.StockEndName}({order.StockEnd.Value:MM/dd HH:mm})";
                isEnd = true;
            }
            else
            {
                endInfo = "";
            }

            var showStartBtn = !isStart;
            var showEndBtn = isStart && !isEnd;

            var result = new SaleOrderStockingResult()
            {
                StockStartInfo = startInfo,
                StockEndInfo = endInfo,
                ShowStartBtn = showStartBtn,
                ShowEndBtn = showEndBtn,
            };
            return result;
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
        public bool ExportStocking(UserModel userModel, Guid saleOrderId, out string msg, out byte[] bt, out SaleOrder saleOrder)
        {
            bt = null;

            var order = Da.Get<SaleOrder>(saleOrderId);
            saleOrder = order;
            var b1 = CheckAuthOrder(userModel, order, out msg);
            if (!b1)
            {
                return false;
            }

            var path = $"{Config.Root}/static/files/ExportStocking.xlsx";
            if (!File.Exists(path))
            {
                msg = "配置错误";
                return false;
            }

            //产品
            var orderProducts = new UserSaleOrderBiz().GetSaleOrderDetailProduct(userModel, saleOrderId);
            var products = orderProducts.data;

            //统计
            var count = new UserSaleOrderBiz().GetSaleOrderDetailCount(userModel, order);

            bt = ExcelHelper.Export(path, wb =>
            {
                var sheet = wb.GetSheetAt(0);

                //
                sheet.GetRow(0).Cells[1].SetCellValue(order.SaleOrderNumber);
                sheet.GetRow(0).Cells[4].SetCellValue(order.UserNickName);
                sheet.GetRow(1).Cells[1].SetCellValue(Convert.ToDateTime(order.ShipDate).ToString("yyyy/MM/dd"));
                sheet.GetRow(1).Cells[4].SetCellValue(order.DefaultShip);
                sheet.GetRow(2).Cells[1].SetCellValue(order.DefaultRemark);

                sheet.GetRow(4).Cells[1].SetCellValue(order.ToConsignee);
                sheet.GetRow(4).Cells[4].SetCellValue(order.ToCompanyName);
                sheet.GetRow(5).Cells[1].SetCellValue(order.ToTelphone);
                sheet.GetRow(5).Cells[4].SetCellValue(order.ToZipcode);
                sheet.GetRow(6).Cells[1].SetCellValue(order.ToAddress);
                sheet.GetRow(6).Cells[1].SetCellValue(order.ToRemark);

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
        /// 开始备货
        /// </summary>
        /// <returns></returns>
        public bool StartStocking(UserModel userModel, Guid saleOrderId, out string msg)
        {
            var order = Da.Get<SaleOrder>(saleOrderId);
            var b1 = CheckAuthOrder(userModel, order, out msg);
            if (!b1)
            {
                return false;
            }

            var res = GetSaleOrderStockingResult(order);
            if (res == null)
            {
                return false;
            }

            if (!res.ShowStartBtn)
            {
                msg = "订单状态已改变，操作失败";
                return false;
            }

            if (order.Status != (int)SaleOrderStatusEnum.SubmitUnpay &&
                order.Status != (int)SaleOrderStatusEnum.SubmitAfterPay &&
                order.Status != (int)SaleOrderStatusEnum.SubmitPay)
            {
                msg = "订单状态已改变，操作失败";
                return false;
            }

            order.StockStart = DateTime.Now;
            order.StockStartName = userModel.UserNickName;

            Da.Update(order);
            return true;
        }

        /// <summary>
        /// 完成备货
        /// </summary>
        /// <returns></returns>
        public bool EndStocking(UserModel userModel, Guid saleOrderId, out string msg)
        {
            var order = Da.Get<SaleOrder>(saleOrderId);
            var b1 = CheckAuthOrder(userModel, order, out msg);
            if (!b1)
            {
                return false;
            }

            var res = GetSaleOrderStockingResult(order);
            if (res == null)
            {
                return false;
            }

            if (!res.ShowEndBtn)
            {
                msg = "订单状态已改变，操作失败";
                return false;
            }

            if (order.Status != (int)SaleOrderStatusEnum.SubmitUnpay &&
                order.Status != (int)SaleOrderStatusEnum.SubmitAfterPay &&
                order.Status != (int)SaleOrderStatusEnum.SubmitPay)
            {
                msg = "订单状态已改变，操作失败";
                return false;
            }

            order.StockEnd = DateTime.Now;
            order.StockEndName = userModel.UserNickName;

            //提交状态
            switch (order.Status)
            {
                case (int)SaleOrderStatusEnum.SubmitUnpay:
                    order.Status = (int)SaleOrderStatusEnum.StockedUnpay;
                    break;
                case (int)SaleOrderStatusEnum.SubmitAfterPay:
                    order.Status = (int)SaleOrderStatusEnum.StockedAfterPay;
                    break;
                case (int)SaleOrderStatusEnum.SubmitPay:
                    order.Status = (int)SaleOrderStatusEnum.StockedPay;
                    break;
                default:
                    throw new Exception("状态报错了");
            }

            Da.Update(order);
            return true;
        }

        /// <summary>
        /// 出库发货
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="request"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool StockOut(UserModel userModel, SaleOrder request, out string msg)
        {
            if (request == null || request.SaleOrderId == Guid.Empty)
            {
                msg = "参数错误";
                return false;
            }

            if (!request.StockShipDate.HasValue)
            {
                msg = "请输入发货时间，操作失败";
                return false;
            }

            var order = Da.Get<SaleOrder>(request.SaleOrderId);
            var b1 = CheckAuthOrder(userModel, order, out msg);
            if (!b1)
            {
                return false;
            }

            var status = new List<int>()
            {
                (int)SaleOrderStatusEnum.StockedAfterPay,
                (int)SaleOrderStatusEnum.StockedPay,
            };

            if (!status.Contains(order.Status))
            {
                msg = "操作失败";
                return false;
            }

            if (order.Status == (int)SaleOrderStatusEnum.StockedAfterPay)
            {
                order.Status = (int)SaleOrderStatusEnum.ShipedAfterPay;
            }
            else if (order.Status == (int) SaleOrderStatusEnum.StockedPay)
            {
                order.Status = (int) SaleOrderStatusEnum.ShipedPay;
            }
            else
            {
                LogHelper.Fatal("其他状态不能操作出库");
                msg = "操作失败";
                return false;
            }

            //
            order.StockShipDate = request.StockShipDate;
            order.StockRemark = request.StockRemark;
            order.StockOutDate = DateTime.Now;
            order.StockOutName = userModel.UserNickName;

            return Da.StockOut(order);
        }
    }
}
