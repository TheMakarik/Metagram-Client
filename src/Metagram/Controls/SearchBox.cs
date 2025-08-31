
namespace Metagram.Controls;

public partial class SearchBox : PlaceholderTextBox
{
    private readonly DispatcherTimer textChangeTimer;

    public TimeSpan TextChangeInterval
    {
        get => (TimeSpan)GetValue(TextChangeIntervalProperty);
        set => SetValue(TextChangeIntervalProperty, value);
    }

    public string IntervalledText
    {
        get => (string)GetValue(IntervalledTextProperty);
        set => SetValue(IntervalledTextProperty, value);
    }

    public SearchBox()
    {
        InitializeComponent();

        textChangeTimer = new DispatcherTimer(DispatcherPriority.Background);
        textChangeTimer.Tick += TextChange_Tick;
    }

    private void TextChange_Tick(object? sender, EventArgs e)
    {
        textChangeTimer.Stop();
        IntervalledText = Text;
        RaiseEvent(new TextChangedEventArgs(TextChangedEvent, UndoAction.None));
        TextChangedCommand?.Execute(Text);
    }

    protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        switch (e.Property.Name)
        {
            case nameof(Text):
                {
                    textChangeTimer.Stop();
                    textChangeTimer.Start();
                    break;
                }

            case nameof(TextChangeInterval):
                {
                    textChangeTimer.Interval = TextChangeInterval;
                    break;
                }
        }
    }

    public static readonly DependencyProperty IntervalledTextProperty = DependencyProperty.Register(
        nameof(IntervalledText), typeof(string), typeof(SearchBox),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public static readonly DependencyProperty TextChangeIntervalProperty = DependencyProperty.Register(
        nameof(TextChangeInterval), typeof(TimeSpan), typeof(SearchBox),
        new PropertyMetadata(TimeSpan.Zero));
}
