using MaterialDesignThemes.Wpf;
using NotepadSharp.Helpers;
using System.Globalization;
using System.Windows.Data;

namespace NotepadSharp.Converters;

internal class FileExtensionToPackIconKindConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        string? extension = (string?)value;
        return ConfigurationHelper.GetIconKind(extension) ?? PackIconKind.FileDocument;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
