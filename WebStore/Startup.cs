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
        public void ConfigureServices(IServiceCollection services) //����� ����� ���������� ��� �������, ������� ����� ����������, � ��� ����� �� ����������� ���� ������
        {
            services.AddDbContext<WebStoreDB>(opt => opt.UseSqlServer(Configuration.GetConnectionString("MSSQL")));
            //��������� ������� MVC � ���������� �� ����
            services.AddControllersWithViews(opt => opt.Conventions.Add(new TestControllerConvention())).AddRazorRuntimeCompilation();
            //��������� ������� IEmployeesData, �� ����� ���������� � ������ InMemoryEmployeesData
            services.AddSingleton<IEmployeesData, InMemoryEmployeesData>(); //������ InMemoryEmployeesData ��������� ���� ��� �� �� ����� ������ ����������,
                                                                            //�����, ���� ������ ����� ������� ��������� �� ����� ������ ����������

            //services.AddScoped<IEmployeesData, InMemoryEmployeesData>();  // ������ InMemoryEmployeesData ��������� ���� ��� ��� �������
            //services.AddTransient<IEmployeesData, InMemoryEmployeesData>();  //������ InMemoryEmployeesData ��������� ������ ��� ������
            //��� �������� ���������� ��������
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
                //��������� �������� ����������: ���������� �� ��������� Home, �������� �� ��������� Index, � ���������� ������ ����� � �� ����
                //���� �� �� ����� �������� ��� ���:
                //endpoints.MapDefaultControllerRoute();
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
    
}
