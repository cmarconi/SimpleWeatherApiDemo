using Microsoft.Extensions.Options;
using SimpleWeatherApiDemo.ExternalServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddOptions<WeatherDotGovOptions>()
    .Bind(builder.Configuration.GetSection(WeatherDotGovOptions.ConfigurationSectionName))
    .ValidateDataAnnotations();

builder.Services.AddHttpClient<WeatherDotGovClient>((services, client) =>
{
    var options = services.GetRequiredService<IOptions<WeatherDotGovOptions>>();
    client.BaseAddress = new Uri(options.Value.BaseUrl);
    client.DefaultRequestHeaders.Add("Accept", "application/ld+json");
    client.DefaultRequestHeaders.Add("User-Agent", options.Value.UserAgent);
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
