using DesitfyMovies;

var builder = WebApplication.CreateBuilder(args);

var startup = Startup.InitializeFromBuilder(builder);
startup.ConfigureServices(builder.Services);
// Add services to the container.
//builder.Services.AddControllers();

//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

var app = builder.Build();
startup.Configure(app, builder.Environment);
app.Run();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//app.UseRouting();
//app.UseAuthorization();
//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllers();
//});

//app.MapControllers();

//app.Run();
