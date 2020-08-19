using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace StudentDemo.KernelGateway.Extends
{
    /// <summary>
    /// 提供Swagger组件扩展
    /// </summary>
    public static class SwaggerExstends
    {
        /// <summary>
        /// Api顶级名称列表
        /// </summary>
        public static readonly List<string> apiList =GatewaySetting._configuration["Swaggersetting:ServiceDocNames"].Split(',').ToList();
        /// <summary>
        /// AddSwagger服务注册
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            #region apiSwagger配置文档列表
            //var apiSwitch = GatewaySetting._configuration["Swaggersetting:Enable"];
            ////api服务名称列表
            //var apiName = GatewaySetting._configuration["Swaggersetting:ServiceApiNames"].Split(',').ToList();
            ////api服务备注
            //var apiTags = GatewaySetting._configuration["Swaggersetting:ServiceApiNamesTags"].Split(',').ToList();

            //var apiScopes = new Dictionary<string, string> { };
            //for (int i = 0; i < (apiName.Count() == apiTags.Count() ? apiName.Count() : apiName.Count()); i++)
            //{
            //    apiScopes.Add(apiName[i], apiTags[i]??apiName[i]);
            //}
            #endregion

            return services.AddSwaggerGen();
        }

        /// <summary>
        /// Use已配置完成的SwaggerUI管道
        /// </summary>
        /// <param name="app"></param>
        public static void UseSwaggerUI(this IApplicationBuilder app)
        {
            app.UseSwaggerUI(options =>
            {
                // 遍历分组信息，生成Json
                apiList.ForEach(apiItem =>
                {
                    options.SwaggerEndpoint($"/docs/{apiItem}/swagger.json", apiItem);
                });
            });
        }
    }
}

