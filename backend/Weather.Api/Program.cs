using Weather.Application.Services;
using Weather.Application.Orchestrators;
using Weather.Infrastructure.DateParsing;
using Weather.Infrastructure.Storage;
using Weather.Infrastructure.WeatherApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});

builder.Services.AddHttpClient<IWeatherService, OpenMeteoClient>();

builder.Services.AddScoped<IDateReader, FileDateReader>();
builder.Services.AddScoped<IWeatherStorage, FileWeatherStorage>();
builder.Services.AddScoped<WeatherOrchestrator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAngular");
app.UseAuthorization();

app.MapControllers();

app.Run();