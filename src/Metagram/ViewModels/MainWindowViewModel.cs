using CommunityToolkit.Mvvm.Input;
using Metagram.Models.Polling;
using Message = Telegram.Bot.Types.Message;

namespace Metagram.ViewModels;

internal partial class MainWindowViewModel : BaseViewModel<MainWindow>, IMainWindowViewModel
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<MainWindowViewModel> _logger;

    [ObservableProperty]
    private ChatMemory? selectedChat;

    [ObservableProperty]
    private string? messageInput;

    [ObservableProperty]
    private ICommand? sendMessageCommand;

    public MainWindowViewModel(ILogger<MainWindowViewModel> logger, ITelegramBotClient botClient)
    {
        _botClient = botClient;
        _logger = logger;
        sendMessageCommand = new RelayCommand(SendMessage);
    }

    private void SendMessage()
    {
        try
        {
            if (SelectedChat == null)
                return;

            if (MessageInput == null)
                return;

            Message message = _botClient.SendMessage(SelectedChat.Chat, MessageInput).Result;
            SelectedChat.AddMessage(message);
            MessageInput = string.Empty;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "");
        }
    }
}
