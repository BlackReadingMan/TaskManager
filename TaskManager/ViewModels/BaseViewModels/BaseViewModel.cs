using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TaskManager.Utilities;

namespace TaskManager.ViewModels.BaseViewModels;

internal abstract class BaseViewModel : INotifyPropertyChanged
{
  private ICommand? _addButtonClick;
  public ICommand AddButtonClick => this._addButtonClick ??= new RelayCommand(f => this.AddSubject(), this.CanExecute);
  protected abstract void AddSubject();
  protected abstract bool CanExecute(object parameter);
  public event PropertyChangedEventHandler? PropertyChanged;
  public void OnPropertyChanged([CallerMemberName] string prop = "")
  {
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
  }
}