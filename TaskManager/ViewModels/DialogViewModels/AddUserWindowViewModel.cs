using System.Net.Mail;
using TaskManager.DB;
using TaskManager.DB.Models;
using TaskManager.ViewModels.BaseViewModels;

namespace TaskManager.ViewModels.DialogViewModels;

internal class AddUserWindowViewModel : DialogWindowViewModel<User>
{
  private string _name = string.Empty;

  public string Name
  {
    set => this._name = value;
  }

  private string _login = string.Empty;

  public string Login
  {
    set => this._login = value;
  }

  private string _password = string.Empty;

  public string Password
  {
    set => this._password = value;
  }

  private string _email = string.Empty;

  public string Email
  {
    set => this._email = value;
  }

  protected override async void AddSubject()
  {
    if (!MailAddress.TryCreate(this._email, out _))
    {
      App.ShowError("Некоректный адрес почты.");
      return;
    }
    if (await DBAPI.IsLoginExists(this._login))
    {
      App.ShowError("Этот логин занят.");
      return;
    }
    var user = new User
    {
      Name = this._name,
      Login = this._login,
      Password = this._password,
      Email = this._email,
      Root = false
    };
    this.DialogResult = user;
  }

  protected override bool CanAddExecute(object parameter)
  {
    return this._name != string.Empty && this._login != string.Empty;
  }
}