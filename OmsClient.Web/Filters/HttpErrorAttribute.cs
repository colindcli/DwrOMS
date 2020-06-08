using OmsClient.Common;
using OmsClient.Entity.Enums;
using OmsClient.Entity.Results;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http.Filters;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Web.Filters
{
    /// <summary>
    /// Http Error Handle
    /// </summary>
    public class HttpErrorAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            LogHelper.Fatal(actionExecutedContext.Exception.Message, actionExecutedContext.Exception);

            var result = new ResponseResult<string>()
            {
                code = (int)ReturnCode.SystemError,
                msg = actionExecutedContext.Exception.Message,
                data = actionExecutedContext.Exception.ToString()
            };
            actionExecutedContext.Response = new HttpResponseMessage()
            {
                Content = new ObjectContent<IResponseResult>(result, new JsonMediaTypeFormatter(), "application/json")
            };

            base.OnException(actionExecutedContext);
        }
    }
}