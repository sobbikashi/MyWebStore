using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
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

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider services)
        {
            //var test_service = services.GetRequiredService<ITestService>();

            //test_service.Test();
            
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

    //interface ITestService
    //{
    //    void Test();
    //}
    //class TestService : ITestService
    //{
        
    //    private IPrinter _Printer;
    //    public TestService(IPrinter Printer) {_Printer = Printer;}

    //    public IPrinter Printer { get; }

    //    public void Test()
    //    {
    //        _Printer.Print("������ �����");
    //    }
    //}

    //interface IPrinter
    //{
    //    void Print(string str);
    //}

    //class DebugPrinter : IPrinter
    //{
    //    public void Print(string str)
    //    {
    //        Debug.WriteLine(str);
    //    }
    //}
}
