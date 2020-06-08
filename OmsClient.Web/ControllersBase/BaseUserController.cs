using OmsClient.Entity;
using OmsClient.Web.Filters;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Web.ControllersBase
{
    /// <summary>
    /// 
    /// </summary>
    [AuthorizeFilter]
    public class BaseUserController : BaseController
    {
        /// <summary>
        /// 
        /// </summary>
        public UserModel UserModel { get; set; }
    }
}