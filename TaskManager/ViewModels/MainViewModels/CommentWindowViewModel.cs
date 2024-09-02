using TaskManager.DB;
using TaskManager.DB.Models;
using TaskManager.ViewModels.BaseViewModels;
using TaskManager.Windows.DialogWindows;
using Task = TaskManager.DB.Models.Task;

namespace TaskManager.ViewModels.MainViewModels;

internal sealed class CommentWindowViewModel(Task task) : ListWindowViewModel<Comment>
{
  protected override async System.Threading.Tasks.Task UpdateData(object sender)
  {
    var comments = await DBAPI.GetTaskComments(task);
    this.CurrentCollection.Clear();
    foreach (var comment in comments)
    {
      this.CurrentCollection.Add(comment);
    }
  }

  protected override async void AddSubject()
  {
    var window = new AddCommentWindow();
    window.ShowDialog();
    var comment = window.ReturnData;
    if (comment is null) return;
    comment.IdTask = task.Id;
    this.CurrentCollection.Add(comment);
    await DBAPI.AddItem(comment);
  }

  protected override bool CanAddExecute(object parameter)
  {
    return true;
  }
}