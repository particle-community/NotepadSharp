using NotepadSharp.Models;
using System.IO;
using System.Text;

namespace NotepadSharp.Services;

internal static class FileService
{
    public static FileDocument Create() => new();

    public static FileDocument Load(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException();
        }
        string content = File.ReadAllText(filePath);
        Encoding encoding = GetEncoding(filePath);
        return new FileDocument(filePath, content, encoding);
    }

    public static bool Save(FileDocument document)
    {
        if (document.FullPath == null)
        {
            return false;
        }
        try
        {
            File.WriteAllText(document.FullPath, document.Content, document.Encoding);
            document.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }

    private static Encoding GetEncoding(string fileName)
    {
        using var reader = new StreamReader(fileName, true);
        reader.Peek();
        return reader.CurrentEncoding;
    }
}
