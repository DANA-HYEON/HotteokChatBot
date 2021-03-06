using HotteokChatBot.Controllers;
using HotteokChatBot.Data;
using HotteokChatBot.Models;
using HotteokChatBot.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotteokChatBot
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 21));

            services.AddDbContext<HotteokDbContext>(options =>
            options.UseMySql(Configuration.GetConnectionString("HotteokDbContext"), serverVersion, mySqlOptionsAction: sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null);
            })
            .EnableSensitiveDataLogging() //응용 프로그램 데이터를 예외 메시지, 로깅 등에 포함할 수 있도록 함
            .EnableDetailedErrors() //저장소 쿼리 결과를 처리 하는 동안 발생 하는 데이터 값 예외를 처리할 때 자세한 오류를 사용)
            );

            string Client_Id = Configuration.GetValue<string>("BotSetting:Client_Id");
            string Client_Secret = Configuration.GetValue<string>("BotSetting:Client_Secret");
            string Redirect_Uri = Configuration.GetValue<string>("BotSetting:Redirect_Uri");
            string Scope = Configuration.GetValue<string>("BotSetting:Scope");

            services.Add(new ServiceDescriptor(typeof(OauthLogin), new OauthLogin(Client_Id, Client_Secret, Redirect_Uri, Scope)));
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
