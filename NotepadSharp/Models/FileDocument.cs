using System.IO;
using System.Text;

namespace NotepadSharp.Models;

public class FileDocument : BaseModel
{
    private string _originalContent;
    private string _content;
    private string? _fullPath;
    private Encoding _encoding;

    public FileDocument() : this(null, string.Empty, Encoding.Default) { }

    public FileDocument(string? fullPath, string content, Encoding encoding)
    {
        _fullPath = fullPath;
        _originalContent = content;
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
            OnPropertyChanged();
            OnPropertyChanged(nameof(IsModified));
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

    public bool IsModified => _originalContent != _content;

    public void SaveChanges()
    {
        _originalContent = _content;
        OnPropertyChanged(nameof(IsModified));        
    }
}
