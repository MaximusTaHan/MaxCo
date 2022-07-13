using MaxCoEmailService;
using Serilog;
using Serilog.Formatting.Json;


Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File(new JsonFormatter(), "logs/log.txt", 
        restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
        rollingInterval: RollingInterval.Day)
    .WriteTo.File("logs/errorlog.txt", restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Warning)
    .CreateLogger();



try
{
    Log.Information("Starting our service..");
    IHost host = Host.CreateDefaultBuilder(args)
        .UseSerilog()
        .ConfigureServices(services =>
        {
            services.AddHostedService<ProcessOrder>();
            services.AddScoped<IProcessOrder, DefaultProcessOrder>();
        })
        .Build();


    await host.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Exception in application");
}
finally
{
    Log.Information("Exiting service");
    Log.CloseAndFlush();
}


