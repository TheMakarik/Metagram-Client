using Metagram.Models.Polling;

namespace Metagram.Controls;

public partial class ChatPresenter 
{
    public ChatMemory SelectedChat
    {
        get => (ChatMemory)GetValue(SelectedChatProperty);
        set => SetValue(SelectedChatProperty, value);
    }

    public ChatPresenter()
    {
        InitializeComponent();
    }

    protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        switch (e.Property.Name)
        {
            case nameof(SelectedChat):
                {
                    break;
                }
        }
    }

    public static readonly DependencyProperty SelectedChatProperty = DependencyProperty.Register(
        nameof(SelectedChat), typeof(ChatMemory), typeof(ChatPresenter),
        new PropertyMetadata(defaultValue: null));
}