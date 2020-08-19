using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace StudentDemo.HubWebapi.Authorize
{
    /// <summary>
    /// Ids4认证过滤器
    /// </summary>
    public class AuthorizeCheckOperationFilter : IOperationFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            //获取是否添加登录特性
            //策略名称映射到范围
            var authAttributes = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
             .Union(context.MethodInfo.GetCustomAttributes(true))
             .OfType<AuthorizeAttribute>()
             .Select(attr => attr.Policy)
             .Distinct();
            if (authAttributes.Any())
            {
                operation.Responses.Add("401",new OpenApiResponse { Description="暂无访问权限" });
                operation.Responses.Add("403",new OpenApiResponse { Description="禁止访问"});
                var oAuthScheme = new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference {Id="oauth2", Type= ReferenceType.SecurityScheme}
                };
                //给api添加锁的标注
                operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new OpenApiSecurityRequirement
                    {
                        [oAuthScheme]=authAttributes.ToList()
                    }
                };
            }
        }
    }
}
