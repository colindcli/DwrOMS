using OmsClient.Biz;
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
    public class LoginController : BaseApiController
    {
        private static readonly UserMainBiz Biz = new UserMainBiz();

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult Check(User request)
        {
            var res = Biz.CheckLogin(request, out var msg);
            return ReturnResult(res, msg);
        }
    }
}
