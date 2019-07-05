
using LL.Common.Handlers;
using LL.Common.Middlewares;
using LL.Models.ComomModel;
using LL.Models.Handlers;
using LLBLL.Common;
using LLBLL.IService;
using LLBLL.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NLog.Extensions.Logging;
using NLog.Web;
using System.Collections.Generic;
using System.Text;
using UEditor.Core;

namespace LL
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            #region 初始化配置
            //初始化配置
            services.Configure<LLSetting>(Configuration.GetSection("LLSetting"));
            services.Configure<JWTSetting>(Configuration.GetSection("JWTSetting"));
            services.Configure<ImagePathSetting>(Configuration.GetSection("ImagePathSetting"));
            #endregion


            #region ConfigureServices中读取配置
            string Issuer = Configuration.GetSection("JWTSetting").GetSection("Issuer").Value;
            string CookieScheme = Configuration.GetSection("CookieSetting").GetSection("CookieScheme").Value;

            string Audience = Configuration.GetSection("JWTSetting:Audience").Value;
            string SecretKey = Configuration.GetSection("JWTSetting:SecretKey").Value;
            #endregion

            #region Authentication cookie,JwtBearer 认证配置

            var jwtAuthorizationRequirement = new JwtAuthorizationRequirement();

            //添加认证Cookie信息
            services.AddAuthentication(CookieScheme) // Sets the default scheme to cookies
                         .AddCookie(CookieScheme, options =>
                         {
                             options.AccessDeniedPath = "/Account/Denied"; //禁止访问路径：当用户试图访问资源时，但未通过该资源的任何授权策略，请求将被重定向到这个相对路径（没有权限跳转）。
                             options.LoginPath = "/Account/Index";// 登录路径：这是当用户试图访问资源但未经过身份验证时，程序将会将请求重定向到这个相对路径。
                             //options.SlidingExpiration = true;  //Cookie可以分为永久性的和临时性的。 临时性的是指只在当前浏览器进程里有效，浏览器一旦关闭就失效（被浏览器删除）。 永久性的是指Cookie指定了一个过期时间，在这个时间到达之前，此cookie一直有效（浏览器一直记录着此cookie的存在）。 slidingExpriation的作用是，指示浏览器把cookie作为永久性cookie存储，但是会自动更改过期时间，以使用户不会在登录后并一直活动，但是一段时间后却自动注销。也就是说，你10点登录了，服务器端设置的TimeOut为30分钟，如果slidingExpriation为false,那么10:30以后，你就必须重新登录。如果为true的话，你10:16分时打开了一个新页面，服务器就会通知浏览器，把过期时间修改为10:46。 更详细的说明还是参考MSDN的文档。
                         });
            #region jwt 


            #region Jwt默认
            //.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, jwtoptions =>
            //{
            //    jwtoptions.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            //    {

            //        //Token颁发机构
            //        ValidIssuer = "https://localhost:44303/",
            //        //颁发给谁
            //        ValidAudience = "https://localhost:44303/",
            //        //这里的key要进行加密，需要引用Microsoft.IdentityModel.Tokens
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("LL_2AWopCMMgEIWt6KkzxEJD0EA4xreXLINaQIDAQABAoGAcUQIoKWyldZa8xnPDJTMKIV8GpeuzebKWvwp5dIu+miTdzmZX4weeHADRNb")),
            //        //验证token 有效期
            //        ValidateLifetime = true,
            //        //ValidateIssuer = true,
            //        //ValidateAudience = true,
            //        //ValidateIssuerSigningKey=true
            //        ///允许的服务器时间偏移量
            //        //ClockSkew = TimeSpan.Zero

            //    };
            //});
            #endregion

            #region Jwt自定义策略

            /*
             * 1.定义 policy
             * 2.定义 AuthorizationHandler 事件
             *             
             */

            services.AddAuthorization(option =>
            {
                option.AddPolicy("LL_Jwt", policy =>
                {
                    //参数约束
                    policy.AddRequirements(jwtAuthorizationRequirement);
                });

            }).AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    //Token颁发机构
                    ValidIssuer = Issuer,
                    //颁发给谁
                    ValidAudience = Audience,
                    //这里的key要进行加密，需要引用Microsoft.IdentityModel.Tokens
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey)),
                    //验证token 有效期
                    ValidateLifetime = true,
                    //ValidateIssuer = true,
                    //ValidateAudience = true,
                    ValidateIssuerSigningKey = true
                    ///允许的服务器时间偏移量
                    //ClockSkew = TimeSpan.Zero
                    
                };

            });


            #endregion

            #endregion


            #endregion

            #region  UEditor文本编辑器
            //services.AddUEditorService("ueditor_config.json", false,"/");
            services.AddUEditorService();
            #endregion

            #region 依赖注入 服务

            //注册EF上下文
            services.AddDbContext<LLDbContext>(options =>
            {
                options.UseSqlServer(
              //数据是sql server 2008  开启 UseRowNumberForPaging
              Configuration.GetConnectionString("LLDbContext"),b=>b.UseRowNumberForPaging());
            });

            // Transient： 每一次GetService都会创建一个新的实例
            // Scoped：  在同一个Scope内只初始化一个实例 ，可以理解为（ 每一个request级别只创建一个实例，同一个http request会在一个 scope内）
            // Singleton ：整个应用程序生命周期以内只创建一个实例
            services.AddTransient<IUsersService, UsersService>();

            //jwt 自定义验证AuthorizationHandler时间
            services.AddSingleton<IAuthorizationHandler, JwtAuthorizationHandler>();
            #endregion

            #region 初始化数据
            //获取注册的服务
            ServiceProvider serviceProvider = services.BuildServiceProvider();
            LLDbContext dbcontext = serviceProvider.GetService<LLDbContext>();
            DataInitialize.DataInit(dbcontext);
            #endregion

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);



            #region 数据保护组件
            //.NETCore 数据保护组件
            services.AddDataProtection();
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
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

            #region Nlog
            loggerFactory.AddNLog();
            env.ConfigureNLog("NLog.config");
            #endregion

            #region  中间件
            /*
             *1.app.Use()，IApplicationBuilder接口原生提供，注册等都用它。
             *2.app.Run() ，是一个扩展方法，它需要一个RequestDelegate委托，里面包含了Http的上下文信息，没有next参数，因为它总是在管道最后一步执行。
             *3.app.Map()，也是一个扩展方法，类似于MVC的路由，用途一般是一些特殊请求路径的处理。如：www.example.com/token 等。 映射中间件
             *4.app.UseMiddleware<>()
             */
            //方式1
            //app.UseMiddleware<NotFoundMiddleware>();

            //方式2
            app.UseLLNotFoundMiddleware();
            #endregion

            //中间件排序很重要--排序错误会导致中间件异常错误  https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/middleware/index?view=aspnetcore-2.1#order
            ///添加静态资源访问
            app.UseStaticFiles();

            //验证中间件--cookie身份认证  Must go before UseMvc
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(name: "default", template: "{controller=Account}/{action=Index}/{id?}");
            });

        }
    }
}
