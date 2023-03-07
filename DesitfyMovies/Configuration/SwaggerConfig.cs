using Microsoft.AspNetCore.Rewrite;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace DesitfyMovies.Configuration;
public static class SwaggerConfig
{
    public static void ConfigureServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Contact = new OpenApiContact
                {
                    Name = "Nathan J. Murphy",
                    Email = "nmurphy@professional@gmail.com"
                },
                Description = "This API is developed for the .Net Coding Challenger at Destify",
                Title = "Destify Movies API",
                Version = "v1"
            });

            options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme()
            {
                Description = "X-API-KEY header authorization",
                Name = "X-API-KEY",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "ApiKeyScheme"
            });

            var key = new OpenApiSecurityScheme()
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                },
                In = ParameterLocation.Header
            };

            var requirement = new OpenApiSecurityRequirement
            {
                {key, new List<string>()}
            };

            options.AddSecurityRequirement(requirement);
        });
    }
    public static void Configure(IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.RoutePrefix = "docs";
            options.DocExpansion(DocExpansion.None);

            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Destify Movies API");
        });

        app.UseRewriter(new RewriteOptions().AddRedirect("^$", "docs"));
    }
}


