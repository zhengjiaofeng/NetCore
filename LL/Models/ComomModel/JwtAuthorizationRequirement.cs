using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LL.Models.ComomModel
{
    public class JwtAuthorizationRequirement: IAuthorizationRequirement
    {
        public string issuer { get; set; }

        public string audience { get; set; }

        public DateTime? expires { get; set; }

        public SigningCredentials signingCredentials { get; set; }

        private readonly IOptions<JWTSetting> jwtsettings;
        public JwtAuthorizationRequirement(IOptions<JWTSetting> _jwtsettings)
        {
            jwtsettings = _jwtsettings;

            this.issuer = jwtsettings.Value.Issuer;
            this.audience = jwtsettings.Value.Audience;
            this.expires = DateTime.Now.AddMinutes(65);
            //对称秘钥
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtsettings.Value.SecretKey));
            //签名证书(秘钥，加密算法)
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            this.signingCredentials = creds;
        }
    }
}
