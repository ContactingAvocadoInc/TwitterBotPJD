using TwitterBotPJD.ReceiveTweets;
using TwitterSharp.Client;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {

    })
    .ConfigureServices((context, services) =>
    {
        var bearerToken = context
            .Configuration
            .GetSection("TwitterCredentials:BearerToken").Value;
        services.AddSingleton(_ => new TwitterClient(bearerToken));
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();