using DwrUtility;
using OmsClient.Biz;
using OmsClient.Common;
using OmsClient.Entity;
using OmsClient.Entity.Enums;
using OmsClient.Entity.Results;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Web.ControllersBase
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseApiController : ApiController
    {
        /// <summary>
        /// 
        /// </summary>
        protected int PageIndex
        {
            get
            {
                var page = HttpContext.Current.Request["page"];
                return page == null ? 1 : int.Parse(page);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected int PageSize
        {
            get
            {
                var count = HttpContext.Current.Request["limit"];
                return count == null ? 1 : int.Parse(count);
            }
        }

        /// <summary>
        /// 脱敏
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        protected static void UserDesensitization(User m)
        {
            m.UserPwd = m.Code = m.SafeCode = m.Token = null;
            m.CreateDate = DateTime.Now;
        }

        /// <summary>
        /// 
        /// </summary>
        protected static readonly FileBiz BizFile = new FileBiz();

        /// <summary>
        /// 统一返回数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        protected IResponseResult ReturnResult<T>(T data, ReturnCode code = ReturnCode.Ok)
        {
            var result = new ResponseResult<T>
            {
                code = (int)code,
                data = data
            };
            if (code != ReturnCode.Ok)
            {
                result.msg = Config.DictReturnCode[result.code];
            }
            return result;
        }

        /// <summary>
        /// 统一返回数据
        /// </summary>
        /// <returns></returns>
        protected IResponseResult ReturnResult(bool flag, string msg)
        {
            if (flag)
            {
                return new ResponseResult<bool>()
                {
                    code = 0,
                    data = true,
                    msg = null
                };
            }

            var result = new ResponseResult<bool>
            {
                code = -100,
                data = false,
                msg = msg,
            };
            return result;
        }

        /// <summary>
        /// 统一返回错误消息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        protected IResponseResult ReturnMsg(string msg)
        {
            if (msg.IsWhiteSpace())
            {
                return new ResponseResult<bool>()
                {
                    code = 0,
                    data = false,
                };
            }

            var result = new ResponseResult<bool>
            {
                code = -100,
                data = false,
                msg = msg
            };
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        protected static FileUploadResult UploadFile(UserModel userModel)
        {
            var files = HttpContext.Current.Request.Files;
            if (files.Count == 0)
            {
                return null;
            }

            var item = files[0];
            var tempPath = $"/Files/Temp/{SeqGuid.NewGuid()}";
            item.SaveAs(tempPath);

            //判断是否是图片

            return null;
        }

        private static readonly string NoImagePath = $"{Config.Root}/App_Data/nopic.jpg";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type">0原图；1小缩略图；2大缩略图</param>
        /// <returns></returns>
        /// /clientApi/AdminFile/Get/Guid?type=0
        public static HttpResponseMessage GetAttchement(Guid id, int type = 0)
        {
            var m = BizFile.GetAttachment(id);

            if (m == null)
            {
                LogHelper.Warn($"AttachmentId不存在：{id}");
                return ResponseAttachment(NoImagePath, "数据库文件不存在");
            };

            var next = "";
            switch (type)
            {
                case 1:
                    {
                        next = "_small";
                        break;
                    }
                case 2:
                    {
                        next = "_big";
                        break;
                    }
            }

            var path = $"{Config.Root}/Files/{m.Folder}/{m.AttachmentId}{next}{m.FileExt}";
            if (!File.Exists(path))
            {
                LogHelper.Warn($"文件不存在：{path}");
                return ResponseAttachment(NoImagePath, "系统文件不存在");
            }

            return ResponseAttachment(path, m.Title);
        }

        /// <summary>
        /// 输出附件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        private static HttpResponseMessage ResponseAttachment(string fileName, string title)
        {
            var fs = new FileStream(fileName, FileMode.Open);
            var len = (int)fs.Length;
            var bt = new byte[len];
            fs.Read(bt, 0, len);
            fs.Close();
            fs.Dispose();

            var response = new HttpResponseMessage { Content = new ByteArrayContent(bt) };
            response.Content.Headers.ContentLength = bt.Length;
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = title,
                FileNameStar = title
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            response.Headers.CacheControl = new CacheControlHeaderValue()
            {
                Public = true,
                MaxAge = new TimeSpan(30, 0, 0, 0)
            };
            return response;
        }

        /// <summary>
        /// 输出附件
        /// </summary>
        /// <param name="bt"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static HttpResponseMessage ResponseAttachment(byte[] bt, string title)
        {
            var response = new HttpResponseMessage { Content = new ByteArrayContent(bt) };
            response.Content.Headers.ContentLength = bt.Length;
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = title,
                FileNameStar = title
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            response.Headers.CacheControl = new CacheControlHeaderValue()
            {
                Public = true,
                MaxAge = new TimeSpan(30, 0, 0, 0)
            };
            return response;
        }
    }
}
