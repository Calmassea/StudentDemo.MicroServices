using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StudentDemo.HubWebapi.Exstends;
using StudentDemo.HubWebapi.Model;
using StudentDemo.HubWebapi.Services;
using StudentDemo.Tools.ApiSupport;
using StudentDemo.Tools.Helper;
using StudentDemo.Tools.Services;

namespace StudentDemo.HubWebapi
{
    /// <summary>
    /// 中间件配置
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// 服务注册
        /// </summary>
        /// <param name="services"></param>
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //未使用认证移动至网关
            #region Ids4配置及注册
            //Action<IdentityServerAuthenticationOptions> idsaOptClient = option =>                                             //罗列ids4配置
            //{
            //    option.Authority = $"http://{Configuration["IdentityService4:Ip"]}:{Configuration["IdentityService4:Port"]}"; //ids4地址                                                              //ids4地址
            //    option.ApiName = "StudentServiceApi";                                                                         //Api资源名称
            //    option.RequireHttpsMetadata = Convert.ToBoolean(Configuration["IdentityService4:UseHttps"]);                  //是否齐用https
            //    option.SupportedTokens = SupportedTokens.Both;                                                                //所有认证支持
            //};
            //services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)                                                                              //认证协议
            //    .AddIdentityServerAuthentication(idsaOptClient);                                                             //注册认证服务
            #endregion
            #region 存储提供商
            services.AddDbContext<StudentDbcontext>(opt=> { opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")); });
            
            services.AddScoped<ICommonService,StudentService>();
            #endregion
            #region 添加apiSwagger文档
            services.AddSwagger();
            #endregion

            services.AddControllers().AddNewtonsoftJson();

        }

        /// <summary>
        /// 请求管道
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            #region Swagger管道配置
            //swagger终结点模板
            app.UseSwagger(o=> {
                o.RouteTemplate = "docs/{documentName}/swagger.json";
            });
            app.UseSwaggerUI();
            #endregion
            
            
            app.UseRouting();

            //app.UseAuthentication();//认证

            app.UseAuthorization(); //授权

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            #region Consul注册
            if (this.Configuration["Consul:RegisterAddress"].MatchUrl())
            {
                this.Configuration.ConsulRegister(this.Configuration["Consul:RegisterAddress"],"hubWebApi");
            }
            else
            {
                Console.WriteLine("Consul地址不正确请检查配置文件！");
            }
            #endregion
        }
       
    }
}
