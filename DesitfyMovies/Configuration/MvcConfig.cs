using DesitfyMovies.Converters;
using DesitfyMovies.Filters;
using DestifyMovies.Core.Services.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace DesitfyMovies.Configuration
{
    public static class MvcConfig
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
                {
                    options.Filters.Add(typeof(RateLimiterFilter));
                })
                .ConfigureApiBehaviorOptions(options =>
                {
                    var badRequest = Response.BadRequest();
                    badRequest.Errors = new List<Error>
                    {
                        new Error {ErrorMessage = "There was an error with the request"}
                    };

                    options.InvalidModelStateResponseFactory = context => new BadRequestObjectResult(badRequest)
                        {StatusCode = 400};
                })
                .AddJsonOptions(opts =>
                {
                    opts.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
                });
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();
            app.UseDefaultFiles();

            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
