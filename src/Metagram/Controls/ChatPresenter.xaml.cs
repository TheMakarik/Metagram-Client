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

/*
public sealed class MessageBubbleTemplateSelector : DataTemplateSelector
{
    public DataTemplate? StickerMessageTemplate { get; set; }
    public DataTemplate? RegularMessageTemplate { get; set; }

    public override DataTemplate SelectTemplate(object item, DependencyObject container)
    {
        if (item is not Message message)
            return null;

        if (message.Sticker != null)
            return StickerMessageTemplate;

        return base.SelectTemplate(item, container);
    }
}
*/