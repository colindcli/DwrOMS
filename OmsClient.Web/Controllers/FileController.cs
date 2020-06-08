using OmsClient.Biz;
using OmsClient.Common;
using OmsClient.Web.ControllersBase;
using OmsClient.Web.Filters;
using System;
using System.IO;
using System.Web.Mvc;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Web.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class FileController : BaseController
    {
        private static readonly FileBiz Biz = new FileBiz();

        /// <summary>
        /// 附件
        /// </summary>
        /// <param name="id">AttachmentId</param>
        /// <returns></returns>
        // GET: File/Guid
        public ActionResult Index(Guid id)
        {
            var attachment = Biz.GetAttachment(id);
            if (attachment == null)
            {
                return new HttpNotFoundResult();
            }

            var path = $"{Config.Root}/Files/{attachment.Folder}/{attachment.AttachmentId}";
            if (!System.IO.File.Exists(path))
            {
                return new HttpNotFoundResult("服务器文件不存在！");
            }

            return File(path, "application/octet-stream");
        }

        private static byte[] NoImageByte { get; set; }

        private static byte[] NoImage
        {
            get
            {
                if (NoImageByte != null)
                {
                    return NoImageByte;
                }

                NoImageByte = GetBytes($"{Config.Root}/App_Data/nopic.jpg");

                return NoImageByte;
            }
        }

        /// <summary>
        /// 图片
        /// </summary>
        /// <param name="id">AttachmentId</param>
        /// <param name="type">0原图；1小缩略图；2大缩略图</param>
        /// <returns></returns>
        // GET: Img/Guid
        [ResponseCacheFilter()]
        public ActionResult Img(Guid id, int type = 0)
        {
            var attachment = Biz.GetAttachment(id);
            if (attachment == null)
            {
                return File(NoImage, "mage/jpeg");
            }

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
            var path = $"{Config.Root}/Files/{attachment.Folder}/{attachment.AttachmentId}{next}{attachment.FileExt}";
            if (!System.IO.File.Exists(path))
            {
                return File(NoImage, "mage/jpeg");
            }

            return File(GetBytes(path), "image/jpeg");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static byte[] GetBytes(string path)
        {
            lock (path)
            {
                var fs = System.IO.File.Open(path, FileMode.Open, FileAccess.Read);
                fs.Position = 0;
                var bt = new byte[fs.Length];
                fs.Read(bt, 0, bt.Length);
                fs.Close();
                fs.Dispose();
                return bt;
            }
        }
    }
}