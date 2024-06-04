using MaterialDesignThemes.Wpf;
using System.Collections.Specialized;
using System.Configuration;
using System.Text;

namespace NotepadSharp.Helpers;

internal static class ConfigurationHelper
{
    public static readonly int DefaultZoomLevel;
    public static readonly int MinZoomLevel;
    public static readonly int MaxZoomLevel;

    private static readonly NameValueCollection _fileFilters;
    private static readonly NameValueCollection _fileIcons;

    static ConfigurationHelper()
    {
        _fileFilters = (NameValueCollection)ConfigurationManager.GetSection("fileFilters");
        _fileIcons = (NameValueCollection)ConfigurationManager.GetSection("fileIcons");

        DefaultZoomLevel = int.Parse(ConfigurationManager.AppSettings["DefaultZoomLevel"]!);
        MinZoomLevel = int.Parse(ConfigurationManager.AppSettings["MinZoomLevel"]!);
        MaxZoomLevel = int.Parse(ConfigurationManager.AppSettings["MaxZoomLevel"]!);
    }

    public static string GetFileFilter()
    {
        StringBuilder filter = new();
        string? value;
        foreach (string? key in _fileFilters.AllKeys)
        {
            if (key == null)
            {
                continue;
            }
            value = _fileFilters[key];
            if (value != null)
            {
                filter.Append($"{key} ({value})|{value}|");
            }
        }
        return filter.ToString().TrimEnd('|');
    }

    public static PackIconKind? GetIconKind(string? fileExtension)
    {
        string? value = _fileIcons[fileExtension?.ToLower()];
        if (Enum.TryParse(value, out PackIconKind iconKind))
        {
            return iconKind;
        }
        return null;
    }
}
