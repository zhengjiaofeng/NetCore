using System.Collections.Generic;

namespace LL.Models.ViewModels
{
    public class ResponseResult<T>
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int StateCode { get; set; }

        public bool IsSucess { get; set; }
        public string Msg { get; set; }

        public List<T> Data { get; set; }
    }
}
