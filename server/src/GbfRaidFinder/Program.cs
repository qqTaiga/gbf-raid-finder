using GbfRaidFinder.Models.Settings;
using GbfRaidFinder.Services;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting webapp");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Create serilog
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services));

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

    app.UseSerilogRequestLogging();

    // app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Stopped webapp");
    Log.CloseAndFlush();
}
