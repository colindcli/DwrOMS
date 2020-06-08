using OmsClient.Biz.Admins.Bases;
using OmsClient.Common;
using OmsClient.Entity;
using OmsClient.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Biz.Admins
{
    /// <summary>
    /// 
    /// </summary>
    public class AdminGenerateHtmlBiz : BaseAdminGenerateHtmlBiz
    {
        private static readonly object IsRunning = new object();
        private static readonly string Path = $"{Config.Root}/Files/generate.html";

        public void CreateGenerateHtml()
        {
            //生成首页
            Da.Add<Guid, GenerateHtml>(new GenerateHtml()
            {
                GenerateHtmlId = SeqGuid.NewGuid(),
                GenerateHtmlUrl = $"{Config.Domain}/Home/Index?ver={DateTime.Now.Ticks}&allow=true",
                GenerateHtmlPath = $"{AppDomain.CurrentDomain.BaseDirectory}index.html",
                IsGenerate = false,
                IsSuccess = false
            });

            Task.Run(() => GenerateHtml());
        }

        private void Write(string msg)
        {
            lock (Path)
            {
                File.AppendAllText(Path, msg + "<br />", Encoding.GetEncoding("utf-8"));
            }
        }

        /// <summary>
        /// 生成Html
        /// </summary>
        private void GenerateHtml()
        {
            lock (IsRunning)
            {
                Action<string> report = Write;

                //report.Invoke($"[{DateTime.Now:mm:ss}] 准备开始...");
                var flag = false;
                List<GenerateHtml> rows;
                while ((rows = Da.GetGenerateHtmls()).Count > 0)
                {
                    if (!flag)
                    {
                        if (File.Exists(Path))
                        {
                            lock (Path)
                            {
                                File.Delete(Path);
                            }
                        }
                    }

                    var model = Da.GetGenerateHtmlReport();
                    report.Invoke($"[{DateTime.Now:mm:ss}] 开始进度：{model.Finished}/{model.Total}");

                    flag = true;
                    var tasks = new List<Task>();
                    foreach (var row in rows)
                    {
                        var task = Generate(row);
                        tasks.Add(task);
                    }
                    Task.WaitAll(tasks.ToArray());

                    Da.BulkUpdate(rows, p => new { p.IsSuccess }, p => new { p.GenerateHtmlId });

                    report.Invoke($"[{DateTime.Now:mm:ss}] 完成进度：{model.Finished}/{model.Total}");

                    Thread.Sleep(100);
                }

                if (!flag)
                {
                    report.Invoke($"[{DateTime.Now:mm:ss}] 完成");
                    return;
                }

                var m = Da.GetGenerateHtmlReport();
                report.Invoke($"[{DateTime.Now:mm:ss}] 成功报告：{m.Success}/{m.Total}，失败：{m.Total - m.Success}");

                Da.BulkDeleteAll<GenerateHtml>();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        private Task Generate(GenerateHtml m)
        {
            return Task.Run(() =>
            {
                try
                {
                    var model = HttpService.GetRequestByUrl(m.GenerateHtmlUrl);
                    if (!model.IsSuccessful)
                    {
                        m.IsSuccess = false;
                        return;
                    }
                    
                    var path = m.GenerateHtmlPath;
                    var dir = System.IO.Path.GetDirectoryName(path);
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir ?? throw new InvalidOperationException());
                    }
                    File.WriteAllText(path, model.Content, Encoding.UTF8);

                    m.IsSuccess = File.Exists(path);
                }
                catch (Exception ex)
                {
                    Write($"[{DateTime.Now:mm:ss}] Url:{m.GenerateHtmlUrl}；{ex.Message}");
                    LogHelper.Fatal(ex.Message, ex);
                }
            });
        }
    }
}