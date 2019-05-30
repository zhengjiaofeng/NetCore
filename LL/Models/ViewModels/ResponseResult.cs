using System.Collections.Generic;

namespace LL.Models.ViewModels
{
    public class ResponseResult<T>
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int stateCode { get; set; }

        public bool isSucess { get; set; }
        public string msg { get; set; }

        public List<T> data { get; set; }
    }
}
