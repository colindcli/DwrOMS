using OmsClient.Entity.Results;
using OmsClient.Web.ControllersBase;
using System;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using OmsClient.Common;
using OmsClient.Entity;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Web.ControllersApi
{
    /// <summary>
    /// 上传附件
    /// </summary>
    public class UserFileController : BaseUserApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type">0原图；1小缩略图；2大缩略图</param>
        /// <returns></returns>
        /// /clientApi/AdminFile/Get/Guid?type=0
        public HttpResponseMessage Get(Guid id, int type = 0)
        {
            return GetAttchement(id, type);
        }

        /// <summary>
        /// 上传附件
        /// api/UserFile/Uplad
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult Upload()
        {
            var files = HttpContext.Current.Request.Files;
            if (files.Count == 0)
            {
                return null;
            }

            var item = files[0];
            var tempPath = $"/Files/Temp/{Guid.NewGuid()}.jpg";
            item.SaveAs($"{Config.Root}{tempPath}");
            
            return ReturnResult(tempPath);
        }
    }
}
