FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TwitterBotPJD/TwitterBotPJD.ReceiveTweets/TwitterBotPJD.ReceiveTweets.csproj", "TwitterBotPJD.ReceiveTweets/"]
RUN dotnet restore "TwitterBotPJD.ReceiveTweets/TwitterBotPJD.ReceiveTweets.csproj"
COPY . .
WORKDIR "/src/TwitterBotPJD.ReceiveTweets"
RUN dotnet build "TwitterBotPJD.ReceiveTweets.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TwitterBotPJD.ReceiveTweets.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TwitterBotPJD.ReceiveTweets.dll"]
