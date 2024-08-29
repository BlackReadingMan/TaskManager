using System.Threading.Tasks;
using System.Windows.Input;
using TaskManager.DB;
using TaskManager.Utilities;
using TaskManager.Windows;

namespace TaskManager.ViewModels;

internal sealed class LoginWindowViewModel : BaseViewModel
{
  private LoginWindow? _window;

  private string _login = "";

  public string Login
  {
    set => this._login = value;
  }

  private string _password = "";

  public string PassWord
  {
    get => this._password;
    set
    {
      this._password = value;
      this.OnPropertyChanged();
    }
  }
  private ICommand? _userCheck;
  public ICommand UserCheck => this._userCheck ??= new RelayCommand(async f =>
  {
    var user = await DBAPI.CheckAuthorization(this._login, this._password);
    if (user is not null && this._window is not null)
    {
      this._window.AuthorizedUser = user;
      this._window.Close();
    }
    else
    {
      this.PassWord = string.Empty;
    }
  });

  protected override Task OnLoad(object sender)
  {
    if (sender is LoginWindow loginWindow)
    {
      this._window = loginWindow;
    }
    return Task.Delay(1);
  }

  protected override bool CanExecute(object parameter)
  {
    return true;
  }
}