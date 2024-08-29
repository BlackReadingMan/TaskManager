using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TaskManager.DB;
using TaskManager.DB.Models;
using TaskManager.Utilities;
using Task = TaskManager.DB.Models.Task;

namespace TaskManager.ViewModels;

internal class CommentWindowViewModel(Task task) : INotifyPropertyChanged
{
  private ObservableCollection<Comment> _comments;
  public ObservableCollection<Comment> Comments
  {
    get => this._comments;
    set
    {
      this._comments = value;
      this.OnPropertyChanged();
    }
  }
  private ICommand? _loadedCommand;
  public ICommand LoadedCommand => this._loadedCommand ??= new RelayCommand(async f =>
  {
    var comments = await DBAPI.GetTaskComments(task);
    this.Comments = new ObservableCollection<Comment>(comments);
  }, CanExecute);
  private ICommand? _addButtonClick;
  public ICommand AddButtonClick => this._addButtonClick ??= new RelayCommand(async f =>
  {

  }, CanExecute);
  private static bool CanExecute(object parameter)
  {
    return App.CurrentUser is not null;
  }
  public event PropertyChangedEventHandler? PropertyChanged;
  public void OnPropertyChanged([CallerMemberName] string prop = "")
  {
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
  }
}