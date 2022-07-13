using Coravel.Queuing.Interfaces;

namespace MaxCoCoravel
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IQueue _queue;
        public Worker(ILogger<Worker> logger, IQueue queue)
        {
            _logger = logger;
            _queue = queue;

            _queue.QueueInvocable<TryEmail>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}