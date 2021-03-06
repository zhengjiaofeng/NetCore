﻿namespace LL.Models.ComomModel
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

    public class JWTSetting
    {
        /// <summary>
        /// 颁发机构
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// 颁发给谁
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// 密钥
        /// </summary>
        public string SecretKey { get; set; }
    }

    public class CookieSetting
    {
        /// <summary>
        /// CookieScheme
        /// </summary>
        public string CookieScheme { get; set; }
    }


    public class ImagePathSetting
    {
        /// <summary>
        /// 聊天图片保存路径
        /// </summary>
        public string ChatImg { get; set; }
    }
}
