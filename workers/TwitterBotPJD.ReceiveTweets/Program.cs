using System.Security.Authentication;
using MassTransit;
using MediatR;
using Tweetinvi;
using Tweetinvi.Models;
using TwitterBotPJD.Application.Features.PushTweet;
using TwitterBotPJD.ReceiveTweets;
using TwitterBotPJD.ReceiveTweets.Configuration;

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
        
        var rabbitMqCredentials = context
            .Configuration
            .GetSection("RabbitMQ")
            .Get<RabbitMqConfiguration>();
        
        services.AddSingleton<ITwitterClient>(_ => new TwitterClient(
            twitterCredentials.ConsumerKey,
            twitterCredentials.ConsumerSecret,
            twitterCredentials.AccessToken,
            twitterCredentials.AccessTokenSecret));
        
        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context,cfg) =>
            {
                cfg.Host(rabbitMqCredentials.Host,5671, rabbitMqCredentials.Username, h => {
                    h.Username(rabbitMqCredentials.Username);
                    h.Password(rabbitMqCredentials.Password);
                    
                    h.UseSsl(s =>
                    {
                        s.Protocol = SslProtocols.Tls12;
                    });
                });

                cfg.ConfigureEndpoints(context);
            });
        });
        
        services.AddMediatR(typeof(PushTweetCommand));
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();