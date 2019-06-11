using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace LL.Common.Middlewares
{

    /// <summary>
    /// 参数中间件
    /// </summary>
    public class ParameFilterMiddleware
    {
        private readonly RequestDelegate next;
        public ParameFilterMiddleware(RequestDelegate _next)
        {
            next = _next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // 调用管道中的下一个代理/中间件
            await next(context);
          
        }
    }
}
