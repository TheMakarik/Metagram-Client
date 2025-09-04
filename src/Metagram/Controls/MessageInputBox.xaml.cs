using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;

namespace Metagram.Controls;

public partial class MessageInputBox : UserControl
{
    public string MessageText
    {
        get => (string)GetValue(MessageTextProperty);
        set => SetValue(MessageTextProperty, value);
    }

    public ICommand SendMessageCommand
    {
        get => (ICommand)GetValue(SendMessageCommandProperty);
        set => SetValue(SendMessageCommandProperty, value);
    }

    public MessageInputBox() : base()
    {
        InitializeComponent();
        placeholderTextBox.TextPreviewKeyDown += TextBox_PreviewKeyDown;
    }

    private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter && Keyboard.Modifiers.HasFlag(ModifierKeys.Shift))
        {
            if (SendMessageCommand.CanExecute(null))
                SendMessageCommand.Execute(null);

            e.Handled = true;
        }
    }

    public static readonly DependencyProperty MessageTextProperty = DependencyProperty.Register(
        nameof(MessageText), typeof(string), typeof(MessageInputBox),
        new FrameworkPropertyMetadata(defaultValue: null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public static readonly DependencyProperty SendMessageCommandProperty = DependencyProperty.Register(
        nameof(SendMessageCommand), typeof(ICommand), typeof(MessageInputBox),
        new FrameworkPropertyMetadata(defaultValue: null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
}
