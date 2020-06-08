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
    public abstract class BaseUserCategoryBiz
    {
        protected static readonly UserCategoryDa Da = new UserCategoryDa();

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool AddCategory(UserModel userModel, Category model)
        {
            model.CreateDate = DateTime.Now;
            Da.Add(model);
            return true;
        }

        /// <summary>
        /// 获取一条记录
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public virtual Category GetCategoryById(UserModel userModel, int categoryId)
        {
            var m = Da.Get<Category>(categoryId);
            if (m == null)
            {
                return null;
            }

            return m;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool UpdateCategory(UserModel userModel, Category model)
        {
            var m = Da.Get<Category>(model.CategoryId);
            if (m == null)
            {
                return false;
            }

            m.CategoryParentId = model.CategoryParentId;
            m.CategoryName = model.CategoryName;
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
        public virtual bool DeleteCategory(UserModel userModel, Category model, out string msg)
        {
            var cates = Da.GetList<Category>();

            var m = cates.Find(p => p.CategoryId == model.CategoryId);
            if (m == null)
            {
                msg = "分类不存在，操作失败";
                return false;
            }

            //子分类
            var childs = cates.Count(p => p.CategoryParentId == m.CategoryId);
            if (childs > 0)
            {
                msg = "请先删除子分类，操作失败";
                return false;
            }

            //判断产品有没有使用
            var list = Da.GetList<Product>(new
            {
                m.CategoryId,
            });
            if (list.Count > 0)
            {
                msg = $"分类关联了{list.Count}产品，解除关联后才能删除，操作失败";
                return false;
            }

            msg = null;
            return Da.Delete(m);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual PagenationResult<Category> GetCategoryList(UserModel userModel, int pageIndex, int pageSize, string keyword)
        {
            var list = GetCategoryList(userModel);
            if (!keyword.IsWhiteSpace())
            {
                var keys = keyword.ToKeys(StringComparer.OrdinalIgnoreCase).ToList();
                list = list.Where(p => $"{p.CategoryName}".HasSearchKeys(keys, StringComparison.OrdinalIgnoreCase, false)).ToList();
            }
            var result = new PagenationResult<Category>()
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
        public virtual List<Category> GetCategoryList(UserModel userModel)
        {
            return Da.GetList<Category>(new
            {
            }).OrderBy(p => p.Sort).ToList();
        }
    }
}
