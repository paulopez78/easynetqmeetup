using Api.Db;
using EasyNetQ;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ApiTest
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = $@"
                host={Configuration["RABBIT_HOST"]};
                username={Configuration["RABBIT_USER"]};
                password={Configuration["RABBIT_PASSWORD"]}";

            services.AddEntityFrameworkInMemoryDatabase()
                    .AddDbContext<MeetupDbContext>();

            services.AddSingleton<IBus>(RabbitHutch.CreateBus(connectionString));

            services.AddMvcCore()
                    .AddApiExplorer()
                    .AddJsonFormatters();

            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();
            app.UseSwagger();            
            app.UseSwaggerUi();
        }
    }
}
