using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using aspCart.Infrastructure;
using aspCart.Infrastructure.EFModels;
using aspCart.Web.Helpers;
using aspCart.Web.Models;
using aspCart.Infrastructure.EFRepository;
using aspCart.Core.Interface.Services.Catalog;
using aspCart.Infrastructure.Services.Catalog;
using AutoMapper;
using aspCart.Web.Areas.Admin.Helpers;
using aspCart.Core.Interface.Services.User;
using aspCart.Infrastructure.Services.User;
using aspCart.Core.Interface.Services.Sale;
using aspCart.Infrastructure.Services.Sale;
using Microsoft.AspNetCore.Rewrite;
using aspCart.Core.Interface.Services.Statistics;
using aspCart.Infrastructure.Services.Statistics;
using aspCart.Web.Middleware;
using aspCart.Core.Interface.Services.Messages;
using aspCart.Infrastructure.Services.Messages;
using Microsoft.Extensions.FileProviders;

namespace aspCart.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets("aspnet-aspCart.Web-b7b6c0c8-2794-41a1-ad6c-528772b97f8a");
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
            HostingEnvironment = env;

            MapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfileConfiguration());
            });
        }

        public IConfigurationRoot Configuration { get; }
        public MapperConfiguration MapperConfiguration { get; set; }
        private IHostingEnvironment HostingEnvironment;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // idenity password requirement
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            });

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // configure admin account injectable
            services.Configure<AdminAccount>(
                Configuration.GetSection("AdminAccount"));

            services.Configure<UserAccount>(
                Configuration.GetSection("UserAccount"));

            services.AddMvc();
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(60);
                options.CookieName = "aspCart";
            });


            // Add application services.
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<IBillingAddressService, BillingAddressService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IImageManagerService, ImageManagerService>();
            services.AddTransient<IManufacturerService, ManufacturerService>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IReviewService, ReviewService>();
            services.AddTransient<ISpecificationService, SpecificationService>();

            services.AddTransient<IOrderCountService, OrderCountService>();
            services.AddTransient<IVisitorCountService, VisitorCountService>();

            services.AddTransient<IContactUsService, ContactUsService>();

            // singleton
            services.AddSingleton(sp => MapperConfiguration.CreateMapper());
            services.AddSingleton<ViewHelper>();
            services.AddSingleton<DataHelper>();
            services.AddSingleton<IFileProvider>(HostingEnvironment.ContentRootFileProvider);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");

                // redirect http request to https with 301 status code
                var options = new RewriteOptions().AddRedirectToHttpsPermanent();
                app.UseRewriter(options);
            }

            app.UseImageResize();
            app.UseStaticFiles();
            app.UseStatusCodePages();
            app.UseIdentity();
            app.UseSession();
            app.UseVisitorCounter();

            // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areaRoute",
                    template: "{area:exists}/{controller}/{action}/{id?}",
                    defaults: new { controller = "Dashboard", action = "Index" });

                routes.MapRoute(
                    name: "productInfo",
                    template: "Product/{seo}",
                    defaults: new { controller = "Home", action = "ProductInfo" });

                routes.MapRoute(
                    name: "category",
                    template: "Category/{category}",
                    defaults: new { controller = "Home", action = "ProductCategory" });

                routes.MapRoute(
                    name: "manufacturer",
                    template: "Manufacturer/{manufacturer}",
                    defaults: new { controller = "Home", action = "ProductManufacturer" });

                routes.MapRoute(
                    name: "productSearch",
                    template: "search/{name?}",
                    defaults: new { controller = "Home", action = "ProductSearch" });

                routes.MapRoute(
                    name: "create review",
                    template: "CreateReview/{id}",
                    defaults: new { controller = "Home", action = "CreateReview" });

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            // apply migration
            SampleDataProvider.ApplyMigration(app.ApplicationServices);

            // seed default data
            SampleDataProvider.Seed(app.ApplicationServices, Configuration);
        }
    }
}
