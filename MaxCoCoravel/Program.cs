using Coravel;
using MaxCoCoravel;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddScheduler();
        services.AddHostedService<Worker>();
        services.AddTransient<TryEmail>();
        services.AddQueue();
    })
    .Build();

await host.RunAsync();
