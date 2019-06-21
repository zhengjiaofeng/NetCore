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
            app.UseSignalR(routes =>
            {
                //映射地址，供前端调用
                routes.MapHub<MessageHub>("/MessageHub");
            });
            app.UseMvc(routes =>
            {
                routes.MapRoute(name: "default", template: "{controller=SignalR}/{action=Index}/{id?}");
            });
        }
    }
}
