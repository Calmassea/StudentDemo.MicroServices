using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using StudentDemo.MainWebClient.Services;

namespace StudentDemo.MainWebClient
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
            //运行程序时实时编译Razor试图
            services.AddRazorPages()
                .AddRazorRuntimeCompilation();
            //Cookie政策询问
            services.Configure<CookiePolicyOptions>(options=> {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            #region 认证服务配置
            //清楚JWT默认声明映射
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            var auth = services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            });
            //Cookie鉴权
            auth.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,options =>
            {
                options.AccessDeniedPath = "/Account/AccessDenied";//无权限访问地址
            });
            auth.AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme,options=> {
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.RequireHttpsMetadata = false;
                options.Authority = $"http://{Configuration["Ids4Setting:Ip"]}:{Configuration["Ids4Setting:Port"]}";

                options.ClientId = "hybrid client";
                options.ClientSecret = "hybrid secret";
                options.ResponseType = "code id_token";
                options.SaveTokens = true;

                options.Scope.Add("StudentServiceApi");
                options.Scope.Add(OidcConstants.StandardScopes.OpenId);
                options.Scope.Add(OidcConstants.StandardScopes.Profile);
                options.Scope.Add(OidcConstants.StandardScopes.Email);
                options.Scope.Add(OidcConstants.StandardScopes.Address);
                options.Scope.Add(OidcConstants.StandardScopes.Phone);
                options.Scope.Add(OidcConstants.StandardScopes.OfflineAccess);
                options.Scope.Add("roles");
                options.Scope.Add("locations");
                // 集合里的东西 都是要被过滤掉的属性，nbf amr exp...
                options.ClaimActions.Remove("nbf");
                options.ClaimActions.Remove("amr");
                // options.ClaimActions.Remove("exp");
                // 不映射到User Claims里
                options.ClaimActions.DeleteClaim("sid");
                options.ClaimActions.DeleteClaim("sub");
                options.ClaimActions.DeleteClaim("idp");

                options.TokenValidationParameters = new TokenValidationParameters {
                    NameClaimType = JwtClaimTypes.Name,
                    RoleClaimType=JwtClaimTypes.Role
                };
            });
            #endregion
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "127.0.0.1:6379,allowadmin=true,abortConnect=false";
                options.InstanceName = "Mainweb_";
            });
            services.AddSingleton<IStudentService,StudentService>();
            //httpcontext
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
           // app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();//认证

            app.UseAuthorization();//授权

            app.UseCookiePolicy();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
