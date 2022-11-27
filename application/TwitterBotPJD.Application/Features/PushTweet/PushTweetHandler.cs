using MediatR;
using Microsoft.Extensions.Logging;

namespace TwitterBotPJD.Application.Features.PushTweet;

public class PushTweetHandler : IRequestHandler<PushTweetCommand>
{
    private readonly ILogger<PushTweetHandler> _logger;

    public PushTweetHandler(ILogger<PushTweetHandler> logger)
    {
        _logger = logger;
    }

    public Task<Unit> Handle(PushTweetCommand request, CancellationToken cancellationToken)
    {
        Console.WriteLine($"From: {request.SentFromId}, Text: {request.TweetText}" );
        return Task.FromResult(Unit.Value);
    }
}