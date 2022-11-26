using TwitterSharp.Client;
using TwitterSharp.Rule;

namespace TwitterBotPJD.ReceiveTweets;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly TwitterClient _twitterClient;

    public Worker(
        ILogger<Worker> logger,
        TwitterClient twitterClient)
    {
        _logger = logger;
        _twitterClient = twitterClient;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var tweets = await _twitterClient.GetRecentTweets(Expression.Author("PJDSentiment"));
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {Time}", DateTimeOffset.Now);
            foreach (var tweet in tweets)
            {
                _logger.LogInformation("Tweet: {Tweet}", tweet.Text);
            }
            await Task.Delay(1000, stoppingToken);
        }
    }
}