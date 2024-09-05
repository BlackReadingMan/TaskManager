using System.Windows.Input;
using TaskManager.DB;
using TaskManager.DB.Models;
using TaskManager.Utilities;
using TaskManager.ViewModels.BaseViewModels;
using TaskManager.Windows.DialogWindows;

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


  protected override async void AddSubject()
  {
    var window = new AddUserWindow();
    window.ShowDialog();
    var user = window.ReturnData;
    if (user is null) return;
    await DBAPI.AddItem(user);
  }

  protected override bool CanAddExecute(object parameter)
  {
    return true;
  }
}