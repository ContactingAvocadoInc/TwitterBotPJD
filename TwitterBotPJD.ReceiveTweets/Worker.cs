using Tweetinvi;

namespace TwitterBotPJD.ReceiveTweets;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly ITwitterClient _twitterClient;

    public Worker(
        ILogger<Worker> logger,
        ITwitterClient twitterClient)
    {
        _logger = logger;
        _twitterClient = twitterClient;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var user = await _twitterClient.UsersV2.GetUserByNameAsync("PJDSentiment");
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {Time}", DateTimeOffset.Now);
            _logger.LogInformation("Name: {Name}", user.User.Name);
            await Task.Delay(1000, stoppingToken);
        }
    }
}