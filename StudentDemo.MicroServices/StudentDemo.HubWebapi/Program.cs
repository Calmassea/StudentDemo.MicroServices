using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StudentDemo.HubWebapi.Model;
using System;

namespace StudentDemo.HubWebapi
{
    /// <summary>
    /// 
    /// </summary>
    public class Program
    {
        /// <summary>
        /// ³ÌÐòÈë¿Ú
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {

           var host=  CreateHostBuilder(args).Build();
            //using (var scope= host.Services.CreateScope())
            //{
            //    var service = scope.ServiceProvider;
            //    var context = service.GetRequiredService<StudentDbcontext>();
            //    Console.WriteLine();
            //}
            host.Run();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
