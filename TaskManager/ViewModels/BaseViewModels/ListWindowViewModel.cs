using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using TaskManager.Utilities;

namespace TaskManager.ViewModels.BaseViewModels;

internal abstract class ListWindowViewModel<T> : BaseViewModel
{
  protected ICommand? _loadCommand;
  public ICommand LoadCommand => this._loadCommand ??= new RelayCommand(async f => await this.UpdateData(f), this.CanLoadExecute);
  private ObservableCollection<T> _currentCollection = [];
  public ObservableCollection<T> CurrentCollection
  {
    get => this._currentCollection;
    set
    {
      this._currentCollection = value;
      this.OnPropertyChanged();
    }
  }
  protected abstract Task UpdateData(object sender);
  protected virtual bool CanLoadExecute(object parameter)
  {
    return App.CurrentUser is not null;
  }
}