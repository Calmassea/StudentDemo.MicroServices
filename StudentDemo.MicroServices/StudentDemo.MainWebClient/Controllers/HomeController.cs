using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using StudentDemo.DataDomin.DataUnitTest;
using StudentDemo.DominStd.Entities;
using StudentDemo.DominStd.Response;
using StudentDemo.MainWebClient.Models;
using StudentDemo.MainWebClient.ViewModels;
using StudentDemo.Tools.ApiSupport;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Linq;

namespace StudentDemo.MainWebClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _iconfiguration;
        private readonly IDistributedCache _cache;

        public HomeController(ILogger<HomeController> logger,IConfiguration iconfiguration, IDistributedCache cache )
        {
            _logger = logger;
            _iconfiguration = iconfiguration;
            _cache = cache;
        }
        public static int iIndex = 0;
        public async Task<IActionResult> Index()
        {
            #region Nginx
                 string url = "http://localhost:8088/api/student";
            #endregion
            #region Consul
            //Consul能提供的就只有服务的Ip:Port--DNS
            //url = "http://NGITHubWebapi/api/student";

            //ConsulClient client = new ConsulClient(c =>
            //{
            //    c.Address = new Uri("http://localhost:8500/");
            //    c.Datacenter = "dc1";
            //});
            //var response = client.Agent.Services().Result.Response;
            ////foreach (var item in response)
            ////{
            ////    Console.WriteLine("***************************************");
            ////    Console.WriteLine(item.Key);
            ////    var service = item.Value;
            ////    Console.WriteLine($"{service.Address}--{service.Port}--{service.Service}");
            ////    Console.WriteLine("***************************************");
            ////}

            //Uri uri = new Uri(url);
            //string groupName = uri.Host;
            //AgentService agentService = null;
            //var serviceDictionary = response.Where(s => s.Value.Service.Equals(groupName, StringComparison.OrdinalIgnoreCase)).ToArray();//得到3个实例  5726  5727  5728
            ////{
            ////    agentService = serviceDictionary[0].Value;//写死访问第1个
            ////}
            ////{
            //    if (iIndex > 100000) iIndex = 0;
            ////    agentService = serviceDictionary[iIndex++ % serviceDictionary.Length].Value;//轮询
            ////}
            //{
            //    int index = new Random(iIndex++).Next(0, serviceDictionary.Length);
            //    agentService = serviceDictionary[index].Value;//随机--平均
            //}
            //{
            //    //权重策略--能给不同的实例分配不同的压力---注册时提供权重
            //    List<KeyValuePair<string, AgentService>> pairsList = new List<KeyValuePair<string, AgentService>>();
            //    foreach (var pair in serviceDictionary)
            //    {
            //        int count = int.Parse(pair.Value.Tags?[0]);//1   5   10
            //        for (int i = 0; i < count; i++)
            //        {
            //            pairsList.Add(pair);
            //        }
            //    }
            //    //16个  
            //   // agentService = pairsList.ToArray()[new Random(iIndex++).Next(0, pairsList.Count())].Value;
            //}
            //url = $"{uri.Scheme}://{agentService.Address}:{agentService.Port}{uri.PathAndQuery}";
            #endregion
            #region Gateway
            url = "http://localhost:5100/api/studentservice/student/all";
            #endregion
            HomeViewModel home = new HomeViewModel();
            string cacheKey = "HomeCacheKey"; 
            try
            {
                var context = new ServiceResultOfT<IEnumerable<StudentModel>>();
                var result = _cache.GetStringAsync(cacheKey).Result;
                if (result == null)
                {
                    context = ApiConnect.InvokeApi<ServiceResultOfT<IEnumerable<StudentModel>>>(url, ApiConnect.RequestFunction.Get);
                    home.Students = context.Entities;
                    await _cache.SetStringAsync(cacheKey,JsonConvert.SerializeObject(context),new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(30)));
                }
                else
                {
                    context = JsonConvert.DeserializeObject<ServiceResultOfT<IEnumerable<StudentModel>>>(_cache.GetStringAsync(cacheKey).Result);
                    home.Students = context.Entities;
                }   
            }
            catch (Exception)
            {

                home.Students = null;
            }
            
            
            return View(home);
        }

        
        public  IActionResult AccessApi1()
        {

            return View();
        }

        // [Authorize(Roles = "管理员")]
        //[Authorize(Policy = "SmithInSomewhere")]
        public async Task<IActionResult> Privacy()
        {
            var accessToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            var idToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.IdToken);

            var refreshToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);
            // var code = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.Code);

            ViewData["accessToken"] = accessToken;
            ViewData["idToken"] = idToken;
            ViewData["refreshToken"] = refreshToken;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        /// <summary>
        /// 注销登录
        /// </summary>
        /// <returns></returns>
        public async Task Logout()
        {
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest 
                            {
                                Address= $"http://{_iconfiguration["Ids4Setting:Ip"]}:{_iconfiguration["Ids4Setting:Port"]}",//123.57.162.237
                                Policy =new DiscoveryPolicy { RequireHttps=false} 
                            });
            if (disco.IsError)
            {
                throw new Exception(disco.Error);
            }

            var accessToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                var revokeAccessTokenResponse = await client.RevokeTokenAsync(new TokenRevocationRequest
                {
                    Address = disco.RevocationEndpoint,
                    ClientId = "hybrid client",
                    ClientSecret = "hybrid secret",
                    Token = accessToken
                });

                if (revokeAccessTokenResponse.IsError)
                {
                    throw new Exception("Access Token Revocation Failed: " + revokeAccessTokenResponse.Error);
                }
            }

            var refreshToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);
            if (!string.IsNullOrWhiteSpace(refreshToken))
            {
                var revokeRefreshTokenResponse = await client.RevokeTokenAsync(new TokenRevocationRequest
                {
                    Address = disco.RevocationEndpoint,
                    ClientId = "hybrid client",
                    ClientSecret = "hybrid secret",
                    Token = refreshToken
                });

                if (revokeRefreshTokenResponse.IsError)
                {
                    throw new Exception("Refresh Token Revocation Failed: " + revokeRefreshTokenResponse.Error);
                }
            }

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
        }

        
    }
}
