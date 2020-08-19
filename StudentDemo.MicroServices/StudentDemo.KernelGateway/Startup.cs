using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using IdentityServer4.AccessTokenValidation;
using IdentityServer4.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Ocelot.Provider.Polly;
using StudentDemo.KernelGateway.Extends;

namespace StudentDemo.KernelGateway
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region Ids4配置及注册集成
            Action<IdentityServerAuthenticationOptions> idsaOptClient = option =>                                             //罗列ids4配置
            {
                option.Authority = $"http://{Configuration["IdentityService4:Ip"]}:{Configuration["IdentityService4:Port"]}"; //ids4地址
                                                                                                                              // option.Authority = "http://localhost:7300";                                                                 //ids4地址
                option.ApiName = "StudentServiceApi";                                                                         //Api资源名称
                option.ApiSecret = "gatewayapi";
                option.RequireHttpsMetadata = Convert.ToBoolean(Configuration["IdentityService4:UseHttps"]);                  //是否启用https
                option.SupportedTokens = SupportedTokens.Both;                                                                //所有认证支持
            };
            var authenticationProviderKey = "NGITKernelGatewayKey";                                                           //路由核心key
            services.AddAuthentication("Bearer")                                                                              //认证协议
                .AddIdentityServerAuthentication(authenticationProviderKey, idsaOptClient);                                   //注册认证服务
            #endregion

            #region Ocelot配置
            services.AddOcelot()
                                .AddConsul()
                                //.AddCacheManager(x =>
                                //{
                                //    x.WithDictionaryHandle();//默认字典存储
                                //})//看不到是怎么存储的--想看到细节---1看看源码 2 替换下缓存
                                .AddPolly();
            #endregion

            services.AddMvcCore()
                    .AddApiExplorer();

            #region Swagger注册服务
            services.AddSwagger();
            #endregion
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            #region SwaggerApi文档配置
            app.UseSwagger().UseSwaggerUI();
            #endregion

            app.UseOcelot().Wait();

        }
    }
}
