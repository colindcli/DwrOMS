using OmsClient.Common;
using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Web.Http.Filters;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Web.Filters
{
    /// <summary>
    /// API压缩
    /// </summary>
    public class CompressionApiFilter : ActionFilterAttribute
    {
        private const string Gzip = "gzip";
        private const string Deflate = "deflate";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="compress"></param>
        /// <returns></returns>
        private static byte[] Compress(byte[] data, Func<MemoryStream, Stream> compress)
        {
            if (data == null || data.Length < 1)
                return data;
            try
            {
                using (var stream = new MemoryStream())
                {
                    using (var gZipStream = compress(stream))
                    {
                        gZipStream.Write(data, 0, data.Length);
                        gZipStream.Close();
                    }

                    return stream.ToArray();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Fatal(ex.Message, ex);
                return data;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static byte[] DeflateCompress(byte[] data)
        {
            return Compress(data, ms => new DeflateStream(ms, CompressionMode.Compress));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static byte[] GzipCompress(byte[] data)
        {
            return Compress(data, ms => new GZipStream(ms, CompressionMode.Compress));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        /// <param name="compress"></param>
        /// <param name="name"></param>
        private static void Response(HttpActionExecutedContext actionExecutedContext, Func<byte[], byte[]> compress,
            string name)
        {
            var bytes = actionExecutedContext.Response.Content.ReadAsByteArrayAsync().Result;
            actionExecutedContext.Response.Content = new ByteArrayContent(compress(bytes));
            actionExecutedContext.Response.Content.Headers.Add("Content-Encoding", name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Response.Content == null ||
                actionExecutedContext.Request.Method == HttpMethod.Options)
            {
                base.OnActionExecuted(actionExecutedContext);
                return;
            }

            var acceptEncoding = actionExecutedContext.Request.Headers.AcceptEncoding.ToString();
            if (acceptEncoding.Contains(Gzip))
            {
                Response(actionExecutedContext, GzipCompress, Gzip);
            }
            else if (acceptEncoding.Contains(Deflate))
            {
                Response(actionExecutedContext, DeflateCompress, Deflate);
            }

            base.OnActionExecuted(actionExecutedContext);
        }
    }
}