using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using WebStore.DAL.Context;
using WebStore.Data;
using WebStore.Infrastructure.Conventions;
using WebStore.Services;
using WebStore.Services.InMemory;
using WebStore.Services.InSQL;
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
        public void ConfigureServices(IServiceCollection services) //здесь будут определены ВСЕ сервисы, которые нужны приложению, а еще здесь же добавляются базы данных
        {
            services.AddDbContext<WebStoreDB>(opt => opt.UseSqlServer(Configuration.GetConnectionString("MSSQL")));
            services.AddTransient<WebStoreDBInitializer>();
            //добавляем систему MVC и компиляцию на лету
            services.AddControllersWithViews(opt => opt.Conventions.Add(new TestControllerConvention())).AddRazorRuntimeCompilation();
            //Интерфейс сервиса IEmployeesData, он будет реализован в классе InMemoryEmployeesData
            services.AddSingleton<IEmployeesData, InMemoryEmployeesData>(); //Объект InMemoryEmployeesData создается один раз на всё время работы приложения,
                                                                            //нужен, если сервис будет хранить состояние на время работы приложения
            if (Configuration["ProductDataSource"] == "db")
                services.AddScoped<IProductData, SqlProductData>();
            else
                services.AddScoped<IProductData, InMemoryProductData>();
            //services.AddScoped<IEmployeesData, InMemoryEmployeesData>();  // Объект InMemoryEmployeesData создается один раз для области
            //services.AddTransient<IEmployeesData, InMemoryEmployeesData>();  //Объект InMemoryEmployeesData создается каждый раз заново
            //три варианта добавления сервисов           

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider services)
        {
            //var initializer = services.GetRequiredService<WebStoreDBInitializer>();
            //initializer.Initialize();
            using (var scope = services.CreateScope())
                scope.ServiceProvider.GetRequiredService<WebStoreDBInitializer>().Initialize(); //а можно и так сделать, через область с отдельным контекстом ДБ
            
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
                //дефолтные значения переменных: контроллер по умолчанию Home, действие по умолчанию Index, а переменной вообще может и не быть
                //либо то же самое делается вот так:
                //endpoints.MapDefaultControllerRoute();
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
    
}
