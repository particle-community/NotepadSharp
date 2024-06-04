using Microsoft.Win32;
using NotepadSharp.Commands;
using NotepadSharp.Helpers;
using NotepadSharp.Models;
using NotepadSharp.Services;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace NotepadSharp.ViewModels;

internal class MainViewModel : BaseViewModel
{
    private static readonly string FileFilter = ConfigurationHelper.GetFileFilter();
    private static readonly int DefaultZoomLevel = ConfigurationHelper.DefaultZoomLevel;
    private static readonly int MinZoomLevel = ConfigurationHelper.MinZoomLevel;
    private static readonly int MaxZoomLevel = ConfigurationHelper.MaxZoomLevel;

    private FileDocument? _fileDocument;
    private int _zoomLevel;

    public MainViewModel()
    {
        Encodings = new(Encoding.GetEncodings().Select(e => e.GetEncoding()));

        FileDocument newDocument = new();
        Documents = [newDocument];
        SelectedDocument = newDocument;

        ZoomLevel = DefaultZoomLevel;

        NewCommand = new RelayCommand(New);
        OpenCommand = new RelayCommand(Open);
        SaveCommand = new RelayCommand(Save, CanSave);
        SaveAsCommand = new RelayCommand(SaveAs, CanSaveAs);

        CloseCommand = new RelayCommand(Close);

        RestoreZoomCommand = new RelayCommand(RestoreZoom);
        ZoomInCommand = new RelayCommand(ZoomIn);
        ZoomOutCommand = new RelayCommand(ZoomOut);

        ExitCommand = new RelayCommand(Exit);
    }

    public List<Encoding> Encodings { get; private set; }

    public ObservableCollection<FileDocument> Documents { get; private set; }

    public FileDocument? SelectedDocument
    {
        get => _fileDocument;
        set
        {
            _fileDocument = value;
            OnPropertyChanged();
        }
    }

    public int ZoomLevel
    {
        get => _zoomLevel;
        set
        {
            _zoomLevel = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(ZoomLevelRatio));
        }
    }

    public double ZoomLevelRatio => ZoomLevel / (double)DefaultZoomLevel;

    public ICommand NewCommand { get; set; }
    public ICommand OpenCommand { get; set; }
    public ICommand SaveCommand { get; set; }
    public ICommand SaveAsCommand { get; set; }
    public ICommand CloseCommand { get; set; }
    public ICommand RestoreZoomCommand { get; set; }
    public ICommand ZoomInCommand { get; set; }
    public ICommand ZoomOutCommand { get; set; }
    public ICommand ExitCommand { get; set; }

    public void New(object? parameter = null)
    {
        FileDocument document = FileService.Create();
        Documents.Add(document);
        SelectedDocument = document;
    }

    public void Open(object? parameter = null)
    {
        OpenFileDialog dialog = new()
        {
            Filter = FileFilter,
            Multiselect = true,
            Title = "Open"
        };
        if (dialog.ShowDialog() == true)
        {
            FileDocument? document = null;
            foreach (string filePath in dialog.FileNames)
            {
                document = Documents.FirstOrDefault(doc => doc.FullPath == filePath);
                if (document == null)
                {
                    document = FileService.Load(filePath);
                    Documents.Add(document);
                }
            }
            SelectedDocument = document;
        }
    }

    public void Save(object? parameter = null)
    {
        FileDocument? document = (parameter as FileDocument) ?? SelectedDocument;
        if (document != null)
        {
            if (document.FullPath == null)
            {
                SaveAs(document);
            }
            else
            {
                FileService.Save(document);
            }
        }
    }

    public void SaveAs(object? parameter = null)
    {
        FileDocument? document = (parameter as FileDocument) ?? SelectedDocument;
        if (document != null)
        {
            SaveFileDialog dialog = new()
            {
                AddExtension = true,
                FileName = document.Name ?? "untitled.txt",
                Filter = FileFilter,
                Title = "Save"
            };
            if (dialog.ShowDialog() == true)
            {
                document.FullPath = dialog.FileName;
                FileService.Save(document);
            }
        }
    }

    public bool CanSave(object? parameter = null)
    {
        return SelectedDocument != null && (SelectedDocument.IsModified || SelectedDocument.Name == null);
    }

    public bool CanSaveAs(object? parameter = null)
    {
        return SelectedDocument != null;
    }

    public void Close(object? parameter)
    {
        if (parameter is FileDocument document)
        {
            if (document.IsModified)
            {
                MessageBoxResult result = MessageBox.Show(
                    messageBoxText: $"Do you want to save the changes to '{document.Name}'?",
                    caption: "Unsaved changes",
                    button: MessageBoxButton.YesNoCancel,
                    icon: MessageBoxImage.Warning
                );
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        Save(document);
                        if (document.IsModified)
                        {
                            return;
                        }
                        break;
                    case MessageBoxResult.No:
                        break;
                    default:
                        return;
                }
            }
            Documents.Remove(document);
            if (Documents.Count == 0)
            {
                document = FileService.Create();
                Documents.Add(document);
                SelectedDocument = document;
            }
        }
    }

    public void RestoreZoom(object? parameter = null)
    {
        ZoomLevel = DefaultZoomLevel;
    }

    public void ZoomIn(object? parameter = null)
    {
        if (ZoomLevel < MaxZoomLevel)
        {
            ZoomLevel += 2;
        }
    }

    public void ZoomOut(object? parameter = null)
    {
        if (ZoomLevel > MinZoomLevel)
        {
            ZoomLevel -= 2;
        }
    }

    public void Exit(object? parameter = null)
    {
        Application.Current.Shutdown();
    }
}
