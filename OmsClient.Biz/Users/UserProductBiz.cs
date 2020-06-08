using DwrExcel;
using DwrUtility;
using DwrUtility.Images;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using OmsClient.Biz.Users.Bases;
using OmsClient.Common;
using OmsClient.Entity;
using OmsClient.Entity.Enums;
using OmsClient.Entity.Results;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Biz.Users
{
    /// <summary>
    /// 
    /// </summary>
    public class UserProductBiz : BaseUserProductBiz
    {
        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="product"></param>
        /// <returns></returns>
        public List<ProductImage> GetProductImages(Guid productId, out Product product)
        {
            product = Da.Get<Product>(productId);
            if (product == null)
            {
                return new List<ProductImage>();
            }

            var list = Da.GetList<ProductImage>(new
            {
                product.ProductId,
            }).OrderBy(p => p.Sort).ToList();

            return list;
        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="productId"></param>
        /// <param name="fullPath"></param>
        /// <param name="path"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool UpdateImage(UserModel userModel, Guid productId, string fullPath, out string path, out string msg)
        {
            path = null;

            if (!File.Exists(fullPath))
            {
                msg = "上传文件不存在";
                return false;
            }

            var product = Da.Get<Product>(productId);
            if (product == null)
            {
                msg = "产品不存在";
                return false;
            }

            //给图片添加编码
            var pathText = $"/Files/{DateTime.Today:yyMMdd}/{Guid.NewGuid()}.jpg";
            var pathLogo = $"/Files/{DateTime.Today:yyMMdd}/{Guid.NewGuid()}.jpg";
            var fullPathText = $"{Config.Root}{pathText}";
            var fullPathLogo = $"{Config.Root}{pathLogo}";
            fullPathText.CreateDirByFilePath();

            var b1 = ImageHelper.AddImageText(fullPath, fullPathText, product.ProductSku, ImagePosition.BottomRigth, 0.5);
            if (!b1)
            {
                msg = "给图片打编码失败";
                return false;
            }

            if (!File.Exists(fullPathText))
            {

                msg = "上传失败";
                return false;
            }
            if (!File.Exists(fullPathLogo))
            {
                pathLogo = null;
            }

            var m = new ProductImage()
            {
                ProductImageId = SeqGuid.NewGuid(),
                ProductId = productId,
                PathImage = pathText,
                PathLogoImage = pathLogo,
                Sort = 999,
                CreateDate = DateTime.Now,
            };

            //添加并更新数量
            var b3 = Da.AddProductImage(m);
            if (!b3)
            {
                msg = "更新失败";
                return false;
            }

            //排序
            var list = Da.GetList<ProductImage>(new
            {
                ProductId = productId,
            }).OrderBy(p => p.Sort).ThenBy(p => p.CreateDate).ToList();
            for (var i = 0; i < list.Count; i++)
            {
                list[i].Sort = i;
            }
            Da.BulkUpdate(list, p => p.Sort, p => p.ProductImageId);

            msg = null;
            path = pathText;
            return true;
        }

        /// <summary>
        /// 产品图片排序
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="model"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool ImageSort(UserModel userModel, Product model, out string msg)
        {
            var product = Da.Get<Product>(model.ProductId);
            if (product == null)
            {
                msg = "产品不存在";
                return false;
            }

            var list = Da.GetList<ProductImage>(new
            {
                model.ProductId
            });

            var items = (from path in model.Paths
                         join pi in list on path equals pi.PathImage into ts
                         where !ts.Any()
                         select path).ToList();
            if (items.Count > 0)
            {
                msg = "图片已经发生变化，请刷新后再试";
                return false;
            }

            //排序
            for (var i = 0; i < model.Paths.Count; i++)
            {
                var obj = list.Find(p => p.PathImage.IsEquals(model.Paths[i]));
                obj.Sort = i;
            }

            //更新排序
            Da.BulkUpdate(list, p => p.Sort, p => p.ProductImageId);

            msg = null;
            return true;
        }

        /// <summary>
        /// 删除产品图片
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="model"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool DeleteProductImage(UserModel userModel, ProductImage model, out string msg)
        {
            var list = Da.GetList<ProductImage>(new
            {
                model.ProductId,
            }).OrderBy(p => p.Sort).ToList();

            var obj = list.Find(p => p.PathImage.IsEquals(model.PathImage));
            if (obj == null)
            {
                msg = "图片不存在";
                return false;
            }

            //删除文件
            var fullPathText = $"{Config.Root}{obj.PathImage}";
            var fullPathLogo = $"{Config.Root}{obj.PathLogoImage}";
            if (File.Exists(fullPathText))
            {
                File.Delete(fullPathText);
            }
            if (File.Exists(fullPathLogo))
            {
                File.Delete(fullPathLogo);
            }

            //删除并更新数量
            Da.DeleteProductImage(obj);

            //重新排序
            list.Remove(obj);
            msg = null;
            if (list.Count == 0)
            {
                return true;
            }

            for (var index = 0; index < list.Count; index++)
            {
                list[index].Sort = index;
            }

            Da.BulkUpdate(list, p => p.Sort, p => p.ProductImageId);
            return true;
        }

        /// <summary>
        /// 获取在途数量
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public List<InTransitQtyResult> GetInTransitQty(UserModel userModel, Guid productId)
        {
            var list = Da.GetList<BuyOrderProduct>(new
            {
                ProductId = productId,
                IsStockIn = false,
            });

            var orders = Da.GetListByBulk<BuyOrder>(new
            {
                BuyOrderId = list.Select(p => p.BuyOrderId).Distinct().ToList()
            });

            var rows = new List<InTransitQtyResult>();
            foreach (var item in list)
            {
                var order = orders.Find(p => p.BuyOrderId == item.BuyOrderId);
                if (order == null)
                {
                    continue;
                }

                var m = new InTransitQtyResult()
                {
                    BuyOrderId = order.BuyOrderId,
                    OrderNumber = order.BuyOrderNumber,
                    NickName = order.UserNickName,
                    Qty = item.Qty,
                    ArrivalsDate = order.ArrivalsDate?.ToString("MM/dd") ?? "",
                };
                switch ((BuyOrderStatusEnum)order.Status)
                {
                    case BuyOrderStatusEnum.Draft:
                        {
                            m.StatusName = "草稿";
                            break;
                        }
                    case BuyOrderStatusEnum.SubmitCheck:
                        {
                            m.StatusName = "审核中";
                            break;
                        }
                    case BuyOrderStatusEnum.SubmitBack:
                        {
                            m.StatusName = "驳回";
                            break;
                        }
                    case BuyOrderStatusEnum.SubmitPass:
                        {
                            m.StatusName = "待提交";
                            break;
                        }
                    case BuyOrderStatusEnum.Purchase:
                    case BuyOrderStatusEnum.PurchaseUnpay:
                    case BuyOrderStatusEnum.PurchasePay:
                        {
                            m.StatusName = "在途";
                            break;
                        }
                    default:
                        {
                            m.StatusName = "-";
                            break;
                        }
                }

                rows.Add(m);
            }

            //临时数据
            return rows;
        }

        /// <summary>
        /// 获取挂起数量
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public List<HoldQtyResult> GetHoldQty(UserModel userModel, Guid productId)
        {
            var list = Da.GetList<SaleOrderProduct>(new
            {
                ProductId = productId,
                IsStockOut = false,
            }).OrderByDescending(p => p.CreateDate).ToList();

            if (list.Count == 0)
            {
                return new List<HoldQtyResult>();
            }

            var orders = Da.GetListByBulk<SaleOrder>(new
            {
                SaleOrderId = list.Select(p => p.SaleOrderId).Distinct().ToList()
            });

            var rows = new List<HoldQtyResult>();
            foreach (var item in list)
            {
                var order = orders.Find(p => p.SaleOrderId == item.SaleOrderId);
                if (order == null)
                {
                    continue;
                }

                var m = new HoldQtyResult()
                {
                    SaleOrderId = order.SaleOrderId,
                    OrderNumber = order.SaleOrderNumber,
                    NickName = order.UserNickName,
                    Qty = item.Qty,
                };
                switch ((SaleOrderStatusEnum)order.Status)
                {
                    case SaleOrderStatusEnum.Draft:
                        {
                            m.StatusName = "草稿";
                            break;
                        }
                    case SaleOrderStatusEnum.SubmitUnpay:
                    case SaleOrderStatusEnum.SubmitAfterPay:
                    case SaleOrderStatusEnum.SubmitPay:
                        {
                            m.StatusName = "未备货";
                            break;
                        }
                    case SaleOrderStatusEnum.StockedUnpay:
                    case SaleOrderStatusEnum.StockedAfterPay:
                    case SaleOrderStatusEnum.StockedPay:
                        {
                            m.StatusName = "已备货";
                            break;
                        }
                    default:
                        {
                            m.StatusName = "-";
                            break;
                        }
                }

                //
                m.Days = (DateTime.Now - item.CreateDate).Days;

                rows.Add(m);
            }

            return rows;
        }

        /// <summary>
        /// 获取产品图片
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public virtual List<string> GetProductImageList(UserModel userModel, Guid productId)
        {
            var list = Da.GetList<ProductImage>(new
            {
                ProductId = productId
            });

            return list.OrderBy(p => p.Sort).Select(p => p.PathImage).ToList();
        }

        /// <summary>
        /// 导入产品
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="categoryId"></param>
        /// <param name="fullPath"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool ImportProduct(UserModel userModel, int categoryId, string fullPath, out string msg)
        {
            if (categoryId == 0)
            {
                msg = "分类错误";
                return false;
            }

            if (!File.Exists(fullPath))
            {
                msg = "上传失败";
                return false;
            }

            var cates = Da.Get<Category>(categoryId);
            if (cates == null)
            {
                msg = "分类错误";
                return false;
            }

            var rows = ExcelHelper.ReadSheetColumn26(fullPath);

            var list = new List<Product>();
            var errors = new List<string>();
            for (var i = 0; i < rows.Count; i++)
            {
                if (i == 0)
                {
                    //首行标题
                    continue;
                }

                var model = rows[i];
                var sku = (model.A ?? "").ToString();
                var name = (model.B ?? "").ToString();
                if (sku.IsWhiteSpace())
                {
                    errors.Add($"第{i + 1}行的编码为空");
                }
                if (name.IsWhiteSpace())
                {
                    errors.Add($"第{i + 1}行的名称为空");
                }

                //允许空
                var specification = model.C?.ToString();
                var remark = model.D?.ToString();

                //重量
                if (!int.TryParse(model.H?.ToString() ?? "", out var weight))
                {
                    errors.Add($"第{i + 1}行的重量数据类型错误");
                }
                //起订量
                if (!int.TryParse(model.I?.ToString() ?? "", out var orderQty))
                {
                    errors.Add($"第{i + 1}行的起订量数据类型错误");
                }
                //价格
                if (!decimal.TryParse(model.E?.ToString() ?? "", out var p1))
                {
                    errors.Add($"第{i + 1}行的价格1数据类型错误");
                }
                if (!decimal.TryParse(model.F?.ToString() ?? "", out var p10))
                {
                    errors.Add($"第{i + 1}行的价格10数据类型错误");
                }
                if (!decimal.TryParse(model.G?.ToString() ?? "", out var p100))
                {
                    errors.Add($"第{i + 1}行的价格100数据类型错误");
                }

                //允许空
                var areaPosition = model.J?.ToString();

                var m = new Product()
                {
                    ProductId = SeqGuid.NewGuid(),
                    CategoryId = categoryId,
                    ProductSku = sku,
                    ProductName = name,
                    ProductSpecification = specification,
                    ProductRemark = remark,
                    ProductWeight = weight,
                    InTransitQty = 0,
                    SaleQty = 0,
                    HoldQty = 0,
                    OrderQty = orderQty,
                    Price1 = p1,
                    Price10 = p10,
                    Price100 = p100,
                    ImageQty = 0,
                    AreaPosition = areaPosition,
                    Status = (int)ProductStatusEnum.Ok,
                    CreateName = userModel.UserNickName,
                    CreateDate = DateTime.Now,
                    StockInDate = DateTime.Now,
                };

                list.Add(m);
            }

            if (errors.Count > 0)
            {
                msg = string.Join(";", errors);
                return false;
            }

            //检查列表编码重复
            var repeats = list.GroupBy(p => p.ProductSku, (m, items) => new { m, items })
                .Where(p => p.items.Count() > 1)
                .Select(p => p.m)
                .ToList();
            if (repeats.Count > 0)
            {
                msg = $"编码重复：{string.Join(";", repeats)}";
                return false;
            }

            //检查编码是否相同
            var rs = Da.GetList<Product>(new
            {
                Status = (int)ProductStatusEnum.Ok
            }).ToList();

            var skus = new List<string>();
            foreach (var item in list)
            {
                if (rs.Exists(p => p.ProductSku.IsEquals(item.ProductSku)))
                {
                    skus.Add(item.ProductSku);
                }
            }
            if (skus.Count > 0)
            {
                msg = $"编码已存在: {string.Join(";", skus)}";
                return false;
            }

            msg = null;
            return Da.ImportProduct(list);
        }

        /// <summary>
        /// 导出产品
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="categoryId"></param>
        /// <param name="keyword"></param>
        /// <param name="msg"></param>
        /// <param name="bt"></param>
        /// <returns></returns>
        public bool ExportProduct(UserModel userModel, int? categoryId, string keyword, out string msg, out byte[] bt)
        {
            bt = null;
            var path = $"{Config.Root}/static/files/ExportProduct.xlsx";
            if (!File.Exists(path))
            {
                msg = "配置错误";
                return false;
            }

            var result = GetProductList(userModel, 1, int.MaxValue, categoryId, keyword);
            var list = result.data;

            bt = ExcelHelper.Export(path, wb =>
            {
                var sheet = wb.GetSheetAt(0);

                for (var i = 0; i < list.Count - 1; i++)
                {
                    sheet.CopyRowSample(i + 1, i + 2);
                }

                for (var i = 0; i < list.Count; i++)
                {
                    var item = list[i];
                    var row = sheet.GetRow(i + 1);

                    //赋值
                    row.Cells[0].SetCellValue(item.CategoryName);
                    row.Cells[1].SetCellValue(item.ProductSku);
                    row.Cells[2].SetCellValue(item.ProductName);
                    row.Cells[3].SetCellValue(item.ProductSpecification);
                    row.Cells[4].SetCellValue(item.ProductRemark);
                    row.Cells[5].SetCellValue(item.ProductWeight);
                    row.Cells[6].SetCellValue(item.OrderQty);
                    // ReSharper disable once SpecifyACultureInStringConversionExplicitly
                    row.Cells[7].SetCellValue(Convert.ToDouble(item.Price1));
                    // ReSharper disable once SpecifyACultureInStringConversionExplicitly
                    row.Cells[8].SetCellValue(Convert.ToDouble(item.Price10));
                    // ReSharper disable once SpecifyACultureInStringConversionExplicitly
                    row.Cells[9].SetCellValue(Convert.ToDouble(item.Price100));
                    if (item.ImageQty == 0)
                    {
                        row.Cells[10].SetCellValue("");
                    }
                    else
                    {
                        row.Cells[10].SetCellValue("Picture");
                        var hssfHyperlink = new XSSFHyperlink(HyperlinkType.Url)
                        {
                            Address = $"{Config.WebUrl}/ProductImage?id={item.ProductId}",
                        };
                        row.Cells[10].Hyperlink = hssfHyperlink;
                    }
                }
            });

            msg = null;
            return true;
        }
    }
}