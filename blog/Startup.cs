using JsonApiDotNetCore.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using blog.dto;
using blog.Database;

namespace blog {
    public class Startup {


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddControllers();
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "test", Version = "v1"}); });
            services.AddJsonApi(options => options.Namespace = "api/v1", resources: builder => builder.Add<BlogEntry>("blogEntries"));
            services.AddDbContext<BlogDBContext>(options => {
                    #if DEBUG
                        options.EnableSensitiveDataLogging();
                        options.EnableDetailedErrors();
                    #endif
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            using (IServiceScope scope = app.ApplicationServices.CreateScope()) {
                var appDbContext = scope.ServiceProvider.GetRequiredService<BlogDBContext>();
                appDbContext.Database.EnsureCreated();
                BlogEntry entity = new BlogEntry() {
                    Content = "Hello World",
                    User = new User() {
                        Username = "testuser",
                        Password = "Password1",
                        EMail = "testuser@example.com"
                    }
                };
                appDbContext.Add(entity);
                appDbContext.SaveChanges();
            }

            if (env.IsDevelopment()) {
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
