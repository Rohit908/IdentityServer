using ApiOne.CustomPolicyProvider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiOne
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication("Bearer")
               .AddJwtBearer("Bearer", config =>
               {
                   config.Authority = "https://localhost:44373/";
                   config.Audience = "ApiOne";
                   config.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateAudience = false
                   };
               });


            //services.AddAuthentication(config =>
            //{
            //    config.DefaultScheme = "Cookie";
            //    config.DefaultChallengeScheme = "oidc";
            //})
            //    .AddCookie("Cookie")
            //    .AddOpenIdConnect("oidc", config =>
            //    {
            //        config.Authority = "https://localhost:44373/";
            //        config.ClientId = "client_id";
            //        config.ClientSecret = "client_secret";
            //        config.SaveTokens = true;

            //        config.ResponseType = "code";

            //        config.Scope.Add("Permission");
            //    });

            services.AddHttpClient();

            services.AddSingleton<IAuthorizationPolicyProvider, CustomAuthorizationPolicyProvider>();

            services.AddControllers();

            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Version = "V1",
                    Title = "API",
                    Description = "API"
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {

                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                    c.RoutePrefix = string.Empty;
                });

                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
