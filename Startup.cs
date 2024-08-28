using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Repositories;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DB;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using WebApplication1.Models;
using Hangfire;
using WebApplication1.HangfireInterfacesAndRepositories;
using WebApplication1.Hubs;
using WebApplication1.Security;

namespace WebApplication1
{
    public class Startup
    {
        private IConfiguration _config;
      
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        
        public void ConfigureServices(IServiceCollection services)
        {
            // Read the connection string from the configuration
            var connectionString = Configuration.GetConnectionString("EmployeeDBConnection");

            services.AddDbContextPool<AppDBContext>(options => options.UseSqlServer(connectionString));
            services.AddControllersWithViews();
            //Adding repository and interface
            services.AddScoped<IEmployeeRepository, SQLEmployeeRepository>();
            // Register IEmailService and its implementation
            services.AddTransient<IEmailService, EmailService>();
            // For Identity Framework
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                // Customize Password
                options.Password.RequiredLength = 3;
                options.Password.RequireUppercase = false;
            }).AddEntityFrameworkStores<AppDBContext> ().AddDefaultTokenProviders();

            // authentication by google
            services.AddAuthentication().AddGoogle(options =>
            {
                options.ClientId = "43656266462-pp7u9u7g3nkpuakh587bmpl91hn2rcnt.apps.googleusercontent.com";
                options.ClientSecret = "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";
            });

            //Hangfire...
            services.AddHangfire((sp, config) =>
            {
                var connectionString = sp.GetRequiredService<IConfiguration>().GetConnectionString("EmployeeDBConnection");
                config.UseSqlServerStorage(connectionString);
            });

            // Customize Access Denied Url...
            //services.ConfigureApplicationCookie(options =>
            //{
            //    options.AccessDeniedPath = new PathString("/Administration/AccessDenied");
            //});

            // claims based authorization
            services.AddAuthorization(options =>
            {
                options.AddPolicy("DeleteRolePolicy", policy => policy.RequireClaim("Delete Role", "true")
                                                                      .RequireClaim("Create Role", "true"));

                //options.AddPolicy("EditRolePolicy", policy => policy.RequireClaim("Edit Role", "true"));

                //custom authorization policy using func...
                options.AddPolicy("EditRolePolicy", policy => policy.RequireAssertion(context =>
                    context.User.IsInRole("admin") &&
                    context.User.HasClaim(claim => claim.Type == "Edit Role" && claim.Value == "true") ||
                    context.User.IsInRole("superadmin")
                    ));

                //custom authorization requirement and handler...
                options.AddPolicy("ManageRolesPolicy", policy => policy.AddRequirements(new ManageAdminRolesAndClaimsRequirement()));

                // Policy role based authorization
                options.AddPolicy("AdminRolePolicy", policy => policy.RequireRole("admin"));
            });

            services.AddSingleton<IAuthorizationHandler, CanEditOnlyOtherAdminRolesAndClaimsHandler>();
            services.AddSingleton<IAuthorizationHandler, SuperAdminHandler>();

            // its for global authorization
            //services.AddMvc(options =>
            //{
            //    var policy = new AuthorizationPolicyBuilder()
            //                     .RequireAuthenticatedUser()
            //                     .Build();
            //    options.Filters.Add(new AuthorizeFilter(policy));
            //}).AddXmlSerializerFormatters();

            //MvcOptions.EnableEndPointRouting = false;

            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                DeveloperExceptionPageOptions developerExceptionPageOptions = new DeveloperExceptionPageOptions
                {
                    SourceCodeLineCount = 10
                };
                app.UseDeveloperExceptionPage(developerExceptionPageOptions);
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
                //app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();

            app.UseFileServer();

            //app.UseMvcWithDefaultRoute();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseHangfireDashboard();
            //app.UseMvcWithDefaultRoute();

            //ConventionalRouting
            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute("Default", "{controller=Home}/{action=AllEmployees/{id?}");
            //});

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Employee}/{action=Home}/{id?}");

                //endpoints.MapHub<UserHub>("/userHub");
               
               
                endpoints.MapHub<OnlineUsersHub>("/onlineUsersHub");
            });

            //app.Run(async(context) =>
            //{
            //    //throw new Exception("Some error passing the request...");
            //    await context.Response.WriteAsync("Hello World!");
            //}); https://kenhaggerty.com/articles/article/aspnet-core-50-online-user-count-with-signalr
        }
    }
}
