using MediatR;
using Tweetinvi;
using Tweetinvi.Parameters.V2;
using TwitterBotPJD.Application.Features.PushTweet;

namespace TwitterBotPJD.ReceiveTweets;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly ITwitterClient _twitterClient;
    private readonly IMediator _mediator;

    public Worker(
        ILogger<Worker> logger,
        ITwitterClient twitterClient,
        IMediator mediator)
    {
        _logger = logger;
        _twitterClient = twitterClient;
        _mediator = mediator;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var startTime = DateTime.Now.AddSeconds(-10);

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
                var command = new PushTweetCommand
                {
                    TweetText = tweet.Text,
                    SentFromId = tweet.AuthorId
                };
                await _mediator.Send(command, stoppingToken);
            }
            await Task.Delay(80, stoppingToken);
        }
    }
}