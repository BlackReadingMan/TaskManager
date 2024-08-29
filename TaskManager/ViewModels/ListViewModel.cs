using System.Collections.ObjectModel;
using System.Windows.Input;
using TaskManager.Utilities;

namespace TaskManager.ViewModels;

internal abstract class ListViewModel<T> : BaseViewModel
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
  protected override bool CanExecute(object parameter)
  {
    return App.CurrentUser is not null;
  }

  public ICommand AddButtonClick => this._addButtonClick ??= new RelayCommand(async f =>
  {
    //кнопка добавить элемент.
  }, this.CanExecute);
}