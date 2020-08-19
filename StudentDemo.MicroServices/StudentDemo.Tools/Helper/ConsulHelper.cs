using Consul;
using Microsoft.Extensions.Configuration;
using System;

namespace StudentDemo.Tools.Helper
{
    public static class ConsulHelper
    {
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="address"></param>
        public static void ConsulRegister(this IConfiguration configuration, string address)
        {
            
            ConsulClient client = new ConsulClient(c=> {
                c.Address =new Uri(address);
                c.Datacenter = "dc1";
            });
            string nowdate = DateTime.Now.DayOfYear.ToString();
            //ip&port
            string ip = configuration["ip"]??"127.0.0.1";
            Random random = new Random();
            int port =int.Parse(configuration["port"]??"5000");//命令行参数必须传入
            int weight = random.Next(1,5)+1;
            //weight = string.IsNullOrWhiteSpace(configuration["weight"]) ? 1 : int.Parse(configuration["weight"]);//权重
            client.Agent.ServiceRegister(new AgentServiceRegistration() { 
                ID="HubWebapi"+nowdate+random.Next(1000,9999)+1,
                Name="NGITHubWebapi",
                Address = ip,
                Port = port,
                Tags = new string[] { weight.ToString() },//标签
                //心跳检查
                Check = new AgentServiceCheck()
                {
                    Interval = TimeSpan.FromSeconds(12),//间隔12s一次
                    HTTP = $"http://{ip}:{port}/Api/Health/Index",//get
                    Timeout = TimeSpan.FromSeconds(2),//检测等待时间
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(10)//失败后多久移除
                }

            });
            //命令行参数获取
            Console.WriteLine($"{ip}:{port}--weight:{weight}");
        }
        public static void ConsulRegister(this IConfiguration configuration, string address,string projectName)
        {

            ConsulClient client = new ConsulClient(c => {
                c.Address = new Uri(address);
                c.Datacenter = "dc1";
            });
            string nowdate = DateTime.Now.DayOfYear.ToString();
            //ip&port
            string ip = configuration["ip"] ?? "127.0.0.1";
            Random random = new Random();
            int port = int.Parse(configuration["port"] ?? "5000");//命令行参数必须传入
            int weight = random.Next(1, 5) + 1;
            //weight = string.IsNullOrWhiteSpace(configuration["weight"]) ? 1 : int.Parse(configuration["weight"]);//权重
            client.Agent.ServiceRegister(new AgentServiceRegistration()
            {
                ID = "Student"+projectName + nowdate + random.Next(1000, 9999) + 1,
                Name = projectName,
                Address = ip,
                Port = port,
                Tags = new string[] { weight.ToString() },//标签
                //心跳检查
                Check = new AgentServiceCheck()
                {
                    Interval = TimeSpan.FromSeconds(12),//间隔12s一次
                    HTTP = $"http://{ip}:{port}/Api/Health/Index",//get
                    Timeout = TimeSpan.FromSeconds(2),//检测等待时间
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(10)//失败后多久移除
                }

            });
            //命令行参数获取
            Console.WriteLine($"{ip}:{port}--weight:{weight}");
        }
    }
}
