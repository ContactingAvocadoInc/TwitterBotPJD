using Tweetinvi;
using Tweetinvi.Models;
using TwitterBotPJD.ReceiveTweets;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {

    })
    .ConfigureServices((context, services) =>
    {
        var twitterCredentials = context
            .Configuration
            .GetSection("TwitterCredentials")
            .Get<TwitterCredentials>();
        services.AddSingleton<ITwitterClient>(_ => new TwitterClient(
            twitterCredentials.ConsumerKey,
            twitterCredentials.ConsumerSecret,
            twitterCredentials.AccessToken,
            twitterCredentials.AccessTokenSecret));
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();