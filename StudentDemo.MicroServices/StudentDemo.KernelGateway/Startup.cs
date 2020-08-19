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
            #region Ids4���ü�ע�Ἧ��
            Action<IdentityServerAuthenticationOptions> idsaOptClient = option =>                                             //����ids4����
            {
                option.Authority = $"http://{Configuration["IdentityService4:Ip"]}:{Configuration["IdentityService4:Port"]}"; //ids4��ַ
                                                                                                                              // option.Authority = "http://localhost:7300";                                                                 //ids4��ַ
                option.ApiName = "StudentServiceApi";                                                                         //Api��Դ����
                option.ApiSecret = "gatewayapi";
                option.RequireHttpsMetadata = Convert.ToBoolean(Configuration["IdentityService4:UseHttps"]);                  //�Ƿ�����https
                option.SupportedTokens = SupportedTokens.Both;                                                                //������֤֧��
            };
            var authenticationProviderKey = "NGITKernelGatewayKey";                                                           //·�ɺ���key
            services.AddAuthentication("Bearer")                                                                              //��֤Э��
                .AddIdentityServerAuthentication(authenticationProviderKey, idsaOptClient);                                   //ע����֤����
            #endregion

            #region Ocelot����
            services.AddOcelot()
                                .AddConsul()
                                //.AddCacheManager(x =>
                                //{
                                //    x.WithDictionaryHandle();//Ĭ���ֵ�洢
                                //})//����������ô�洢��--�뿴��ϸ��---1����Դ�� 2 �滻�»���
                                .AddPolly();
            #endregion

            services.AddMvcCore()
                    .AddApiExplorer();

            #region Swaggerע�����
            services.AddSwagger();
            #endregion
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            #region SwaggerApi�ĵ�����
            app.UseSwagger().UseSwaggerUI();
            #endregion

            app.UseOcelot().Wait();

        }
    }
}
