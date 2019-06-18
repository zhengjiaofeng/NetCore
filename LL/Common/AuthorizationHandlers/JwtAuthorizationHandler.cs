using LL.Common.AuthorizationHandlers;
using LL.Models.ComomModel;
using LL.Models.Handlers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace LL.Common.Handlers
{
    public class JwtAuthorizationHandler : AuthorizationHandler<JwtAuthorizationRequirement>
    {
        private readonly IOptions<JWTSetting> jwtsettings;
        /// <summary>
        /// 验证方案提供对象
        /// </summary>
        public IAuthenticationSchemeProvider Schemes { get; set; }
        /// <summary>
        /// 自定义策略参数
        /// </summary>
        public JwtAuthorizationRequirement Requirement
        { get; set; }
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="schemes"></param>
        public JwtAuthorizationHandler(IOptions<JWTSetting> _jwtsettings)
        {
            jwtsettings = _jwtsettings;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, JwtAuthorizationRequirement requirement)
        {
            
            bool valiResult = false;//验证结果

            //从AuthorizationHandlerContext转成HttpContext，以便取出表求信息
            var httpContext = (context.Resource as Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext).HttpContext;
            //获取Authorization参数
            bool result = httpContext.Request.Headers.TryGetValue("Authorization", out StringValues tokenstr);
            if (!result || string.IsNullOrEmpty(tokenstr))
            {
                //httpContext.Response.Redirect("/Account/Index");
                //context.Fail();
                valiResult = false;
            }
            else
            {
                JwtComom jwtcommon = new JwtComom();
                string token= tokenstr.ToString().Replace("Bearer ","");
                //jwt token 验证
                JwtSecurityToken jwtSecurityToken= jwtcommon.ValidateAndDecode(jwtsettings, token);
                if (jwtSecurityToken != null)
                {
                    valiResult = true;
                }
                else
                {
                    valiResult = false;
                }
            }

            if (valiResult)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail(); 
            }

            return Task.CompletedTask;
        }
    }


}
