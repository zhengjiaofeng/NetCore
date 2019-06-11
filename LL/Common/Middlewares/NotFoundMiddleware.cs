using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LL.Common.Middlewares
{
    public class NotFoundMiddleware
    {
        private readonly RequestDelegate next;
        public NotFoundMiddleware(RequestDelegate _next)
        {
            next = _next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            // 调用管道中的下一个代理/中间件
            await next(context);

            if (context.Response.StatusCode == 404)
            {
                context.Response.Redirect("/Home/error");
                return;
            }
        }
    }
}
