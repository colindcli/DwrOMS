using OmsClient.Common;
using OmsClient.Web;
using Swashbuckle.Application;
using Swashbuckle.Swagger;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using WebActivatorEx;
#if DEBUG
[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]
#endif

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Web
{
    /// <summary>
    /// 
    /// </summary>
    public class GlobalHttpHeaderFilter : IOperationFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="schemaRegistry"></param>
        /// <param name="apiDescription"></param>
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            if (operation.parameters == null)
            {
                operation.parameters = new List<Parameter>();
            }

            //operation.parameters.Add(new Parameter { name = "Id", @in = "header", description = "Id", required = false, type = "string", @default = "0" });
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SwaggerConfig
    {
        /// <summary>
        /// 
        /// </summary>
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", "OmsClient.Web");
                    c.IncludeXmlComments(GetXmlCommentsPath(thisAssembly.GetName().Name));

                    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

                    c.SchemaId(t => t.FullName.Contains('`') ? t.FullName.Substring(0, t.FullName.IndexOf('`')) : t.FullName);

                    c.IgnoreObsoleteProperties();

                    c.DescribeAllEnumsAsStrings();

                    c.OperationFilter<GlobalHttpHeaderFilter>();


                })
                .EnableSwaggerUi(c =>
                {
                });
        }

        private static string GetXmlCommentsPath(string fileName)
        {
            return string.Format("{0}/bin/{1}.XML", Config.Root, fileName);
        }
    }
}
