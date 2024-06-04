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
        Encoding encoding = GetEncoding(filePath);
        string content = File.ReadAllText(filePath, encoding);
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

    public static Encoding GetEncoding(string filename)
    {
        using var reader = new StreamReader(filename, true);
        if (reader.Peek() >= 0)
        {
            reader.Read();
        }
        return reader.CurrentEncoding;
    }
}
