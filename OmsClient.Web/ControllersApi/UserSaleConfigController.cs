using OmsClient.Biz.Users;
using OmsClient.Entity;
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
    //[MenuApiFilter(MenuEnum.SaleConfig)]
    public class UserSaleConfigController : BaseUserApiController
    {
        private static readonly UserSaleConfigBiz Biz = new UserSaleConfigBiz();

        /// <summary>
        /// 获取记录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetSaleConfig()
        {
            var result = Biz.GetSaleConfig(UserModel);
            return ReturnResult(result);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult UpdateSaleConfig(SaleConfig model)
        {
            var result = Biz.UpdateSaleConfig(UserModel, model);
            return ReturnResult(result);
        }
    }
}
