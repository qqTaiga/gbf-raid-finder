using GbfRaidFinder.Models.Settings;
using GbfRaidFinder.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<Urls>(builder.Configuration.GetSection(nameof(Urls)));
builder.Services.Configure<Keys>(builder.Configuration.GetSection(nameof(Keys)));
builder.Services.AddTransient<ITwitterFilteredStreamService, TwitterFilteredStreamService>();
builder.Services.AddHttpClient();
builder.Services.AddControllers();
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

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
