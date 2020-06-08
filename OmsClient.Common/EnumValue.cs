using DwrUtility;
using OmsClient.Entity.Models;
using System.Collections.Generic;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Common
{
    public class EnumValue
    {
        /// <summary>
        /// 获取枚举值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<EnumModel> GetEnumModels<T>() where T : struct
        {
            var valueNames = EnumHelper.GetValueNameDict<T>();
            var valueDesc = EnumHelper.GetValueDescriptionDict<T>();

            var list = new List<EnumModel>();
            foreach (var item in valueNames)
            {
                valueDesc.TryGetValue(item.Key, out var desc);
                var m = new EnumModel()
                {
                    Key = item.Key,
                    Name = item.Value,
                    Description = desc
                };
                list.Add(m);
            }

            return list;
        }
    }
}
