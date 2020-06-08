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
    //[MenuApiFilter(MenuEnum.StockIn)]
    public class UserStockInController : BaseUserApiController
    {
        private static readonly UserStockInBiz Biz = new UserStockInBiz();

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetStockInList(int page, int limit, string keyword = null)
        {
            return Biz.GetStockInList(UserModel, page, limit, keyword);
        }
    }
}
