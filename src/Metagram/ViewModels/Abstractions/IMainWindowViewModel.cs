using Metagram.Models.Polling;

namespace Metagram.ViewModels.Abstractions;

internal interface IMainWindowViewModel
{
    public ChatMemory SelectedChat { get; set; }
    public string MessageInput { get; set; }
    public ICommand SendMessageCommand { get; }
}
