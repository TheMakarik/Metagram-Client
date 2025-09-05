using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media.Imaging;

namespace Metagram.Converters;

[ValueConversion(typeof(Sticker), typeof(Task<BitmapImage>))]
public class StickerToBitmapConverter : MarkupExtension, IValueConverter
{
    private static readonly object _lock = new object();
    private static readonly Dictionary<string, BitmapImage> _cached = [];
    private static readonly Dictionary<string, Task<BitmapImage>> _downloading = [];
    private static readonly ITelegramBotClient? _client = App.Services.GetService<ITelegramBotClient>();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (_client == null)
            return Task.FromResult(new BitmapImage());

        if (value is not Sticker sticker)
            return Task.FromResult(new BitmapImage());

        if (_cached.TryGetValue(sticker.FileId, out BitmapImage? bitmapImage))
            return bitmapImage;

        Task<BitmapImage>? promise = null;
        lock (_lock)
        {
            if (!_downloading.TryGetValue(sticker.FileId, out promise))
            {
                promise = Download(sticker);
                _downloading.Add(sticker.FileId, promise);
            }
        }

        return promise.Result;
    }

    private static async Task<BitmapImage> Download(Sticker sticker)
    {
        BitmapImage bitmapImage = new BitmapImage();
        bitmapImage.BeginInit();

        bitmapImage.StreamSource = new MemoryStream();
        await _client!.DownloadFile(sticker.FileId, bitmapImage.StreamSource);

        bitmapImage.EndInit();
        bitmapImage.Freeze();

        _cached.Add(sticker.FileId, bitmapImage);
        _downloading.Remove(sticker.FileId);
        return bitmapImage;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => DependencyProperty.UnsetValue;
    public override object ProvideValue(IServiceProvider serviceProvider) => this;
}
