using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace StudentDemo.KernelGateway
{
    public class GatewaySetting
    {
        public static IConfigurationRoot _configuration { get; }
        static GatewaySetting()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                    .AddJsonFile("GatewayConfig.json", true, true);
            _configuration = builder.Build();
        }
        /// <summary>
        /// ids4设置获取
        /// </summary>
        public static class Ids4Settings
        {
            public static string Ip => _configuration["IdentityService4:Ip"];
            public static string Port =>_configuration["IdentityService4:Port"];
        }
    }
}
