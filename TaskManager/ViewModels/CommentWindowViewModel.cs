using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TaskManager.DB;
using TaskManager.DB.Models;
using TaskManager.Utilities;
using Task = TaskManager.DB.Models.Task;

namespace TaskManager.ViewModels;

internal sealed class CommentWindowViewModel : ListViewModel<Comment>
{
  public CommentWindowViewModel(Task task)
  {
    this._loadedCommand ??= new RelayCommand(async f =>
    {
      var comments = await DBAPI.GetTaskComments(task);
      this.CurrentCollection = new ObservableCollection<Comment>(comments);
    }, CanExecute);
  }
}