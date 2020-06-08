//var ip = Request?.ServerVariables?.Get("REMOTE_ADDR");
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Common.Utilitys
{
    /// <summary>
    /// IP拦截
    /// </summary>
    public class BlackIpHelper
    {
        private static readonly int Seconds = 60;
        private static readonly Regex RegSpider = new Regex("Baiduspider|Googlebot|360Spider|Sosospider|sogou|bingbot|Yahoo|YoudaoBot", RegexOptions.IgnoreCase);
        private static readonly List<Ips> BlackIpLists = new List<Ips>();
        private static readonly List<Ips> IpLists = new List<Ips>();

        /// <summary>
        /// 是否机器人IP
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="userAgent"></param>
        /// <returns></returns>
        public static bool IsRobotIp(string ip, string userAgent)
        {
            if (string.IsNullOrWhiteSpace(ip) || string.IsNullOrWhiteSpace(userAgent))
            {
                return true;
            }

            if (RegSpider.IsMatch(userAgent))
            {
                return false;
            }

            if (BlackIpLists.Exists(p => p.Ip.Equals(ip)))
            {
                return true;
            }

            var item = new Ips()
            {
                Ip = ip,
                Ticks = DateTime.Now.Ticks
            };
            var overLine = item.Ticks - Seconds * 10000000;

            Task.Run(() =>
            {
                lock (IpLists)
                {
                    IpLists.RemoveAll(p => p.Ticks < overLine);
                }
            });

            var total = 0;
            lock (IpLists)
            {
                IpLists.Add(item);
                total = IpLists.Count(p => p.Ip.Equals(ip) && p.Ticks > overLine);
            }

            if (total > Seconds)
            {
                BlackIpLists.Add(item);
                return true;
            }

            return false;
        }

        public class Ips
        {
            public string Ip { get; set; }
            public long Ticks { get; set; }
        }
    }
}
