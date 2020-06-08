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
    //[MenuApiFilter(MenuEnum.BuySupplier)]
    public class UserBuySupplierController : BaseUserApiController
    {
        private static readonly UserBuySupplierBiz Biz = new UserBuySupplierBiz();

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult AddBuySupplier(BuySupplier model)
        {
            var result = Biz.AddBuySupplier(UserModel, model);
            return ReturnResult(result);
        }

        /// <summary>
        /// 获取一条记录
        /// </summary>
        /// <param name="buySupplierId"></param>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetBuySupplierById(Guid buySupplierId)
        {
            var result = Biz.GetBuySupplierById(UserModel, buySupplierId);
            return ReturnResult(result);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult UpdateBuySupplier(BuySupplier model)
        {
            var result = Biz.UpdateBuySupplier(UserModel, model);
            return ReturnResult(result);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult DeleteBuySupplier(BuySupplier model)
        {
            var result = Biz.DeleteBuySupplier(UserModel, model);
            return ReturnResult(result);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetBuySupplierList(int page, int limit, string keyword = null)
        {
            return Biz.GetBuySupplierList(UserModel, page, limit, keyword);
        }

        /// <summary>
        /// 获取所有记录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetBuySupplierList()
        {
            var result = Biz.GetBuySupplierList(UserModel);
            return ReturnResult(result);
        }
    }
}
