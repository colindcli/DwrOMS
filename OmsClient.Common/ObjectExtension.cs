using System;
using System.Collections.Generic;
using System.Linq;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Common
{
    public static class ObjectExtension
    {
        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static List<string> ToSplit(this string str, char c)
        {
            return string.IsNullOrWhiteSpace(str) ? new List<string>() : str.Split(new[] { c }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }
    }
}
