namespace TwitterBotPJD.Application.Features.PushTweet;

public record TwitterMessage
{
    public string Text { get; init; }
    public string Id { get; init; }
}