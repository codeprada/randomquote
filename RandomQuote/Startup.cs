using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RandomQuote.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

namespace RandomQuote
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApp(options =>
                {
                    options.Instance = Configuration["Instance"];
                    options.Domain = Configuration["Domain"];
                    options.TenantId = Configuration["TenantId"];
                    options.ClientId = Configuration["ClientId"];
                    options.ClientSecret = Configuration["ClientSecret"];
                    options.CallbackPath = Configuration["CallbackPath"];
                    
                })
                .EnableTokenAcquisitionToCallDownstreamApi(new[] { "user.readwrite.all" })
                .AddInMemoryTokenCaches();

            services.AddControllersWithViews(options =>
            {
                AuthorizationPolicy policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

                options.Filters.Add(new AuthorizeFilter(policy));
            }).AddMicrosoftIdentityUI();

            services.AddAuthorization(options =>
            {
                // By default, all incoming requests will be authorized according to the default policy
                options.FallbackPolicy = options.DefaultPolicy;
            });

            services.AddRazorPages()
                .AddMicrosoftIdentityUI();
            services.AddServerSideBlazor();
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
