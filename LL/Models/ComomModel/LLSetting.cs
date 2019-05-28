using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LL.Models.ComomModel
{
    public class LLSetting
    {
        /// <summary>
        /// 私钥
        /// </summary>
        public string RsaPrivateKey { get; set; }

        /// <summary>
        /// 公钥
        /// </summary>
        public string RsaPublicKey { get; set; }
    }
}
