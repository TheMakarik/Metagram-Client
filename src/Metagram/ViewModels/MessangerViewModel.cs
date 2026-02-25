namespace Metagram.ViewModels;

public partial class MessangerViewModel : RoutableViewModelBase
{
    private readonly ILogger<MessangerViewModel> _logger;
    private readonly IAccountsManager _accountsManager;

    [Reactive]
    private BotRuntimeSession? selectedSession;

    [Reactive]
    private ChatHistory? selectedChat;

    [Reactive]
    private string? messageInput;

    [Reactive]
    private string windowTitle = "Metagram client";

    [Reactive]
    private ICommand? sendMessageCommand;

    [Reactive]
    private ICommand? addMetaBotCommand;

    public MessangerViewModel(ILogger<MessangerViewModel> logger, IScreen screen, IAccountsManager accountsManager) : base("messanger", screen)
    {
        _logger = logger;
        _accountsManager = accountsManager;

        sendMessageCommand = ReactiveCommand.Create(SendMessage);
        addMetaBotCommand = ReactiveCommand.Create(TryLogin);
    }

    private void TryLogin()
    {
        HostScreen.NavigateTo<LoginViewModel>();
    }

    private void SendMessage()
    {
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
