using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WebStore
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration Configuration)
        {
            this.Configuration = Configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            //добавляем систему MVC и компиляцию на лету
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            
        }
                
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
                        
            app.UseRouting();
                                  

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/greetings", async context =>
                {
                    await context.Response.WriteAsync(Configuration["Greetings"]);
                });
                //дефолтные значения переменных: контроллер по умолчанию Home, действие по умолчанию Index, а переменной вообще может и не быть
                //либо то же самое делается вот так:
                //endpoints.MapDefaultControllerRoute();
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
