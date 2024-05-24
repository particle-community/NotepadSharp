using MaterialDesignThemes.Wpf;
using System.Globalization;
using System.Windows.Data;

namespace NotepadSharp.Converters;

internal class FileExtensionToPackIconKindConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        string extension = (string)value;
        return extension switch
        {
            // C
            ".c" => PackIconKind.LanguageC,
            ".h" => PackIconKind.LanguageC,

            // C++
            ".cc" => PackIconKind.LanguageCpp,
            ".cpp" => PackIconKind.LanguageCpp,
            ".cxx" => PackIconKind.LanguageCpp,
            ".hh" => PackIconKind.LanguageCpp,
            ".hpp" => PackIconKind.LanguageCpp,
            ".hxx" => PackIconKind.LanguageCpp,

            // C#
            ".cs" => PackIconKind.LanguageCsharp,
            ".csharp" => PackIconKind.LanguageCsharp,

            // CSS
            ".css" => PackIconKind.LanguageCss3,

            // Fortran
            ".f" => PackIconKind.LanguageFortran,
            ".for" => PackIconKind.LanguageFortran,
            ".f90" => PackIconKind.LanguageFortran,
            ".f95" => PackIconKind.LanguageFortran,
            ".f03" => PackIconKind.LanguageFortran,
            ".f08" => PackIconKind.LanguageFortran,
            ".f15" => PackIconKind.LanguageFortran,
            ".fh" => PackIconKind.LanguageFortran,

            // Go
            ".go" => PackIconKind.LanguageGo,

            // HTML
            ".html" => PackIconKind.LanguageHtml5,
            ".htm" => PackIconKind.LanguageHtml5,

            // Java
            ".java" => PackIconKind.LanguageJava,

            // JavaScript
            ".js" => PackIconKind.LanguageJavascript,
            ".mjs" => PackIconKind.LanguageJavascript,
            ".cjs" => PackIconKind.LanguageJavascript,

            // Kotlin
            ".kt" => PackIconKind.LanguageKotlin,
            ".kts" => PackIconKind.LanguageKotlin,

            // PHP
            ".php" => PackIconKind.LanguagePhp,
            ".phtml" => PackIconKind.LanguagePhp,

            // Python
            ".py" => PackIconKind.LanguagePython,

            // Ruby
            ".rb" => PackIconKind.LanguageRuby,

            // Rust
            ".rs" => PackIconKind.LanguageRust,

            // TypeScript
            ".ts" => PackIconKind.LanguageTypescript,
            ".tsx" => PackIconKind.LanguageTypescript,

            // SQL
            ".sql" => PackIconKind.Database,

            // Markdown
            ".md" => PackIconKind.LanguageMarkdown,

            // Shell Script
            ".sh" => PackIconKind.Console,

            // PowerShell
            ".ps1" => PackIconKind.Powershell,

            // XAML
            ".xaml" => PackIconKind.LanguageXaml,

            // XML
            ".xml" => PackIconKind.Xml,

            // JSON
            ".json" => PackIconKind.CodeJson,

            _ => PackIconKind.Text
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
