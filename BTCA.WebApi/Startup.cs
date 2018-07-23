using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using IdentityServer4.AccessTokenValidation;
using BTCA.DomainLayer.Managers.Interface;
using BTCA.DomainLayer.Managers.Implementation;
using BTCA.DataAccess.Core;
using BTCA.DataAccess.EF;

namespace BTCA.WebApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore()
                .AddAuthorization()
                .AddJsonFormatters();

            // services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
            //     .AddIdentityServerAuthentication(options =>
            //     {
            //         options.Authority = "http://localhost:50011";
            //         options.RequireHttpsMetadata = false;
            //         options.ApiName = "scope.fullaccess";
            //         options.ApiSecret = "Info99Gum";
            //     });

            services.AddDbContext<HOSContext>(options => 
                options.UseSqlServer(Configuration.GetConnectionString("HOS")));

            services.AddScoped<IRepository, Repository>();
            services.AddScoped<ICompanyManager, CompanyManager>();
            services.AddScoped<ICompanyAddressManager, CompanyAddressManager>();
            services.AddScoped<IStateProvinceCodeManager, StateProvinceCodeManager>();
            services.AddScoped<ICompanyUserManager, CompanyUserManager>();                
            services.AddScoped<IDailyLogManager, DailyLogManager>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseStatusCodePages();
            //app.UseAuthentication();            
            app.UseMvc();
        }
    }
}
