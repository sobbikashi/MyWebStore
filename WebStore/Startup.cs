using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using WebStore.DAL.Context;
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
        public void ConfigureServices(IServiceCollection services) //здесь будут определены ¬—≈ сервисы, которые нужны приложению, а еще здесь же добавл€ютс€ базы данных
        {
            services.AddDbContext<WebStoreDB>(opt => opt.UseSqlServer(Configuration.GetConnectionString("MSSQL")));
            //добавл€ем систему MVC и компил€цию на лету
            services.AddControllersWithViews(opt => opt.Conventions.Add(new TestControllerConvention())).AddRazorRuntimeCompilation();
            //»нтерфейс сервиса IEmployeesData, он будет реализован в классе InMemoryEmployeesData
            services.AddSingleton<IEmployeesData, InMemoryEmployeesData>(); //ќбъект InMemoryEmployeesData создаетс€ один раз на всЄ врем€ работы приложени€,
                                                                            //нужен, если сервис будет хранить состо€ние на врем€ работы приложени€

            //services.AddScoped<IEmployeesData, InMemoryEmployeesData>();  // ќбъект InMemoryEmployeesData создаетс€ один раз дл€ области
            //services.AddTransient<IEmployeesData, InMemoryEmployeesData>();  //ќбъект InMemoryEmployeesData создаетс€ каждый раз заново
            //три варианта добавлени€ сервисов
            //services.AddScoped<ITestService, TestService>();
            //services.AddScoped<IPrinter, DebugPrinter>();

            services.AddSingleton<IProductData, InMemoryProductData>();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider services)
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
