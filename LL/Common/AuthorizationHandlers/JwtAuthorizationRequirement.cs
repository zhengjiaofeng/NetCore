using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace LL.Models.Handlers
{
    public class JwtAuthorizationRequirement : IAuthorizationRequirement
    {
        public string issuer { get; set; }

        public string audience { get; set; }

        public DateTime? expires { get; set; }

        public SigningCredentials signingCredentials { get; set; }

        public JwtAuthorizationRequirement()
        {
            //jwtsettings = _jwtsettings;

            //this.issuer = jwtsettings.Value.Issuer;
            //this.audience = jwtsettings.Value.Audience;
            //this.expires = DateTime.Now.AddMinutes(65);
            ////对称秘钥
            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtsettings.Value.SecretKey));
            ////签名证书(秘钥，加密算法)
            //var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //this.signingCredentials = creds;

            //this.issuer = "";
            //this.audience = "";
            //this.expires = DateTime.Now.AddMinutes(65);
            ////对称秘钥
            //var SecretKey = "LL_2AWopCMMgEIWt6KkzxEJD0EA4xreXLINaQIDAQABAoGAcUQIoKWyldZa8xnPDJTMKIV8GpeuzebKWvwp5dIu+miTdzmZX4weeHADRNb";
            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            ////签名证书(秘钥，加密算法)
            //var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //this.signingCredentials = creds;
        }



    }
}
