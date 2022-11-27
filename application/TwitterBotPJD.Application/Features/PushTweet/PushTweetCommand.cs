using MediatR;

namespace TwitterBotPJD.Application.Features.PushTweet;

public class PushTweetCommand : IRequest
{
    public string TweetText { get; init; }
    public string RequestedUser { get; init; }
    public string SentFromTweetId { get; init; }
}