using System;
using TaskManager.DB.Models;
using TaskManager.ViewModels.BaseViewModels;

namespace TaskManager.ViewModels.DialogViewModels;

internal class AddCommentWindowViewModel : DialogWindowViewModel<Comment>
{
  private string _description = string.Empty;
  public string Description
  {
    set => this._description = value;
  }
  protected override void AddSubject()
  {
    if (_description == string.Empty)
    {
      App.ShowError("Комментарий пуст");
      return;
    }
    var newComment = new Comment
    {
      Description = this._description,
      IdCreator = App.CurrentUser.Id,
      CreationTime = DateOnly.FromDateTime(DateTime.Now)
    };
    this.DialogResult = newComment;
  }

  protected override bool CanAddExecute(object parameter)
  {
    return true;
  }
}