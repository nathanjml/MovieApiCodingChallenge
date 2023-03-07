using DesitfyMovies;

var builder = WebApplication.CreateBuilder(args);

var startup = Startup.InitializeFromBuilder(builder);
startup.ConfigureServices(builder.Services);

var app = builder.Build();

startup.Configure(app, builder.Environment);
app.Run();

