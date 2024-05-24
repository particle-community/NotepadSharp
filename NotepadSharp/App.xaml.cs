using NotepadSharp.ViewModels;
using NotepadSharp.Views;
using System.Configuration;
using System.Data;
using System.Windows;

namespace NotepadSharp;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        MainView mainView = new()
        {
            DataContext = new MainViewModel()
        };
        mainView.Show();
    }
}
