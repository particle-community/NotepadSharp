using System.IO;
using System.Text;

namespace NotepadSharp.Models;

public class FileDocument : BaseModel
{
    private string _content;
    private string? _fullPath;
    private bool _isModified;
    private Encoding _encoding;

    public FileDocument() : this(null, string.Empty, Encoding.Default) { }

    public FileDocument(string? fullPath, string content, Encoding encoding)
    {
        _fullPath = fullPath;
        _content = content;
        _encoding = encoding;
    }

    public string? FullPath
    {
        get => _fullPath;
        set
        {
            if (_fullPath != value)
            {
                _fullPath = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(Extension));
            }
        }
    }

    public string Content
    {
        get => _content;
        set
        {
            _content = value;
            IsModified = true;
            OnPropertyChanged();
        }
    }

    public bool IsModified
    {
        get => _isModified;
        set
        {
            _isModified = value;
            OnPropertyChanged();
        }
    }

    public Encoding Encoding
    {
        get => _encoding;
        set
        {
            _encoding = value;
            OnPropertyChanged();
        }
    }

    public string? Name => Path.GetFileName(FullPath);

    public string? Extension => Path.GetExtension(FullPath);
}
