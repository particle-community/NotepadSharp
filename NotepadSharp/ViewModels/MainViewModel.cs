using Microsoft.Win32;
using NotepadSharp.Commands;
using NotepadSharp.Helpers;
using NotepadSharp.Models;
using NotepadSharp.Services;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace NotepadSharp.ViewModels;

internal class MainViewModel : BaseViewModel
{
    private static readonly string FileFilter = ConfigurationHelper.GetFileFilter();

    private FileDocument? _fileDocument;

    public MainViewModel()
    {
        FileDocument newDocument = new();
        Documents = [newDocument];
        SelectedDocument = newDocument;

        NewCommand = new RelayCommand(New);
        OpenCommand = new RelayCommand(Open);
        SaveCommand = new RelayCommand(Save, CanSave);
        SaveAsCommand = new RelayCommand(SaveAs, CanSaveAs);
        CloseCommand = new RelayCommand(Close);
        ExitCommand = new RelayCommand(Exit);
    }

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

    public ICommand NewCommand { get; set; }

    public ICommand OpenCommand { get; set; }

    public ICommand SaveCommand { get; set; }

    public ICommand SaveAsCommand { get; set; }

    public ICommand CloseCommand { get; set; }

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

    public void Exit(object? parameter)
    {
        Application.Current.Shutdown();
    }
}
