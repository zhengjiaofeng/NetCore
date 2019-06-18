using LL.Models.ComomModel;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace LL.Common.AuthorizationHandlers
{
    public class JwtComom
    {
        /// <summary>
        /// jwt token验证
        /// </summary>
        /// <param name="jwtsettings"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public JwtSecurityToken ValidateAndDecode(IOptions<JWTSetting> jwtsettings,string token)
        {
            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    //Token颁发机构
                    ValidIssuer = jwtsettings.Value.Issuer,
                    //颁发给谁
                    ValidAudience = jwtsettings.Value.Audience,
                    //这里的key要进行加密，需要引用Microsoft.IdentityModel.Tokens
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtsettings.Value.SecretKey)),
                    //验证token 有效期
                    ValidateLifetime = true,
                    //ValidateIssuer = true,
                    //ValidateAudience = true,
                    ValidateIssuerSigningKey = true
                    ///允许的服务器时间偏移量
                    //ClockSkew = TimeSpan.Zero
                };

                //验证token和读取
                var claimsPrincipal = new JwtSecurityTokenHandler()
          .ValidateToken(token, validationParameters, out var rawValidatedToken);

                return (JwtSecurityToken)rawValidatedToken;
            }
            catch (Exception ex)
            {
                throw new Exception($"Token failed validation: {ex.Message}");
            }
        }
    }
}
