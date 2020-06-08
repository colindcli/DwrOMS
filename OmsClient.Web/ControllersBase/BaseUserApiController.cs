using System;
using OmsClient.Entity;
using OmsClient.Web.Filters;

#pragma warning disable 1591

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Web.ControllersBase
{
    /// <summary>
    /// 
    /// </summary>
    [AuthorizeApiFilter]
    public abstract class BaseUserApiController : BaseApiController
    {
        public UserModel UserModel { get; set; }
    }
}