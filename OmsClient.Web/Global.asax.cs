using DwrUtility;
using DwrUtility.CreanFiles;
using Newtonsoft.Json;
using OmsClient.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.SessionState;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Web
{
    /// <summary>
    /// 
    /// </summary>
    public class WebApiApplication : HttpApplication
    {
        /// <summary>
        /// 
        /// </summary>
        protected void Application_Start()
        {
            //log4net
            var fileInfo = new FileInfo(Config.Root + "/Log4net.config");
            log4net.Config.XmlConfigurator.Configure(fileInfo);

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            HttpFilterConfig.RegisterGlobalFilters(GlobalConfiguration.Configuration.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var settings = GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings;
            settings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
            settings.Culture = new CultureInfo("zh-CN");
            
            // json
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();

            //
            $"{Config.Root}/Files/Temp/".CreateDir();
            $"{Config.Root}/Logs/".CreateDir();

            //只读权限
            DirectoryHelper.AddDirectoryReadAuth($"{Config.Root}/Files");

            //清理日志
            CreanFileHelper.Start(new CreanFileParam()
            {
                FileTimes = new List<FileTime>()
                {
                    new FileTime()
                    {
                        DeleteTime = TimeSpan.FromDays(30),
                        Directories = new List<string>()
                        {
                            $"{AppDomain.CurrentDomain.BaseDirectory}Logs"
                        }
                    },
                    new FileTime()
                    {
                        DeleteTime = TimeSpan.FromDays(1),
                        Directories = new List<string>()
                        {
                            $"{AppDomain.CurrentDomain.BaseDirectory}Files/Temp"
                        }
                    },
                },
                Log = ex => LogHelper.Fatal(ex.Message, ex),
                DueTime = TimeSpan.FromMinutes(1),
                Period = TimeSpan.FromHours(6)
            });
        }
    }
}
