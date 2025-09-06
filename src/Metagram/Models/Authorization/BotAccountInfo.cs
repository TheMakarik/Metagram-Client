namespace Metagram.Models.Authorization;

public sealed class BotAccountInfo(string token)
{
    // Bot's Settings
    public string Token { get; set; } = token;

    public TelegramBotClientOptions GetOptions() => new TelegramBotClientOptions(Token);
}
