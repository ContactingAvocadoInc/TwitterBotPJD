using Tweetinvi;
using Tweetinvi.Parameters.V2;

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
        var startTime = DateTime.Now.AddDays(-7);

        while (!stoppingToken.IsCancellationRequested)
        {
            var searchResponse = await _twitterClient.SearchV2.SearchTweetsAsync(
                new SearchTweetsV2Parameters("pjdsentiment")
                {
                    StartTime = startTime
                });
            var tweets = searchResponse.Tweets;
            var lastTweet = tweets.FirstOrDefault();

            if (lastTweet != null)
                startTime = lastTweet!.CreatedAt.DateTime.AddSeconds(1);

            foreach (var tweet in tweets)
            {
                _logger.LogInformation("tweet: {@Tweet}", tweet.Text);
            }
            await Task.Delay(10000, stoppingToken);
        }
    }
}