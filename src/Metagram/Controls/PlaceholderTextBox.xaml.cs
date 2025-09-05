namespace Metagram.Controls;

public partial class PlaceholderTextBox : UserControl
{
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    public bool IsTextEmpty
    {
        get => (bool)GetValue(IsTextEmptyProperty);
        protected set => SetValue(IsTextEmptyProperty, value);
    }

    public Brush PlaceholderForeground
    {
        get => (Brush)GetValue(PlaceholderForegroundProperty);
        set => SetValue(PlaceholderForegroundProperty, value);
    }

    public Thickness PlaceholderMargin
    {
        get => (Thickness)GetValue(PlaceholderMarginProperty);
        set => SetValue(PlaceholderMarginProperty, value);
    }

    public double PlaceholderFontSize
    {
        get => (double)GetValue(PlaceholderFontSizeProperty);
        set => SetValue(PlaceholderFontSizeProperty, value);
    }

    public ICommand? TextChangedCommand
    {
        get => (ICommand?)GetValue(TextChangedCommandProperty);
        set => SetValue(TextChangedCommandProperty, value);
    }

    public event TextChangedEventHandler TextChanged
    {
        add => AddHandler(TextChangedEvent, value);
        remove => RemoveHandler(TextChangedEvent, value);
    }

    public event KeyEventHandler TextPreviewKeyDown
    {
        add => AddHandler(TextBox.PreviewKeyDownEvent, value);
        remove => RemoveHandler(TextBox.TextChangedEvent, value);
    }

    public PlaceholderTextBox()
    {
        InitializeComponent();
    }

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        TextBox textBox = (TextBox)sender;
        Text = textBox.Text;
    }

    protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        switch (e.Property.Name)
        {
            case nameof(Text):
                {
                    IsTextEmpty = string.IsNullOrEmpty(Text);
                    break;
                }
        }
    }

    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
        nameof(Text), typeof(string), typeof(PlaceholderTextBox),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.Register(
        nameof(Placeholder), typeof(string), typeof(PlaceholderTextBox),
        new PropertyMetadata(null));

    public static readonly DependencyProperty IsTextEmptyProperty = DependencyProperty.Register(
        nameof(IsTextEmpty), typeof(bool), typeof(PlaceholderTextBox),
        new PropertyMetadata(true));

    public static readonly DependencyProperty PlaceholderForegroundProperty = DependencyProperty.Register(
        nameof(PlaceholderForeground), typeof(Brush), typeof(PlaceholderTextBox),
        new PropertyMetadata(null));

    public static readonly DependencyProperty PlaceholderMarginProperty = DependencyProperty.Register(
        nameof(PlaceholderMargin), typeof(Thickness), typeof(PlaceholderTextBox),
        new PropertyMetadata(default(Thickness)));

    public static readonly DependencyProperty PlaceholderFontSizeProperty = DependencyProperty.Register(
        nameof(PlaceholderFontSize), typeof(double), typeof(PlaceholderTextBox),
        new PropertyMetadata(15d));

    public static readonly DependencyProperty TextChangedCommandProperty = DependencyProperty.Register(
        nameof(TextChangedCommand), typeof(ICommand), typeof(PlaceholderTextBox),
        new PropertyMetadata(null));

    public static readonly RoutedEvent TextChangedEvent = EventManager.RegisterRoutedEvent(
        nameof(TextChanged), RoutingStrategy.Bubble,
        typeof(TextChangedEventHandler), typeof(PlaceholderTextBox));
}
