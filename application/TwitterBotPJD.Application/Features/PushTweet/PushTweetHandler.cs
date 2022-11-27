using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Tweetinvi;
using Tweetinvi.Parameters.V2;

namespace TwitterBotPJD.Application.Features.PushTweet;

public class PushTweetHandler : IRequestHandler<PushTweetCommand>
{
    private readonly ILogger<PushTweetHandler> _logger;
    private readonly ITwitterClient _twitterClient;
    private readonly IBus _bus;
    public PushTweetHandler(
        ILogger<PushTweetHandler> logger,
        ITwitterClient twitterClient,
        IBus bus)
    {
        _logger = logger;
        _twitterClient = twitterClient;
        _bus = bus;
    }

    public async Task<Unit> Handle(PushTweetCommand request, CancellationToken cancellationToken)
    {
        Console.WriteLine($"From: {request.SentFromTweetId}, Text: {request.TweetText}" );
        
        var searchResponse = await _twitterClient.SearchV2.SearchTweetsAsync(
            new SearchTweetsV2Parameters($"from:{request.RequestedUser}")
            {
                PageSize = 10
            });

        var latestTweetFromUser = searchResponse.Tweets.FirstOrDefault();

        if (latestTweetFromUser == null)
        {
            Console.WriteLine("No tweets from this user.");
            return await Task.FromResult(Unit.Value);
        }

        await _bus.Publish(new TwitterMessage { Text = latestTweetFromUser.Text, Id = latestTweetFromUser.Id },cancellationToken);
        
        return await Task.FromResult(Unit.Value);
    }
}