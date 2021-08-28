using JsonApiDotNetCore.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using blog.dto;
using blog.Database;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace blog
{
    public class Startup
    {
        public IConfigurationRoot Config { get; private set; }

        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Config = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "test", Version = "v1" }); });
            services.AddScoped<BlogDBContext>();
            services.AddSingleton<ILoggingBuilder>(loggingBuilder => loggingBuilder.GetRequiredService<ILoggingBuilder>());
            services.AddJsonApi<BlogDBContext>(opt =>
            {
                opt.Namespace = "api/v1";
                opt.IncludeTotalResourceCount = true;
            });
            services.AddDbContext<BlogDBContext>(options =>
            {
#if DEBUG
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
#endif
            });
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConfiguration(Config.GetSection("Logging"));
                loggingBuilder.AddConsole();
                loggingBuilder.AddDebug();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggingBuilder loggerBuilder, BlogDBContext context)
        {
            using (IServiceScope scope = app.ApplicationServices.CreateScope())
            {
                context.Database.EnsureCreated();
                BlogEntry entity = new BlogEntry()
                {
                    Content = "Hello World",
                    User = new User()
                    {
                        Username = "testuser",
                        Password = "Password1",
                        EMail = "testuser@example.com"
                    }
                };
                context.Add(entity);
                context.SaveChanges();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "test v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseJsonApi();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
