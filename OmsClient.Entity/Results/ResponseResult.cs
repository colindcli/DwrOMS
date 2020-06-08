using System.Collections.Generic;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Entity.Results
{
    /// <summary>
    /// 返回的数据统一接口
    /// </summary>
    public interface IResponseResult
    {
        /// <summary>
        /// 返回码
        /// </summary>
        int code { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        string msg { get; set; }
    }

    /// <summary>
    /// 响应结果
    /// </summary>
    public class ResponseResult<T> : IResponseResult
    {
        /// <summary>
        /// 返回码
        /// </summary>
        public int code { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// 返回的数据
        /// </summary>
        public T data { get; set; }
    }

    /// <summary>
    /// Layui响应结果
    /// </summary>
    public class PagenationResult<T> : IResponseResult
    {
        /// <summary>
        /// 返回码
        /// </summary>
        public int code { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int count { get; set; }
        /// <summary>
        /// 返回的数据
        /// </summary>
        public List<T> data { get; set; }
        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize { get; set; }

        public object obj { get; set; }
    }
}
