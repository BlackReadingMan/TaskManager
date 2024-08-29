using System.Collections.ObjectModel;
using TaskManager.DB;
using TaskManager.DB.Models;
using Task = TaskManager.DB.Models.Task;

namespace TaskManager.ViewModels;

internal sealed class CommentWindowViewModel(Task task) : ListViewModel<Comment>
{
  protected override async System.Threading.Tasks.Task OnLoad(object sender)
  {
    var comments = await DBAPI.GetTaskComments(task);
    this.CurrentCollection = new ObservableCollection<Comment>(comments);
  }
}