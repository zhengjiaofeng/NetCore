using LL.Models.ComomModel;
using LLSignalR.Hubs;
using LLSignalR.Models.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.Tasks;

namespace LLSignalR
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
            services.AddMvc();

            #region 初始化配置
            //初始化配置

            services.Configure<JWTSetting>(Configuration.GetSection("JWTSetting"));
            #endregion

            #region 读取配置
            string Issuer = Configuration.GetSection("JWTSetting").GetSection("Issuer").Value;
            string Audience = Configuration.GetSection("JWTSetting:Audience").Value;
            string SecretKey = Configuration.GetSection("JWTSetting:SecretKey").Value;
            #endregion

            #region 跨域请求配置
            services.AddCors(options => options.AddPolicy("CorsPolicy",
            builder =>
            {
                builder.AllowAnyMethod().AllowAnyHeader()
                       .WithOrigins("https://localhost:44303")
                       .AllowCredentials();
            }));
            #endregion

            #region SignalR
            services.AddSignalR()
            #region MessagePack   引用Microsoft.AspNetCore.SignalR.Protocols.MessagePack
                .AddMessagePackProtocol(options =>
            {
                //options.FormatterResolvers = new List<MessagePack.IFormatterResolver>()
                //{
                //     MessagePack.Resolvers.StandardResolver.Instance
                // };
            });
            #endregion
            #endregion

            #region jwt认证  由于cors 接受不到access_token 暂停 jwt 验证
            /*
             *1.0添加授权自定义策略 Hubs
             *2.0设置认证方式(cookie、bearer、openid)
             *3.0添加JWT认证机制
             *             
             */

            //1.0
            services.AddAuthorization(option =>
            {
                option.AddPolicy("Hubs", policy => policy.Requirements.Add(new JwtAuthorizationRequirement()));
            });

            //2.0
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            //3.0
            .AddJwtBearer(x => {
                x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
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

                //为JwtBearerEvents注册事件
                x.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"]; ;
                        var path = context.HttpContext.Request.Path;
                        if (!(string.IsNullOrWhiteSpace(accessToken))
                            && path.StartsWithSegments("/ChatHub"))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
                        //Token expired
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        else if (context.Exception.GetType() == typeof(SecurityTokenInvalidLifetimeException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            }
          
            );


            #endregion

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
            app.UseAuthentication();

            // Make sure the CORS middleware is ahead of SignalR.
            app.UseCors("CorsPolicy");
            app.UseSignalR(routes =>
            {
                //映射地址，供前端调用
                routes.MapHub<MessageHub>("/MessageHub");
                routes.MapHub<ChatHub>("/ChatHub");
            });
            #region 跨域中间件



            #endregion
            app.UseMvc(routes =>
            {
                routes.MapRoute(name: "default", template: "{controller=Chat}/{action=Index}/{id?}");

            });
        }
    }
}
