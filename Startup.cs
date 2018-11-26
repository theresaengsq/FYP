using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using P06.Models;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;

namespace P06
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            RotativaConfiguration.Setup(env);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddDbContext<AppDbContext>(
                  options =>
                     options
                        .UseSqlServer(
                             Configuration.GetConnectionString("DefaultConnection")));
            services
                  .AddAuthentication("UserSecurity")
                  .AddCookie("UserSecurity",
                     options =>
                     {
                         options.LoginPath = "/Account/Login/";
                         options.AccessDeniedPath = "/Account/Forbidden/";
                     });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc(
               routes =>
               {
                   routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
               });

        }
    }
}

