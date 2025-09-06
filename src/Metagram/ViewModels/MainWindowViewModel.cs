using CommunityToolkit.Mvvm.Input;
using Metagram.Models.Authorization;
using Metagram.Models.Polling;
using Message = Telegram.Bot.Types.Message;

namespace Metagram.ViewModels;

internal partial class MainWindowViewModel : BaseViewModel<MainWindow>, IMainWindowViewModel
{
    private readonly ILogger<MainWindowViewModel> _logger;

    [ObservableProperty]
    private BotRuntimeSession? selectedSession;

    [ObservableProperty]
    private ChatMemory? selectedChat;

    [ObservableProperty]
    private string? messageInput;

    [ObservableProperty]
    private ICommand? sendMessageCommand;

    public MainWindowViewModel(ILogger<MainWindowViewModel> logger)
    {
        _logger = logger;
        sendMessageCommand = new RelayCommand(SendMessage);
    }

    private void SendMessage()
    {
        MessageBox.Show(SelectedSession?.Account.Token ?? "<NULL>");
        try
        {
            if (SelectedSession == null)
                return;

            if (SelectedChat == null)
                return;

            if (MessageInput == null)
                return;

            Message message = SelectedSession.Client.SendMessage(SelectedChat.Chat, MessageInput).Result;
            SelectedChat.AddMessage(message);
            MessageInput = string.Empty;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "");
        }
    }
}
