using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StudentDemo.AuthenticationCenterIds4.Configs
{
    public static class ClientInitConfig
    {
        /// <summary>
        /// 下方定义的api资源和客户端访问密钥等，都可以动态的从配置文件中读取
        /// </summary>
        public static IConfiguration Configuration { get; set; }

        /// <summary>
        /// 添加对OpenID Connect的支持
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IdentityResource> GetIdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResources.Address(),
                new IdentityResources.Phone(),
                new IdentityResource("roles", "角色", new List<string> { JwtClaimTypes.Role }),
                new IdentityResource("locations", "地点", new List<string> { "location" }),
            };


        /// <summary>
        /// 需要保护的api资源
        /// </summary>
        public static IEnumerable<ApiResource> GetApiResources =>
            new ApiResource []
            {
                new ApiResource("StudentServiceApi","学生信息服务Api",new List<string>{"locations" }){ 
                    ApiSecrets= { new Secret("gatewayapi".Sha256()) }
                },
                new ApiResource("ProductServiceApi","ProductServiceapi")
            };
        /// <summary>
        /// 定制验证条件
        /// </summary>
        public static IEnumerable<Client> GetClients =>
            new List<Client>
            {
                //客户端模式
                new Client
                {
                    ClientId="Web.Client",
                    ClientName="Web.Client.Name",
                    ClientSecrets=new [] { new Secret("hby123456".Sha256()) },
                    AllowAccessTokensViaBrowser = true,//是否通过浏览器为此客户端传输访问令牌
                    AllowedGrantTypes=GrantTypes.ClientCredentials,
                    AccessTokenLifetime=3600*8,    //token过期时间设置为8小时
                   
                    //允许访问api的范围
                    AllowedScopes = new [] { "StudentServiceApi", "ProductServiceApi",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile },
                    
                    AllowOfflineAccess=true,
                    Claims=new List<Claim>()
                    {
                        new Claim(JwtClaimTypes.Role,"Admin"),
                        new Claim(JwtClaimTypes.NickName,"hby"),
                        new Claim("eMail","1944703558@qq.com")
                    }
                },
                new Client
                {
                    ClientId = "Swagger",//客服端名称
                    ClientName = "Swagger UI for demo_api",//描述
                    AllowedGrantTypes = GrantTypes.Implicit,//指定允许的授权类型（AuthorizationCode，Implicit，Hybrid，ResourceOwner，ClientCredentials的合法组合）。
                    AllowAccessTokensViaBrowser = true,//是否通过浏览器为此客户端传输访问令牌
                    AccessTokenLifetime=3600*8,    //token过期时间设置为8小时
                    RedirectUris =
                    {
                        "http://localhost:6100/swagger/oauth2-redirect.html"
                    },
                    AllowedScopes = { "StudentServiceApi",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile  }//指定客户端请求的api作用域。 如果为空，则客户端无法访问
                },
                //密码模式
                new Client
                {
                    ClientId="Web.Manager",
                    ClientName="Web.Manager.Name",
                    ClientSecrets=new [] { new Secret("manager123456".Sha256()) },
                  
                    AllowedGrantTypes=GrantTypes.ResourceOwnerPassword,
                    AccessTokenLifetime=3600*8,    //token过期时间设置为8小时
                   
                    //允许访问api的范围
                    AllowedScopes = new [] { "StudentServiceApi",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile },
                    
                    AllowOfflineAccess=true
                },
                // mvc, hybrid flow混合模式
                new Client
                {
                    ClientId = "hybrid client",
                    ClientName = "ASP.NET Core Hybrid 客户端",
                    ClientSecrets = {new Secret("hybrid secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Hybrid,

                    AccessTokenType = AccessTokenType.Reference,

                    RedirectUris =
                    {
                        "http://localhost:4000/signin-oidc"
                    },

                    PostLogoutRedirectUris =
                    {
                        "http://localhost:4000/signout-callback-oidc"
                    },

                    AccessTokenLifetime=3600*8,    //token过期时间设置为8小时

                    AllowOfflineAccess = true,

                    AlwaysIncludeUserClaimsInIdToken = true,

                    AllowedScopes =
                    {
                        "StudentServiceApi",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Address,
                        IdentityServerConstants.StandardScopes.Phone,
                        IdentityServerConstants.StandardScopes.Profile,
                        "roles",
                        "locations"
                    }

                },
                //score client mvc, hybrid flow混合模式
                new Client
                {
                    ClientId = "score client",
                    ClientName = "学生成绩管理 客户端",
                    ClientSecrets = {new Secret("score secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Hybrid,

                    AccessTokenType = AccessTokenType.Reference,

                    RedirectUris =
                    {
                        "http://localhost:4001/signin-oidc"
                    },

                    PostLogoutRedirectUris =
                    {
                        "http://localhost:4001/signout-callback-oidc"
                    },

                    AccessTokenLifetime=3600*8,    //token过期时间设置为8小时

                    AllowOfflineAccess = true,

                    AlwaysIncludeUserClaimsInIdToken = true,

                    AllowedScopes =
                    {
                        "StudentServiceApi",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Address,
                        IdentityServerConstants.StandardScopes.Phone,
                        IdentityServerConstants.StandardScopes.Profile,
                        "roles",
                        "locations"
                    }

                }
            };
    }
}
