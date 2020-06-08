using OmsClient.Biz.Users;
using OmsClient.Entity;
using OmsClient.Entity.Results;
using OmsClient.Web.ControllersBase;
using System;
using System.Web.Http;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Web.ControllersApi
{
    /// <summary>
    /// 
    /// </summary>
    //[MenuApiFilter(MenuEnum.Category)]
    public class UserCategoryController : BaseUserApiController
    {
        private static readonly UserCategoryBiz Biz = new UserCategoryBiz();

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult AddCategory(Category model)
        {
            var result = Biz.AddCategory(UserModel, model);
            return ReturnResult(result);
        }

        /// <summary>
        /// 获取一条记录
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetCategoryById(int categoryId)
        {
            var result = Biz.GetCategoryById(UserModel, categoryId);
            return ReturnResult(result);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult UpdateCategory(Category model)
        {
            var result = Biz.UpdateCategory(UserModel, model);
            return ReturnResult(result);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult DeleteCategory(Category model)
        {
            var result = Biz.DeleteCategory(UserModel, model, out var msg);
            return ReturnResult(result, msg);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetCategoryList(int page, int limit, string keyword = null)
        {
            return Biz.GetCategoryList(UserModel, page, limit, keyword);
        }

        /// <summary>
        /// 获取所有记录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetCategoryList()
        {
            var result = Biz.GetCategoryList(UserModel);
            return ReturnResult(result);
        }
    }
}
