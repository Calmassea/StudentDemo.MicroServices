using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static StudentDemo.Domin.Shared.PublicConsts;

namespace StudentDemo.HubWebapi.Exstends
{
    /// <summary>
    /// 提供Swagger服务扩展
    /// </summary>
    public static class SwaggerExstends
    {
        /// <summary>
        /// 当前API版本，从appsettings.json获取
        /// </summary>
        private static readonly string version = "V1";

        /// <summary>
        /// Swagger描述信息
        /// </summary>
        private static readonly string description = @"<b>Website</b>：<a target=""_blank"" href=""http://www.nuogit.com"">http://www.nuogit.com</a> <b>GitHub</b>：<a target=""_blank"" href=""https://github.com/Calmassea/SimpleCaptcha"">https://github.com/Calmassea/SimpleCaptchaa</a> <b>Hangfire</b>：<a target=""_blank"" href=""#"">任务调度中心</a> <code>Powered by .NET Core 3.1 on Linux</code>";

        /// <summary>
        /// Swagger分组信息，将进行遍历使用
        /// </summary>
        private static readonly List<SwaggerApiInfo> ApiInfos = new List<SwaggerApiInfo>()
        {
            new SwaggerApiInfo
            {
                UrlPrefix="hubwebapi",
                Name = "大数据架构核心汇聚接口",
                OpenApiInfo = new OpenApiInfo
                {
                    Version = version,
                    Title = "NGIT - 大数据架构核心汇聚接口",
                    Description = description
                }
            }
        };
        /// <summary>
        /// AddSwagger服务注册
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
          #region 添加apiSwagger文档

          return services.AddSwaggerGen(options => {
                // 遍历并应用Swagger分组信息
                ApiInfos.ForEach(x =>
                {
                    options.SwaggerDoc(x.UrlPrefix, x.OpenApiInfo);
                });
                #region Xml注释文件加载
                //获取Xml文件名{Assembly.GetExecutingAssembly().GetName().Name}老版本
                //获取xml文件路径
                var xmlpath = Path.Combine(AppContext.BaseDirectory,  "StudentDemo.HubWebapi.xml");
                //添加控制器注释，true表示显示控制器注释
                options.IncludeXmlComments(xmlpath, true);
                #endregion
                #region 小绿锁Api身份认证
                //向生成的Swagger添加一个或多个“securityDefinitions”，用于API的登录校验
                var security = new OpenApiSecurityScheme
                {
                    Description = "JWT模式授权，请输入 Bearer {Token} 进行身份验证",
                    Name = "Authorization",
                    //In = ParameterLocation.Header,
                    //Type = SecuritySchemeType.ApiKey
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            //授权地址
                            //AuthorizationUrl = new Uri($"http://localhost:7300}/connect/authorize"),
                            Scopes = new Dictionary<string, string>
                            {
                                { "StudentServiceApi", "StudentServiceApi" },
                            }
                        }
                    }

                };
                options.AddSecurityDefinition("oauth2", security);
                options.AddSecurityRequirement(new OpenApiSecurityRequirement { { security, new List<string>() } });
                //添加IdentityServer4认证过滤Filter(Aop)
                options.OperationFilter<AddResponseHeadersFilter>();
                options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                options.OperationFilter<SecurityRequirementsOperationFilter>();
                #endregion
            });
            #endregion
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
                ApiInfos.ForEach(x =>
                {
                    options.SwaggerEndpoint($"/docs/{x.UrlPrefix}/swagger.json", x.Name);
                });
                // API页面Title
                options.DocumentTitle = "😍网站汇聚接口文档 - 博远Plus⭐⭐⭐";
                options.OAuthClientId("Swagger");//客服端名称
                options.OAuthAppName("Swagger UI for demo_api");// 描述
                options.OAuthClientSecret("");//密码
            });
        }
    }
    /// <summary>
    /// Swagger接口信息
    /// </summary>
    internal class SwaggerApiInfo
    {
        /// <summary>
        /// URL前缀
        /// </summary>
        public string UrlPrefix { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// <see cref="Microsoft.OpenApi.Models.OpenApiInfo"/>
        /// </summary>
        public OpenApiInfo OpenApiInfo { get; set; }
    }
}
