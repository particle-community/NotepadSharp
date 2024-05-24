using System.Globalization;
using System.Windows.Data;

namespace NotepadSharp.Converters;

internal class NullableStringToFileName : IValueConverter
{
    private const string NullableFileName = "Untitled";

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {

        if (value is null)
        {
            return NullableFileName;
        }
        else if (value is string stringValue)
        {
            return stringValue;
        }
        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
