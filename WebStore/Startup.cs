using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebStore.Infrastructure.Conventions;
using WebStore.Services;
using WebStore.Services.Interfaces;

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
            //добавл€ем систему MVC и компил€цию на лету
            services.AddControllersWithViews(opt => opt.Conventions.Add(new TestControllerConvention())).AddRazorRuntimeCompilation();
            services.AddSingleton<IEmployeesData, InMemoryEmployeesData>(); //»нтерфейс сервиса IEmployeesData, он будет реализован в классе InMemoryEmployeesData

        }       
                
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseRouting();
            //app.UseMiddleware<TestMiddleWare>();
            app.UseWelcomePage("/WelcomePage");
                                  

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/greetings", async context =>
                {
                    await context.Response.WriteAsync(Configuration["Greetings"]);
                });
                //дефолтные значени€ переменных: контроллер по умолчанию Home, действие по умолчанию Index, а переменной вообще может и не быть
                //либо то же самое делаетс€ вот так:
                //endpoints.MapDefaultControllerRoute();
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
   
}
