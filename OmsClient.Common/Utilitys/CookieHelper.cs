using System;
using System.Web;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Common.Utilitys
{
    public class CookieHelper
    {
        /// <summary>
        /// 设置Cookie值
        /// </summary>
        /// <param name="cookieName">Cookie名称</param>
        /// <param name="cookieValue">Cookie值</param>
        /// <param name="expires">过期时间</param>
        public static void SetCookie(string cookieName, string cookieValue, DateTime expires)
        {
            var cookie = new HttpCookie(cookieName, cookieValue)
            {
                Value = cookieValue,
                Expires = expires
            };
            HttpContext.Current.Response.SetCookie(cookie);
        }

        /// <summary>
        /// 获取Cookie值
        /// </summary>
        /// <param name="cookieName">Cookie名称</param>
        /// <returns>无值返回null</returns>
        public static string GetCookie(string cookieName)
        {
            var cookie = HttpContext.Current.Request.Cookies[cookieName];
            return cookie?.Value;
        }

        /// <summary>
        /// 清除Cookie
        /// </summary>
        /// <param name="cookieName">Cookie名称</param>
        public static void ClearCookie(string cookieName)
        {
            var responseCookie = HttpContext.Current.Response.Cookies[cookieName];
            if (responseCookie != null)
                responseCookie.Expires = DateTime.Now.AddYears(-1000);
        }
    }

}
