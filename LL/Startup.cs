﻿using LL.Models.ComomModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace LL
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        public const string CookieScheme = "LLCoreCookie1";
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //添加认证Cookie信息
            services.AddAuthentication(CookieScheme) // Sets the default scheme to cookies
                         .AddCookie(CookieScheme, options =>
                         {
                             options.AccessDeniedPath = "/Account/Denied"; //禁止访问路径：当用户试图访问资源时，但未通过该资源的任何授权策略，请求将被重定向到这个相对路径（没有权限跳转）。
                             options.LoginPath ="/Account/Index";// 登录路径：这是当用户试图访问资源但未经过身份验证时，程序将会将请求重定向到这个相对路径。
                                                                                                            //options.SlidingExpiration = true;  //Cookie可以分为永久性的和临时性的。 临时性的是指只在当前浏览器进程里有效，浏览器一旦关闭就失效（被浏览器删除）。 永久性的是指Cookie指定了一个过期时间，在这个时间到达之前，此cookie一直有效（浏览器一直记录着此cookie的存在）。 slidingExpriation的作用是，指示浏览器把cookie作为永久性cookie存储，但是会自动更改过期时间，以使用户不会在登录后并一直活动，但是一段时间后却自动注销。也就是说，你10点登录了，服务器端设置的TimeOut为30分钟，如果slidingExpriation为false,那么10:30以后，你就必须重新登录。如果为true的话，你10:16分时打开了一个新页面，服务器就会通知浏览器，把过期时间修改为10:46。 更详细的说明还是参考MSDN的文档。

                         });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            //初始化配置
            services.Configure<LLSetting>(Configuration.GetSection("LLSetting"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            ///添加静态资源访问
            app.UseStaticFiles();

            //验证中间件--cookie身份认证
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(name: "default", template: "{controller=Account}/{action=Index}/{id?}");
            });
        }
    }
}
