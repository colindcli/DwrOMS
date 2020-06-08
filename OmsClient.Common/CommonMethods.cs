using DwrUtility;
using OmsClient.Entity;
using OmsClient.Entity.Models;
using OmsClient.Entity.Results;
using System;
using System.Collections.Generic;
using System.Linq;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Common
{
    public class CommonMethods
    {
        /// <summary>
        /// 输出日期
        /// </summary>
        /// <param name="type">类型：1按月；2按周；3按日</param>
        /// <returns></returns>
        public static List<SaleChartDate> GetDateType(int type)
        {
            var list = new List<SaleChartDate>();
            var today = DateTime.Today;
            if (type == 1)
            {
                list.AddRange(GetDateTypeMonths(today));
            }
            else if (type == 2)
            {
                list.AddRange(GetDateTypeWeeks(today));
            }
            else if (type == 3)
            {
                list.AddRange(GetDateTypeDays(today));
            }
            return list;
        }

        /// <summary>
        /// 按月
        /// </summary>
        /// <returns></returns>
        public static List<SaleChartDate> GetDateTypeMonths(DateTime today)
        {
            var list = new List<SaleChartDate>();

            var monthOfFirstDay = Convert.ToDateTime(today.ToString("yyyy-MM-01"));

            SaleChartDateMonth(list, monthOfFirstDay.AddMonths(-6));
            SaleChartDateMonth(list, monthOfFirstDay.AddMonths(-5));
            SaleChartDateMonth(list, monthOfFirstDay.AddMonths(-4));
            SaleChartDateMonth(list, monthOfFirstDay.AddMonths(-3));
            SaleChartDateMonth(list, monthOfFirstDay.AddMonths(-2));
            SaleChartDateMonth(list, monthOfFirstDay.AddMonths(-1));
            SaleChartDateMonth(list, monthOfFirstDay);

            return list;
        }

        /// <summary>
        /// 按周
        /// </summary>
        /// <returns></returns>
        public static List<SaleChartDate> GetDateTypeWeeks(DateTime today)
        {
            var list = new List<SaleChartDate>();
            var week = (int)today.DayOfWeek;

            SaleChartDateWeek(list, today.AddDays(-41 - week));
            SaleChartDateWeek(list, today.AddDays(-34 - week));
            SaleChartDateWeek(list, today.AddDays(-27 - week));
            SaleChartDateWeek(list, today.AddDays(-20 - week));
            SaleChartDateWeek(list, today.AddDays(-13 - week));
            SaleChartDateWeek(list, today.AddDays(-6 - week));
            SaleChartDateWeek(list, today.AddDays(1 - week));

            return list;
        }

        /// <summary>
        /// 按天
        /// </summary>
        /// <returns></returns>
        public static List<SaleChartDate> GetDateTypeDays(DateTime today)
        {
            var list = new List<SaleChartDate>();
            var week = (int)today.DayOfWeek;
            SaleChartDate(list, today.AddDays(1 - week), "周一");
            SaleChartDate(list, today.AddDays(2 - week), "周二");
            SaleChartDate(list, today.AddDays(3 - week), "周三");
            SaleChartDate(list, today.AddDays(4 - week), "周四");
            SaleChartDate(list, today.AddDays(5 - week), "周五");
            SaleChartDate(list, today.AddDays(6 - week), "周六");
            SaleChartDate(list, today.AddDays(7 - week), "周日");
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <param name="name"></param>
        private static void SaleChartDate(List<SaleChartDate> list, DateTime begin, string name)
        {
            var index = list.Count;
            var m = new SaleChartDate()
            {
                Index = index,
                Begin = begin,
                End = begin.AddDays(1).AddSeconds(-1),
                Name = $"{begin:MM/dd}({name})",
            };
            m.EndNextDay = m.End.Date.AddDays(1);
            list.Add(m);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="begin"></param>
        private static void SaleChartDateWeek(List<SaleChartDate> list, DateTime begin)
        {
            var index = list.Count;
            var m = new SaleChartDate()
            {
                Index = index,
                Begin = begin,
                End = begin.AddDays(7).AddSeconds(-1),
            };
            m.EndNextDay = m.End.Date.AddDays(1);
            m.Name = $"{begin:MM/dd}-{m.End:MM/dd}";
            list.Add(m);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="begin"></param>
        private static void SaleChartDateMonth(List<SaleChartDate> list, DateTime begin)
        {
            var index = list.Count;
            var m = new SaleChartDate()
            {
                Index = index,
                Begin = begin,
                End = begin.AddMonths(1).AddSeconds(-1),
                Name = $"{begin:yyyy年MM月}",
            };
            m.EndNextDay = m.End.Date.AddDays(1);
            list.Add(m);
        }

        /// <summary>
        /// 输出属性KeyValue
        /// </summary>
        /// <param name="attributeString">A:a1;a2|B:b1;b2</param>
        /// <param name="selectedString">选择属性(A:a1|B:b1)</param>
        /// <returns></returns>
        public static List<AttributeKeyValueModel> GetAttributeKeyValues(string attributeString, string selectedString = null)
        {
            if (string.IsNullOrWhiteSpace(attributeString))
            {
                return null;
            }

            var attributes = attributeString.ToSplit('|');
            if (attributes.Count == 0)
            {
                return null;
            }

            //选中值
            var hasSelectValue = !string.IsNullOrWhiteSpace(selectedString);
            List<AttributeKeyValueModel> selectKeyValues = null;
            if (hasSelectValue)
            {
                selectKeyValues = GetAttributeKeyValues(selectedString);
            }

            var list = new List<AttributeKeyValueModel>();
            foreach (var attribute in attributes)
            {
                var keyValue = attribute.ToSplit(':');
                if (keyValue.Count != 2)
                {
                    continue;
                }

                var key = (keyValue[0] ?? "").Trim();
                var keyValues = (keyValue[1] ?? "").ToSplit(';')
                    .Select(p => new AttributeKeyValueSelectedModel()
                    {
                        Value = p,
                        IsSelected = false
                    }).ToList();

                //设置选中值
                if (hasSelectValue)
                {
                    var selectValues = selectKeyValues.Find(p => p.Name.IsEquals(key)).Values;
                    if (selectValues != null)
                    {
                        foreach (var item in keyValues)
                        {
                            item.IsSelected = selectValues.Exists(p => p.Value.IsEquals(item.Value));
                        }
                    }
                }

                list.Add(new AttributeKeyValueModel()
                {
                    Name = key,
                    Values = keyValues
                });
            }
            return list;
        }

        public static string GetUserName(User model)
        {
            if (!string.IsNullOrWhiteSpace(model.UserChnName))
            {
                return model.UserChnName;
            }

            return model.UserEmail?.Split('@')[0];
        }

        /// <summary>
        /// 生成订单号
        /// </summary>
        /// <param name="orderNumbers">已经存在的订单号</param>
        /// <param name="today"></param>
        /// <returns></returns>
        public static string GetOrderNumber(List<int> orderNumbers, DateTime today)
        {
            today = today.Date;
            if (orderNumbers.Count == 0)
            {
                return $"{today:yyMMdd}001";
            }

            var number = orderNumbers.Max() + 1;
            return number.ToString();
        }

        /// <summary>
        /// 生成订单号
        /// </summary>
        /// <param name="orderNumbers">已经存在的订单号</param>
        /// <param name="today"></param>
        /// <returns></returns>
        public static string GetBuyOrderNumber(List<string> orderNumbers, DateTime today)
        {
            today = today.Date;
            if (orderNumbers.Count == 0)
            {
                return $"B{today:yyMMdd}001";
            }

            var number = orderNumbers.Select(p => p.Substring(1, p.Length - 1).ToInt()).Max() + 1;
            return $"B{number}";
        }
    }
}
