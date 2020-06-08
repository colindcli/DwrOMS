using System;
using System.IO;
using System.IO.Compression;
using System.Web;
using System.Web.Mvc;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Web.Filters
{
    /// <summary>
    /// MVC压缩
    /// </summary>
    public class CompressionFilter : ActionFilterAttribute
    {
        private const string Gzip = "gzip";
        private const string Deflate = "deflate";

        private static void Compress(HttpResponseBase response, Func<Stream, Stream> compress, string acceptEncoding)
        {
            response.Filter = compress(response.Filter);
            response.Headers.Remove("Content-Encoding");
            response.AppendHeader("Content-Encoding", acceptEncoding);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static Stream DeflateCompress(Stream data)
        {
            return new DeflateStream(data, CompressionMode.Compress);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static Stream GzipCompress(Stream data)
        {
            return new GZipStream(data, CompressionMode.Compress);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            if (filterContext.Exception != null)
                return;

            var response = filterContext.HttpContext.Response;
            if (response.Filter is GZipStream || response.Filter is DeflateStream)
                return;

            var acceptEncoding = filterContext.HttpContext.Request.Headers["Accept-Encoding"];
            if (string.IsNullOrWhiteSpace(acceptEncoding))
                return;

            if (acceptEncoding.Contains(Gzip))
            {
                Compress(response, GzipCompress, Gzip);
            }
            else if (acceptEncoding.Contains(Deflate))
            {
                Compress(response, DeflateCompress, Deflate);
            }
			
			base.OnResultExecuted(filterContext);
        }
    }
}