using OmsClient.Biz.Users;
using OmsClient.Entity.Results;
using OmsClient.Web.ControllersBase;
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
    //[MenuApiFilter(MenuEnum.StockOut)]
    public class UserStockOutController : BaseUserApiController
    {
        private static readonly UserStockOutBiz Biz = new UserStockOutBiz();
        
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetStockOutList(int page, int limit, string keyword = null)
        {
            return Biz.GetStockOutList(UserModel, page, limit, keyword);
        }
    }
}
