using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;
using StudentDemo.DataDomin.DataUnitTest;
using StudentDemo.DominStd.Entities;
using StudentDemo.DominStd.Enums;
using StudentDemo.DominStd.Response;
using StudentDemo.MainWebClient.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StudentDemo.MainWebClient.Services
{
    public class StudentService : IStudentService
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly IConfiguration _iconfiguration;
        private readonly string _idsUrl = "";

        public StudentService(IHttpContextAccessor httpContext,IConfiguration iconfiguration)
        {
            _httpContext = httpContext;
            _iconfiguration = iconfiguration;
            _idsUrl = $"http://{_iconfiguration["Ids4Setting:Ip"]}:{_iconfiguration["Ids4Setting:Port"]}";
        }
        
        public async Task<ServiceResultOfT<StudentModel>> AddStudentAsync(StudentModel student)
        {
            var result = new ServiceResultOfT<StudentModel>();
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
                                {
                                    Address = _idsUrl,
                                    Policy = new DiscoveryPolicy { RequireHttps = false }
                                });
            if (disco.IsError)
            {
                throw new Exception(disco.Error);
            }

            var accessToken = await _httpContext.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

            client.SetBearerToken(accessToken);
            var contentJson = new StringContent(
                JsonConvert.SerializeObject(student),
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync("http://localhost:6100/api/studentservice/student", contentJson);
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    await RenewTokensAsync();
                    result.Code = ServiceResultCode.Unauthorized;
                    result.Entities = null;
                    return result;
                }
                if (response.StatusCode == HttpStatusCode.TooManyRequests)
                {
                    result.Code = ServiceResultCode.Failed;
                    result.Entities = null;
                    return result;
                }
                throw new Exception(response.ReasonPhrase);
            }
            var content = await response.Content.ReadAsStringAsync();
            if (content ==null)
            {
                result.Code = ServiceResultCode.Failed;
                result.Entities = null;
                return result;
            }
            var entity = JsonConvert.DeserializeObject<StudentModel>(content);
            client.Dispose();
            result.Code = ServiceResultCode.Success;
            result.Entities = entity;
            return result;
        }
        /// <summary>
        /// 删除一名学生信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ServiceResultOfT<IEnumerable<StudentModel>>> DeleteStudentAsync(int id)
        {
            var result = new ServiceResultOfT<IEnumerable<StudentModel>>();
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
                {
                    Address = _idsUrl,
                    Policy = new DiscoveryPolicy { RequireHttps = false }
                });
            if (disco.IsError)
            {
                throw new Exception(disco.Error);
            }

            var accessToken = await _httpContext.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

            client.SetBearerToken(accessToken);
            var response = await client.DeleteAsync($"http://localhost:6100/api/studentservice/student/{id}");
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    await RenewTokensAsync();
                    result.Code = ServiceResultCode.Unauthorized;
                    result.Entities = null;
                    return result;
                }
                if (response.StatusCode == HttpStatusCode.TooManyRequests)
                {
                    result.Code = ServiceResultCode.Failed;
                    result.Entities = null;
                    return result;
                }
                throw new Exception(response.ReasonPhrase);
            }

            var content = await response.Content.ReadAsStringAsync();
            var entity = JsonConvert.DeserializeObject<IEnumerable<StudentModel>>(content);
            client.Dispose();
            result.Code = ServiceResultCode.Success;
            result.Entities = entity;
            return result;
        }

        public async Task<ServiceResultOfT<StudentModel>> EditStudentAsync(StudentModel student)
        {
            var result = new ServiceResultOfT<StudentModel>();
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
                {
                    Address = _idsUrl,
                    Policy = new DiscoveryPolicy { RequireHttps = false }
                });
            if (disco.IsError)
            {
                throw new Exception(disco.Error);
            }

            var accessToken = await _httpContext.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

            client.SetBearerToken(accessToken);
            var contentJson = new StringContent(
                JsonConvert.SerializeObject(student),
                Encoding.UTF8,
                "application/json");

            var response = await client.PutAsync("http://localhost:6100/api/studentservice/student", contentJson);
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    await RenewTokensAsync();
                    result.Code = ServiceResultCode.Unauthorized;
                    result.Entities = null;
                    return result;
                }
                if (response.StatusCode == HttpStatusCode.TooManyRequests)
                {
                    result.Code = ServiceResultCode.Failed;
                    result.Entities = null;
                    return result;
                }
                throw new Exception(response.ReasonPhrase);
            }

            var content = await response.Content.ReadAsStringAsync();
            var entity = JsonConvert.DeserializeObject<StudentModel>(content);
            client.Dispose();
            result.Code = ServiceResultCode.Success;
            result.Entities = entity;
            return result;
        }
        /// <summary>
        /// 获取所有学生
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceResultOfT<IEnumerable<StudentModel>>> GetAllAsync()
        {
            var result = new ServiceResultOfT<IEnumerable<StudentModel>>();
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _idsUrl,
                Policy = new DiscoveryPolicy { RequireHttps = false }
            }) ;
            if (disco.IsError)
            {
                throw new Exception(disco.Error);
            }

            var accessToken = await _httpContext.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

            client.SetBearerToken(accessToken);
            var response = await client.GetAsync("http://localhost:6100/api/studentservice/student/all");
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    await RenewTokensAsync();
                    result.Code = ServiceResultCode.Unauthorized;
                    result.Entities = null;
                    return result;
                }
                if (response.StatusCode==HttpStatusCode.TooManyRequests)
                {
                    result.Code = ServiceResultCode.Failed;
                    result.Entities = null;
                    return result;
                }
                throw new Exception(response.ReasonPhrase);
            }

            var content = await response.Content.ReadAsStringAsync();
            var entity = JsonConvert.DeserializeObject<ServiceResultOfT<IEnumerable<StudentModel>> >(content);
            result.Code = ServiceResultCode.Success;
            result.Entities = entity.Entities;
            return result;
        }
        /// <summary>
        /// 获取一个学生
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceResultOfT<StudentModel>> GetStudentAsync(int id)
        {
            var result = new ServiceResultOfT<StudentModel>();
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
                {
                    Address = _idsUrl,
                    Policy = new DiscoveryPolicy { RequireHttps = false }
                });
            if (disco.IsError)
            {
                throw new Exception(disco.Error);
            }

            var accessToken = await _httpContext.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

            client.SetBearerToken(accessToken);
            var response = await client.GetAsync($"http://localhost:6100/api/studentservice/student/{id}");
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    await RenewTokensAsync();
                    result.Code = ServiceResultCode.Unauthorized;
                    result.Entities = null;
                    return result;
                }
                if (response.StatusCode == HttpStatusCode.TooManyRequests)
                {
                    result.Code = ServiceResultCode.Failed;
                    result.Entities = null;
                    return result;
                }
                throw new Exception(response.ReasonPhrase);
            }

            var content = await response.Content.ReadAsStringAsync();
            var entity = JsonConvert.DeserializeObject<ServiceResultOfT<StudentModel>>(content);
            result.Code = ServiceResultCode.Success;
            result.Entities = entity.Entities;
            return result;
        }
        /// <summary>
        /// 刷新Token
        /// </summary>
        /// <returns></returns>
        private async Task<string> RenewTokensAsync()
        {
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _idsUrl,
                Policy = new DiscoveryPolicy { RequireHttps = false }
            });
            if (disco.IsError)
            {
                throw new Exception(disco.Error);
            }

            var refreshToken = await _httpContext.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

            // Refresh Access Token
            var tokenResponse = await client.RequestRefreshTokenAsync(new RefreshTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "hybrid client",
                ClientSecret = "hybrid secret",
                Scope = "StudentServiceApi openid profile email phone address roles locations",
                GrantType = OpenIdConnectGrantTypes.RefreshToken,
                RefreshToken = refreshToken
            });

            if (tokenResponse.IsError)
            {
                throw new Exception(tokenResponse.Error);
            }

            var expiresAt = DateTime.UtcNow + TimeSpan.FromSeconds(tokenResponse.ExpiresIn);

            var tokens = new[]
            {
                new AuthenticationToken
                {
                    Name = OpenIdConnectParameterNames.IdToken,
                    Value = tokenResponse.IdentityToken
                },
                new AuthenticationToken
                {
                    Name = OpenIdConnectParameterNames.AccessToken,
                    Value = tokenResponse.AccessToken
                },
                new AuthenticationToken
                {
                    Name = OpenIdConnectParameterNames.RefreshToken,
                    Value = tokenResponse.RefreshToken
                },
                new AuthenticationToken
                {
                    Name = "expires_at",
                    Value = expiresAt.ToString("o", CultureInfo.InvariantCulture)
                }
            };

            // 获取身份认证的结果，包含当前的pricipal和properties
            var currentAuthenticateResult =
                await _httpContext.HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // 把新的tokens存起来
            currentAuthenticateResult.Properties.StoreTokens(tokens);

            // 登录
            await _httpContext.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                currentAuthenticateResult.Principal, currentAuthenticateResult.Properties);

            return tokenResponse.AccessToken;
        }
    }
}
