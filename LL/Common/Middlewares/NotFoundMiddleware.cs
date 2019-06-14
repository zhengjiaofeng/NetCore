using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace LL.Common.Middlewares
{
    public class NotFoundMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<NotFoundMiddleware> logger;
        public NotFoundMiddleware(RequestDelegate _next, ILogger<NotFoundMiddleware> _logger)
        {
            next = _next;
            logger = _logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            // 调用管道中的下一个代理/中间件
            await next(context);

            try
            {
                if (context.Response.StatusCode == 404)
                {
                    context.Response.Redirect("/Home/error");
                    return;
                }

            }
            catch (Exception ex)
            {
                logger.LogError("NotFoundMiddleware ex:" + ex.ToString());
            }

        }
    }
}
