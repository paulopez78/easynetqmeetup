using Api.Db;
using EasyNetQ;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;

namespace ApiTest
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
            var connectionString = $@"
                host={Configuration["RABBIT_HOST"]};
                username={Configuration["RABBIT_USER"]};
                password={Configuration["RABBIT_PASSWORD"]}";

            services.AddEntityFrameworkInMemoryDatabase()
                    .AddDbContext<MeetupDbContext>();

            services.AddSingleton<IBus>(RabbitHutch.CreateBus(connectionString));

            services.AddMvc();
            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new Info { Title = "meetup API", Version = "v1" }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseMvc();
            app.UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "meetup API v.1");
                });
        }
    }
}
