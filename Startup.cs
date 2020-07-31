using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookTracker.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Caching.Distributed;
namespace BookTracker
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
            services.AddControllersWithViews();
           

            //    services.AddDbContext<MvcBookTrackerContext>(options =>
            //        options.UseSqlServer(Configuration.GetConnectionString("MvcBookTrackerContext")));

            services.AddDbContext<BookTrackerContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("BookTrackerContext")));

            //TempDataProvider
            services.AddControllersWithViews()
            .AddSessionStateTempDataProvider();
            services.AddRazorPages()
                .AddSessionStateTempDataProvider();
            services.AddSession();

            //Session
            //services.AddDistributedMemoryCache();

            //services.AddSession(options =>
            //{
            //    options.IdleTimeout = TimeSpan.FromSeconds(10);
            //    options.Cookie.HttpOnly = true;
            //    options.Cookie.IsEssential = true;
            //});

            //services.AddMvc();
            //services.AddSession(options => {
            //    options.IdleTimeout = TimeSpan.FromMinutes(30);
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.UseSession();
            //app.UseMvc();
            //app.Run(context => {
            //    return context.Response.WriteAsync("Hello Readers!");
            //});
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


            app.UseSession();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Books}/{action=Index}/{id?}");
            });
        }
    }
}
