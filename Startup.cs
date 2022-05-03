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
using Microsoft.OpenApi.Models;
using SuvillianceSystem.Connections.Infrastructure;
using SuvillianceSystem.Connections.Concrete;
using Microsoft.AspNetCore.Identity;

namespace SuvillianceSystem.Authentication
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
            services.AddSingleton(typeof(IPasswordHasher<>),typeof(PasswordHasher<>));
            services.AddSingleton<IMongoSettings, MongoSettings>(service => this.ConfigureMongo(service));
            services.AddSingleton(typeof(ICRUD<>), typeof(MongoCRUD<>));
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SuvillianceSystem.Authentication", Version = "v1" });
            });
        }

        private MongoSettings ConfigureMongo(IServiceProvider service)
        {
            string database = Environment.GetEnvironmentVariable("DataBaseName");
            database = database == null ? this.Configuration["MongoDevSettings:DataBaseName"] : database;

            string host = Environment.GetEnvironmentVariable("Host");
            host = host == null ? this.Configuration["MongoDevSettings:Host"] : host;

            string password = Environment.GetEnvironmentVariable("Password");
            password = password == null ? this.Configuration["MongoDevSettings:Password"] : password;

            string port = Environment.GetEnvironmentVariable("Port");
            port = port == null ? this.Configuration["MongoDevSettings:Port"] : port;
            
            string userName = Environment.GetEnvironmentVariable("UserName");
            userName = userName == null ? this.Configuration["MongoDevSettings:UserName"] : userName;

            MongoSettings response = new MongoSettings()
            {
                DataBaseName = database,
                HostName = host,
                Password = password,
                Port = port,
                UserName = userName
            };


            System.Console.WriteLine(response.ComposeConnectionString());

            return response;

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SuvillianceSystem.Authentication v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
