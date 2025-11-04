using Tutorium.AuthService.Core.Models.Google;
using Tutorium.AuthService.Core.Services.Interfaces;
using Tutorium.AuthService.Core.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

ConfigureAppSettings(builder);
ConfigureServices(builder);

var app = builder.Build();

app.UseCors("AllowFrontend");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

#region Setup Helpers

void ConfigureAppSettings(WebApplicationBuilder builder)
{
    builder.Configuration
       .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
       .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);

    if (builder.Environment.IsDevelopment())
        builder.Configuration.AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true);
    
    builder.Configuration.AddEnvironmentVariables();
}

void ConfigureServices(WebApplicationBuilder builder)
{
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddHttpClient<IGoogleAuthService, GoogleAuthService>();

    builder.Services.Configure<GoogleOptions>(builder.Configuration.GetSection("Google"));
}

#endregion Setup Helpers

