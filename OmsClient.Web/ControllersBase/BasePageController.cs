using OmsClient.Entity.Results;
using OmsClient.Web.Filters;
using System.Text.RegularExpressions;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Web.ControllersBase
{
    /// <summary>
    /// 
    /// </summary>
    [ExecutePageFilter]
    public class BasePageController : BaseController
    {
        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex => Request.QueryString["page"] != null ? int.Parse(Request.QueryString["page"]) : 1;
        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize => Request.QueryString["pageSize"] != null ? int.Parse(Request.QueryString["pageSize"]) : 12;
        /// <summary>
        /// 
        /// </summary>
        public PageInfoResult PageInfo { get; set; } = new PageInfoResult();
        
    }
}