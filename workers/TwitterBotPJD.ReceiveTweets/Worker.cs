using MassTransit;
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
    private string _sinceId = string.Empty;
    private readonly IBus _bus;

    public Worker(
        ILogger<Worker> logger,
        ITwitterClient twitterClient,
        IMediator mediator,
        IBus bus)
    {
        _logger = logger;
        _twitterClient = twitterClient;
        _mediator = mediator;
        _bus = bus;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var tenRecentTweets = await _twitterClient.SearchV2.SearchTweetsAsync(
            new SearchTweetsV2Parameters("#pjdsentiment")
        {
            PageSize = 10
        });
        var recentTweet = tenRecentTweets.Tweets.FirstOrDefault();
        if (recentTweet != null)
            _sinceId = recentTweet.Id;

        while (!stoppingToken.IsCancellationRequested)
        {
            var searchResponse = await _twitterClient.SearchV2.SearchTweetsAsync(
                new SearchTweetsV2Parameters("#pjdsentiment")
                {
                    SinceId = _sinceId
                });
            var tweets = searchResponse.Tweets;
            var lastTweet = tweets.FirstOrDefault();

            if (lastTweet != null)
                _sinceId = lastTweet.Id;

            
            foreach (var tweet in tweets)
            {
                var firstMentions = tweet.Entities.Mentions.FirstOrDefault().Username;

                if (firstMentions == null)
                {
                    Console.WriteLine("No mentions in tweet.");
                    continue;
                }
                    
                
                var command = new PushTweetCommand
                {
                    TweetText = tweet.Text,
                    RequestedUser = firstMentions,
                    SentFromTweetId = tweet.AuthorId
                };
                await _mediator.Send(command, stoppingToken);
            }
            await Task.Delay(8000, stoppingToken);
        }
    }
}