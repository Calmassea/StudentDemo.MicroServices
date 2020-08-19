using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StudentDemo.ScoreWebapi.Exstends;
using StudentDemo.ScoreWebapi.Models;
using StudentDemo.ScoreWebapi.Services;
using StudentDemo.Tools.ApiSupport;
using StudentDemo.Tools.Helper;
using StudentDemo.Tools.Services;

namespace StudentDemo.Score
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
            #region 添加apiSwagger文档
            services.AddSwagger();
            #endregion
            services.AddDbContext<ScoreDbContext>(opt=> {
                opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddScoped<ICommonService,ScoreService>();
            services.AddControllers().AddNewtonsoftJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            #region Swagger管道配置
            //swagger终结点模板
            app.UseSwagger(o => {
                o.RouteTemplate = "docs/{documentName}/swagger.json";
            });
            app.UseSwaggerUI();
            #endregion

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            #region Consul注册
            if (this.Configuration["Consul:RegisterAddress"].MatchUrl())
            {
                this.Configuration.ConsulRegister(this.Configuration["Consul:RegisterAddress"], "ScoreWebApi");
            }
            else
            {
                Console.WriteLine("Consul地址不正确请检查配置文件！");
            }
            #endregion
        }
    }
}
