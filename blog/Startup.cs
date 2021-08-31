using JsonApiDotNetCore.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using blog.dto;
using blog.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;

namespace blog {
    public class Startup {
        private IConfigurationRoot Config;

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
        public void ConfigureServices(IServiceCollection services) {
            services.AddScoped<BlogDBContext>();
            services.AddControllers();
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "test", Version = "v1"}); });
            services.AddJsonApi<BlogDBContext>(opt =>
            {
                opt.Namespace = "api/v1";
                opt.IncludeTotalResourceCount = true;
            });
            services.AddDbContext<BlogDBContext>(options => {
                    #if DEBUG
                        options.EnableSensitiveDataLogging();
                        options.EnableDetailedErrors();
                    #endif
                });
            services.AddDefaultIdentity<User>()
                .AddUserStore<BlogDBContext>()  
                .AddDefaultTokenProviders();
            var identityUrl = Config.GetValue<string>("IdentityUrl");
            var callBackUrl = Config.GetValue<string>("CallBackUrl");
            var sessionCookieLifetime = Config.GetValue("SessionCookieLifetimeMinutes", 60);

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddCookie(setup => setup.ExpireTimeSpan = TimeSpan.FromMinutes(sessionCookieLifetime))
            .AddOpenIdConnect(options =>
            {
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.Authority = identityUrl.ToString();
                options.SignedOutRedirectUri = callBackUrl.ToString();
                options.ClientId = "mvc";
                options.ClientSecret = "secret";
                options.ResponseType = "code id_token";
                options.SaveTokens = true;
                options.GetClaimsFromUserInfoEndpoint = true;
                options.RequireHttpsMetadata = false;
                options.Scope.Add("openid");
                options.Scope.Add("profile");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            using (IServiceScope scope = app.ApplicationServices.CreateScope()) {
                var appDbContext = scope.ServiceProvider.GetRequiredService<BlogDBContext>();
                appDbContext.Database.Migrate();
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

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseRouting();
            app.UseJsonApi();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
