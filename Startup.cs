using System;
using System.Collections.Generic;
using dotnet_new_angular.DataProtection;
using dotnet_new_angular.HelseId;
using Fhi.HelseId.Web;
using Fhi.HelseId.Web.ExtensionMethods;
using Fhi.HelseId.Web.Hpr;
using Fhi.HelseId.Web.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace dotnet_new_angular
{
    public class Startup
    {
        private readonly IConfigurationSection _helseIdConfigurationSection;
        private readonly IConfigurationSection _redirectPagesConfigurationSection;
        private readonly IConfigurationSection _hprConfigurationSection;
        private readonly IConfigurationSection _dataprotectionConfigSection;
        private readonly DemoHelseIdConfig _demoHelseIdConfiguration;
        private readonly RedirectPagesKonfigurasjon _redirectPagesConfiguration;
        private readonly HprKonfigurasjon _hprConfiguration;
        private readonly Whitelist _whitelist;
        private readonly DataProtectionConfig _dataProtectionConfig;

        private bool UseAuth => _demoHelseIdConfiguration.AuthUse;
        private bool UseHttps => _demoHelseIdConfiguration.UseHttps;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            _helseIdConfigurationSection = Configuration.GetSection(nameof(DemoHelseIdConfig));
            _demoHelseIdConfiguration = _helseIdConfigurationSection.Get<DemoHelseIdConfig>();

            _redirectPagesConfigurationSection = Configuration.GetSection(nameof(RedirectPagesKonfigurasjon));
            _redirectPagesConfiguration = _redirectPagesConfigurationSection.Get<RedirectPagesKonfigurasjon>();

            _hprConfigurationSection = Configuration.GetSection(nameof(HprKonfigurasjon));
            _hprConfiguration = _hprConfigurationSection.Get<HprKonfigurasjon>();

            _whitelist = Configuration.GetSection(nameof(WhitelistConfiguration)).Get<WhitelistConfiguration>().Whitelist;

            _dataprotectionConfigSection = Configuration.GetSection(nameof(DataProtectionConfig));
            _dataProtectionConfig = _dataprotectionConfigSection.Get<DataProtectionConfig>();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.Configure<DemoHelseIdConfig>(_helseIdConfigurationSection);
            services.Configure<RedirectPagesKonfigurasjon>(_redirectPagesConfigurationSection);
            services.Configure<HprKonfigurasjon>(_hprConfigurationSection);

            services.AddCors();

            // These are needed by the Fhi.HelseId package.
            services.AddSingleton<IWhitelist>(_whitelist);
            services.AddScoped<IGodkjenteHprKategoriListe, HprApprovals>();

            if (UseAuth)
            {
                services.AddHelseIdWebAuthentication(_demoHelseIdConfiguration, _redirectPagesConfiguration, _hprConfiguration, _whitelist);
            }
            else
            {
                services.AddControllersWithViews();
            }

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            // If you run on a webfarm you need to persist the keys protecting your cookies
            // Note that we do not encrypt the stored keys in this sample. For            
            // more information, see DataProtectionExtensions.cs.
            if (_dataProtectionConfig.Enabled)
            {
                services
                    .AddDataProtection()                    
                    .PersistKeysToSqlServer(_dataProtectionConfig.ConnectionString, _dataProtectionConfig.Schema, _dataProtectionConfig.TableName);
            }
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

            if (UseHttps)
            {
                app.UseHttpsRedirection();
            }

            app.UseStaticFiles();
            if (!env.IsDevelopment())
            { 
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
            );

            if (UseAuth)
            {
                app.UseAuthentication();
                app.UseAuthorization();
            }

            // Note: The path '/api/User/Logout' must be whitelisted.
            // This is to enable users that are logged in to HelseId, but without sufficient access, allowed to call this 
            // endpoint in order to logout. Endpoint is called from the Forbidden page.
            app.UseHelseIdProtectedPaths(_demoHelseIdConfiguration, _redirectPagesConfiguration,
                new List<PathString>
                {
                    "/assets/favicon.ico",
                    "/api/User/Logout"
                });
            app.UseEndpoints(endpoints => endpoints.MapControllers());

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
