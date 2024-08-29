using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TaskManager.Utilities;

namespace TaskManager.ViewModels;

internal abstract class ListViewModel<T>: BaseViewModel
{
  private ObservableCollection<T> _currentCollection;
  public ObservableCollection<T> CurrentCollection
  {
    get => this._currentCollection;
    set
    {
      this._currentCollection = value;
      this.OnPropertyChanged();
    }
  }
  private ICommand? _addButtonClick;
  public ICommand AddButtonClick => this._addButtonClick ??= new RelayCommand(async f =>
  {
    //кнопка добавить элемент.
  }, CanExecute);
  protected static bool CanExecute(object parameter)
  {
    return App.CurrentUser is not null;
  }
}