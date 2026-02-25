namespace Metagram.Models.Authorization;

public sealed class BotAccountInfo
{
    public string Token { get; }

    public bool IsLogedIn { get; private set; }
    // Other Bot's Settings

    public BotAccountInfo(string token)
    {
        Token = token;
    }

    public async Task<User> Login(ITelegramBotClient botClient, CancellationToken cancellationToken = default)
    {
        return await botClient.GetMe(cancellationToken);
    }

    public TelegramBotClientOptions GetOptions() => new TelegramBotClientOptions(Token);
}
