﻿using System.Collections.ObjectModel;
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
    this.CurrentCollection = new ObservableCollection<Comment>(comments);
  }

  protected override void AddSubject()
  {
    var window = new AddCommentWindow();
    window.ShowDialog();
    var comment = window.ReturnData;
    if (comment is null) return;
    comment.IdTask = task.Id;
    this.CurrentCollection.Add(comment);
    DBAPI.AddItem(comment);
  }
}