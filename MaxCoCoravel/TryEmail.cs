using Coravel.Invocable;
using Coravel.Queuing.Interfaces;

public class TryEmail : IInvocable
{
    private readonly ILogger _logger;
    public TryEmail(ILogger<TryEmail> logger)
    {
        _logger = logger;
    }

    public Task Invoke()
    {
        Console.WriteLine("We in here!!!!!!!!! :D");
        _logger.LogInformation($"Starting job. Time: {DateTime.Now}");
        return Task.CompletedTask;
    }
}