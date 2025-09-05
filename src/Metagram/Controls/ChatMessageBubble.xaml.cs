using Metagram.Services.PollingServices.Abstractions;

namespace Metagram.Controls;

public partial class ChatMessageBubble : UserControl
{
    private readonly IBotMemory _botMemory = default!;

    public Message? Message
    {
        get => (Message?)GetValue(MessageProperty);
        set => SetValue(MessageProperty, value);
    }

    public Brush? BubbleBackground
    {
        get => (Brush?)GetValue(MessageProperty);
        set => SetValue(MessageProperty, value);
    }

    public Visibility TextVisibility
    {
        get => (Visibility)GetValue(TextVisibilityProperty);
        set => SetValue(TextVisibilityProperty, value);
    }

    public ChatMessageBubble()
    {
        if (App.Services != null)
        {
            _botMemory = App.Services.GetRequiredService<IBotMemory>();
        }

        InitializeComponent();
    }

    protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        switch (e.Property.Name)
        {
            case nameof(Message):
                {
                    UpdateBuble().ConfigureAwait(true);
                    break;
                }
        }
    }
    
    private async Task UpdateBuble()
    {
        await Task.Yield();
        if (Message == null)
            return;

    }

    public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(
        nameof(Message), typeof(Message), typeof(ChatMessageBubble),
        new FrameworkPropertyMetadata(defaultValue: null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public static readonly DependencyProperty BubbleBackgroundProperty = DependencyProperty.Register(
        nameof(BubbleBackground), typeof(Brush), typeof(ChatMessageBubble),
        new FrameworkPropertyMetadata(defaultValue: null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public static readonly DependencyProperty TextVisibilityProperty = DependencyProperty.Register(
        nameof(TextVisibility), typeof(Visibility), typeof(ChatMessageBubble),
        new FrameworkPropertyMetadata(defaultValue: Visibility.Hidden, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
}
