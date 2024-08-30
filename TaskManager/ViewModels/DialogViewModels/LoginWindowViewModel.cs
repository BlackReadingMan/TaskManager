using System.Windows.Input;
using TaskManager.DB;
using TaskManager.DB.Models;
using TaskManager.Utilities;
using TaskManager.ViewModels.BaseViewModels;

namespace TaskManager.ViewModels.DialogViewModels;

internal sealed class LoginWindowViewModel : DialogWindowViewModel<User>
{
  private string _login = string.Empty;

  public string Login
  {
    set => this._login = value;
  }

  private string _password = string.Empty;

  public string PassWord
  {
    set => this._password = value;
  }

  private ICommand? _userCheck;
  public ICommand UserCheck => this._userCheck ??= new RelayCommand(async f =>
  {
    var user = await DBAPI.CheckAuthorization(this._login, this._password);
    if (user is null) return;
    this.DialogResult = user;
  });


  protected override void AddSubject()
  {

  }

  protected override bool CanExecute(object parameter)
  {
    return true;
  }
}