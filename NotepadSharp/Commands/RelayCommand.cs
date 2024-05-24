using System.Windows.Input;

namespace NotepadSharp.Commands
{
    class RelayCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Predicate<object?>? _predicate;

        public RelayCommand(Action<object?> execute, Predicate<object?>? predicate = null)
        {
            _execute = execute;
            _predicate = predicate;
        }

        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object? parameter)
        {
            return _predicate == null || _predicate(parameter);
        }

        public void Execute(object? parameter)
        {
            _execute?.Invoke(parameter);
        }
    }
}
