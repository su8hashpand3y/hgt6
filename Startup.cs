using System.Text;
using Amazon.S3;
using HGT6.Helpers;
using HGT6.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Amazon.Extensions.NETCore.Setup;
using Amazon;
using Amazon.Runtime;
using System;
using System.Linq;
using System.Collections.Generic;

namespace HGT6
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            ConfigurationRoot = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .AddEnvironmentVariables()
               .Build();
        }

        public IConfigurationRoot ConfigurationRoot { get; }


        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var MYPS = $"User ID=hgtadmin;Password={Environment.GetEnvironmentVariable("CPASS")};Host=hgtinstance.csftqsjshidx.ap-south-1.rds.amazonaws.com;Port=5432;Database=hgtDB;Pooling=true;";

            services.AddDbContext<HGTDbContext>(options =>
              //options.UseSqlServer(Configuration.GetConnectionString("HGTDB")));
              // options.UseSqlServer(Configuration.GetConnectionString("AWSSQL")));
              options.UseNpgsql(MYPS));
             // options.UseNpgsql(Configuration.GetConnectionString("MYPSLocal")));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
       .AddJwtBearer(options =>
       {
           options.TokenValidationParameters = new TokenValidationParameters
           {
               ValidateIssuer = true,
               ValidateAudience = true,
               ValidateLifetime = true,
               ValidateIssuerSigningKey = true,
               ValidIssuer = Configuration["ValidIssuer"],
               ValidAudience = Configuration["ValidAudience"],
               IssuerSigningKey = new SymmetricSecurityKey(
                   Encoding.UTF8.GetBytes(Configuration["SecurityKey"]))
           };
       });

            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddCors();
            services.AddMvc();

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            services.AddDefaultAWSOptions(ConfigurationRoot.GetAWSOptions());
            //AWSOptions awsOption = new AWSOptions();
            //awsOption.Region = RegionEndpoint.APSoutheast1;
            //awsOption.Credentials = new AWSCredentials();
            services.AddAWSService<IAmazonS3>();



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                // app.UseDeveloperExceptionPage();
                app.UseExceptionHandler("/Home/Error");
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}");
            });


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
