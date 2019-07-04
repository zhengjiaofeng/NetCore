using LLSignalR.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace LLSignalR
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            #region 跨域请求配置
            services.AddCors(options => options.AddPolicy("CorsPolicy",
            builder =>
            {
                builder.AllowAnyMethod().AllowAnyHeader()
                       .WithOrigins("https://localhost:44303")
                       .AllowCredentials();
            }));
            #endregion

            services.AddSignalR();

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
