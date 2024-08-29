using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using TaskManager.Utilities;

namespace TaskManager.ViewModels;

internal abstract class BaseViewModel : INotifyPropertyChanged
{
  protected ICommand? _loadedCommand;
  public ICommand LoadedCommand => this._loadedCommand ??= new RelayCommand(async f => await this.OnLoad(f), this.CanExecute);

  protected abstract Task OnLoad(object sender);
  protected abstract bool CanExecute(object parameter);
  public event PropertyChangedEventHandler? PropertyChanged;
  public void OnPropertyChanged([CallerMemberName] string prop = "")
  {
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
  }
}