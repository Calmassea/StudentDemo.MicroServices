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
            #region �洢�ṩ��
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<IdentityUser,IdentityRole>(
                options => {
                    options.SignIn.RequireConfirmedAccount = false;
                    //��������Identity����
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
            //�������ݿ�
            //services.AddDbContext<AuthenticationDbContext>(options => options.UseMySql(Configuration.GetConnectionString("MySqlConnection")));
            #region Ids4����
            //ע��Ids4��֤����
                        Action<IdentityServerOptions> ids4Options = option =>
                        {
                            option.Events.RaiseErrorEvents = true;
                            option.Events.RaiseFailureEvents = true;
                            option.Events.RaiseInformationEvents = true;
                            option.Events.RaiseSuccessEvents = true;

                        };
                        var ids4Builder = services.AddIdentityServer()
                                          .AddInMemoryApiResources(ClientInitConfig.GetApiResources)           //���API��Դ
                                          .AddInMemoryClients(ClientInitConfig.GetClients)                     //��ӷ��������֤����
                                         // .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()         //����Զ�����֤��ʽ
                                         // .AddProfileService<ProfileService>()                                 //�û���Ϣ��ȡ
                                         .AddAspNetIdentity<IdentityUser>()
                                          .AddInMemoryIdentityResources(ClientInitConfig.GetIdentityResources);//���Openid Connect֧��
                        ids4Builder.AddDeveloperSigningCredential();                                           //��ӿ�����˽Կ֤��

            #endregion
            services.Configure<IISOptions>(options =>
            {
                options.AutomaticAuthentication = false;
                options.AuthenticationDisplayName = "Windows";
            });
            # region ��������
            services.AddCors(options =>
            {
                options.AddPolicy("any", builder =>
                {
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            }); //���ÿ���
            #endregion
            #region �ⲿ��¼
            var authProvider=  services.AddAuthentication();
            //Github��¼
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

            #region Nginx�����������ͷ����
            var fordwardedHeaderOptions = new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            };
            fordwardedHeaderOptions.KnownNetworks.Clear();
            fordwardedHeaderOptions.KnownProxies.Clear();

            app.UseForwardedHeaders(fordwardedHeaderOptions);
            #endregion

            app.UseRouting();

            app.UseIdentityServer();                                                          //���IdentityServer4��֤�ܵ�

            app.UseCors("any");                                                               //����ܵ�


            app.UseAuthorization();
            //MVC Router
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
