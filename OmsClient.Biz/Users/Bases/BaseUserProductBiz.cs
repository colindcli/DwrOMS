using DwrUtility;
using DwrUtility.Strings;
using DwrUtility.Trees;
using OmsClient.DataAccess.Users;
using OmsClient.Entity;
using OmsClient.Entity.Enums;
using OmsClient.Entity.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Biz.Users.Bases
{
    public abstract class BaseUserProductBiz
    {
        protected static readonly UserProductDa Da = new UserProductDa();

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="model"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public virtual bool AddProduct(UserModel userModel, Product model, out string msg)
        {
            if (model == null)
            {
                msg = "参数错误";
                return false;
            }

            if (model.ProductSku.IsWhiteSpace())
            {
                msg = "编码不能为空";
                return false;
            }

            if (model.ProductName.IsWhiteSpace())
            {
                msg = "产品名称不能为空";
                return false;
            }

            if (model.Price1 < 0 || model.Price10 < 0 || model.Price100 < 0)
            {
                msg = "价格不能小于0";
                return false;
            }

            var cates = Da.Get<Category>(model.CategoryId);
            if (cates == null)
            {
                msg = "分类错误";
                return false;
            }

            //验证编码唯一性
            var list = Da.GetList<Product>(new
            {
                model.ProductSku,
                Status = (int)ProductStatusEnum.Ok,
            });
            if (list.Count > 0)
            {
                msg = "编码已存在";
                return false;
            }

            var m = new Product()
            {
                ProductId = SeqGuid.NewGuid(),
                CategoryId = model.CategoryId,
                ProductSku = model.ProductSku,
                ProductName = model.ProductName,
                ProductSpecification = model.ProductSpecification,
                ProductRemark = model.ProductRemark,
                ProductWeight = model.ProductWeight,
                InTransitQty = 0,
                SaleQty = 0,
                HoldQty = 0,
                OrderQty = model.OrderQty,
                Price1 = model.Price1,
                Price10 = model.Price10,
                Price100 = model.Price100,
                ImageQty = 0,
                AreaPosition = model.AreaPosition,
                Status = (int)ProductStatusEnum.Ok,
                CreateName = userModel.UserNickName,
                CreateDate = DateTime.Now,
                StockInDate = DateTime.Now,
            };

            Da.Add<Guid, Product>(m);
            msg = null;
            return true;
        }

        /// <summary>
        /// 获取一条记录
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public virtual Product GetProductById(UserModel userModel, Guid productId)
        {
            return Da.Get<Product>(productId);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="model"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public virtual bool UpdateProduct(UserModel userModel, Product model, out string msg)
        {
            if (model == null)
            {
                msg = "参数错误";
                return false;
            }

            if (model.ProductSku.IsWhiteSpace())
            {
                msg = "编码不能为空";
                return false;
            }

            if (model.ProductName.IsWhiteSpace())
            {
                msg = "产品名称不能为空";
                return false;
            }

            if (model.Price1 < 0 || model.Price10 < 0 || model.Price100 < 0)
            {
                msg = "价格不能小于0";
                return false;
            }

            var cates = Da.Get<Category>(model.CategoryId);
            if (cates == null)
            {
                msg = "分类错误";
                return false;
            }

            var m = Da.Get<Product>(model.ProductId);
            if (m == null)
            {
                msg = "产品不存在";
                return false;
            }

            //验证编码唯一性
            var list = Da.GetList<Product>(new
            {
                model.ProductSku,
                Status = (int)ProductStatusEnum.Ok,
            }).Where(p => p.ProductId != m.ProductId).ToList();
            if (list.Count > 0)
            {
                msg = "编码已存在";
                return false;
            }

            m.CategoryId = model.CategoryId;
            m.ProductSku = model.ProductSku;
            m.ProductName = model.ProductName;
            m.ProductSpecification = model.ProductSpecification;
            m.ProductRemark = model.ProductRemark;
            m.ProductWeight = model.ProductWeight;
            m.OrderQty = model.OrderQty;
            m.Price1 = model.Price1;
            m.Price10 = model.Price10;
            m.Price100 = model.Price100;
            m.AreaPosition = model.AreaPosition;

            msg = null;
            return Da.Update(m);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="model"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public virtual bool DeleteProduct(UserModel userModel, Product model, out string msg)
        {
            var m = Da.Get<Product>(model.ProductId);
            if (m == null)
            {
                msg = "删除失败";
                return false;
            }

            //检查销售单
            var saleProducts = Da.GetList<SaleOrderProduct>(new
            {
                model.ProductId,
            });
            if (saleProducts.Count > 0)
            {
                msg = "产品已下销售单，不能删除了";
                return false;
            }

            //检查采购单
            var buyProducts = Da.GetList<BuyOrderProduct>(new
            {
                model.ProductId
            });
            if (buyProducts.Count > 0)
            {
                msg = "产品已下采购单，不能删除了";
                return false;
            }

            m.Status = (int)ProductStatusEnum.Delete;
            msg = null;
            return Da.Update(m);
        }

        /// <summary>
        /// 获取列表 （按创建时间）
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="categoryId"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual PagenationResult<Product> GetProductList(UserModel userModel, int pageIndex, int pageSize,
            int? categoryId, string keyword)
        {
            return GetProductListResult(userModel, pageIndex, pageSize, categoryId, keyword,
                g => g.OrderByDescending(k => k.ProductId.ToSqlGuid()).ToList());
        }

        /// <summary>
        /// 获取列表 (按入库倒序)
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="categoryId"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual PagenationResult<Product> GetProductQueryList(UserModel userModel, int pageIndex, int pageSize,
            int? categoryId, string keyword)
        {
            return GetProductListResult(userModel, pageIndex, pageSize, categoryId, keyword,
                g => g.OrderByDescending(k => k.StockInDate).ToList());
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="categoryId"></param>
        /// <param name="keyword"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public virtual PagenationResult<Product> GetProductListResult(UserModel userModel, int pageIndex, int pageSize, int? categoryId, string keyword, Func<List<Product>, List<Product>> orderBy)
        {
            var currencys = Da.GetListAsync<Currency>();
            var cate = Da.GetListAsync<Category>();
            var list = GetProductList(userModel);

            //排序
            list = orderBy.Invoke(list);

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

            if (list.Count > 0)
            {
                list.ForEach(item =>
                {
                    var parentCates = cate.Result.GetParentNodes(item.CategoryId, p => p.CategoryId, p => p.CategoryParentId, true);
                    if (parentCates != null)
                    {
                        item.CategoryName = string.Join("|", parentCates.Select(p => p.CategoryName));
                    }
                });
            }

            Task.WaitAll(currencys);
            var result = new PagenationResult<Product>()
            {
                count = list.Count,
                data = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList(),
                obj = currencys.Result,
            };
            return result;
        }

        /// <summary>
        /// 获取所有记录
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        public virtual List<Product> GetProductList(UserModel userModel)
        {
            return Da.GetList<Product>(new
            {
                Status = (int)ProductStatusEnum.Ok
            });
        }
    }
}
