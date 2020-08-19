using IdentityServer4;
using IdentityServer4.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StudentDemo.AuthenticationCenterIds4.Configs;
using StudentDemo.AuthenticationCenterIds4.Data;
using System;

namespace StudentDemo.AuthenticationCenterIds4
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            #region 存储提供商
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<IdentityUser,IdentityRole>(
                options => {
                    options.SignIn.RequireConfirmedAccount = false;
                    //这里面是Identity设置
                    // Password settings.
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequiredLength = 6;
                    options.Password.RequiredUniqueChars = 1;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            #endregion

            services.AddMvc();
            //连接数据库
            //services.AddDbContext<AuthenticationDbContext>(options => options.UseMySql(Configuration.GetConnectionString("MySqlConnection")));
            #region Ids4配置
            //注册Ids4认证服务
                        Action<IdentityServerOptions> ids4Options = option =>
                        {
                            option.Events.RaiseErrorEvents = true;
                            option.Events.RaiseFailureEvents = true;
                            option.Events.RaiseInformationEvents = true;
                            option.Events.RaiseSuccessEvents = true;

                        };
                        var ids4Builder = services.AddIdentityServer()
                                          .AddInMemoryApiResources(ClientInitConfig.GetApiResources)           //添加API资源
                                          .AddInMemoryClients(ClientInitConfig.GetClients)                     //添加服务对象认证配置
                                         // .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()         //添加自定义验证方式
                                         // .AddProfileService<ProfileService>()                                 //用户信息获取
                                         .AddAspNetIdentity<IdentityUser>()
                                          .AddInMemoryIdentityResources(ClientInitConfig.GetIdentityResources);//添加Openid Connect支持
                        ids4Builder.AddDeveloperSigningCredential();                                           //添加开发者私钥证书

            #endregion
            services.Configure<IISOptions>(options =>
            {
                options.AutomaticAuthentication = false;
                options.AuthenticationDisplayName = "Windows";
            });
            # region 跨域配置
            services.AddCors(options =>
            {
                options.AddPolicy("any", builder =>
                {
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            }); //配置跨域
            #endregion
            #region 外部登录
            var authProvider=  services.AddAuthentication();
            //Github登录
            //authProvider.AddGitHub(options=> {
            //    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
            //    options.ClientId = "";//id
            //    options.ClientSecret = "";//key
            //    options.Scope.Add("user:email");
            //});

            #endregion
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            #region Nginx反向代理请求头配置
            var fordwardedHeaderOptions = new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            };
            fordwardedHeaderOptions.KnownNetworks.Clear();
            fordwardedHeaderOptions.KnownProxies.Clear();

            app.UseForwardedHeaders(fordwardedHeaderOptions);
            #endregion

            app.UseRouting();

            app.UseIdentityServer();                                                          //添加IdentityServer4认证管道

            app.UseCors("any");                                                               //跨域管道


            app.UseAuthorization();
            //MVC Router
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
