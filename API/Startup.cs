using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Core.Interfaces;
using API.Helpers;
using AutoMapper;
using API.Middleware;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using Infrastructure.Identity;
using API.Extensions;

namespace API
{
    public class Startup
    {
        private readonly IConfiguration _config;
        public Startup(IConfiguration config)
        {
            _config = config;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationServices();
            
            services.AddControllers();
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddDbContext<StoreContext>(x => 
                    x.UseSqlite(_config.GetConnectionString("DefaultConnection")));

            //adding Dbcontext and db for identity
            services.AddDbContext<AppIdentityDbContext>(x =>
                    x.UseSqlite(_config.GetConnectionString("IdentityConnection"))
            );

            //adding identity services
            services.AddIdentityServices(_config);

            //config Redis 
            services.AddScoped<IConnectionMultiplexer>(c => {
                var configuration = ConfigurationOptions.Parse(_config.GetConnectionString("Redis"),
                true);
                return ConnectionMultiplexer.Connect(configuration);
            });
            
            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy",policy => 
                {
                    policy.AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithOrigins("https://localhost:4200");
                });
            });
            services.AddSwaggerGen(c => {
                c.SwaggerDoc
                ("v1", new OpenApiInfo{Title =  "SkiNet API" , Version = "v1"} );
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
           app.UseMiddleware<ExceptionMiddleware>();

            app.UseHttpsRedirection();

            app.UseCors("CorsPolicy");

            app.UseRouting();
            
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c => 
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SkiNet API v1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
