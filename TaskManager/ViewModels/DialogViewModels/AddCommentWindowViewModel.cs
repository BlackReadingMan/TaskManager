using System;
using TaskManager.DB.Models;
using TaskManager.ViewModels.BaseViewModels;

namespace TaskManager.ViewModels.DialogViewModels;

internal class AddCommentWindowViewModel : DialogWindowViewModel<Comment>
{
  private string? _description;
  public string Description
  {
    set => this._description = value;
  }
  protected override void AddSubject()
  {
    var newComment = new Comment
    {
      Description = this._description,
      IdCreator = App.CurrentUser.Id,
      CreationTime = DateOnly.FromDateTime(DateTime.Now)
    };
    this.DialogResult = newComment;
  }

  protected override bool CanExecute(object parameter)
  {
    return this._description is not null;
  }
}