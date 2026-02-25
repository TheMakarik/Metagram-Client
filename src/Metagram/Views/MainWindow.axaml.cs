namespace Metagram.Views;

public partial class MainWindow : Window
{
    /*
    private static readonly Key CheatCodeActivatorKey = Key.RightShift;
    private static readonly Key[] IgnoredTextFocusKeys = [Key.LeftShift, Key.RightShift, Key.LeftCtrl, Key.RightCtrl, Key.LeftAlt, Key.RightAlt, Key.Tab, Key.Escape];
    private static readonly Key[] CheatCodeSequence = [Key.Up, Key.Up, Key.Down, Key.Down, Key.Left, Key.Right, Key.Left, Key.Right, Key.B, Key.A];

    private Queue<Key>? cheatCodeProgress = null;
    */

    public MainWindow(MainWindowViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        //KeyDown += FocusChanger;
    }
    
    /*
    private void FocusChanger(object? sender, KeyEventArgs e)
    {
        if (IgnoredTextFocusKeys.Contains(e.Key))
            return;

        if (!Keyboard.IsKeyDown(CheatCodeActivatorKey))
        {
            if (Keyboard.FocusedElement is TextBoxBase or PasswordBox)
                return;

            if (IgnoredTextFocusKeys.Contains(e.Key))
                return;

            messageInputBox.FocusInput();
            return;
        }

        if (e.Key == CheatCodeActivatorKey)
        {
            cheatCodeProgress = null;
            return;
        }

        cheatCodeProgress ??= new Queue<Key>(CheatCodeSequence);
        if (cheatCodeProgress.Peek() != e.Key)
        {
            cheatCodeProgress = null;
            return;
        }
        else
        {

        }

        cheatCodeProgress.Dequeue();
        if (cheatCodeProgress.Count == 0)
        {
            cheatCodeProgress = null;
            MessageBox.Show("������� ���");
            return;
        }
    }

    private void CheatCodeHandler(object sender, KeyEventArgs e)
    {
        if (!Keyboard.IsKeyDown(Key.RightShift) && e.Key != Key.RightShift)
            return;
    }
    */
}