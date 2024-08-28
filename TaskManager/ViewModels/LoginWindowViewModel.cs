using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TaskManager.DB;
using TaskManager.Utilities;
using TaskManager.Windows;

namespace TaskManager.ViewModels
{
  internal class LoginWindowViewModel : INotifyPropertyChanged
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

    private ICommand? _loadedCommand;
    public ICommand LoadedCommand => this._loadedCommand ??= new RelayCommand(f =>
    {
      if (f is LoginWindow loginWindow)
      {
        this._window = loginWindow;
      }
    });
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
    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string prop = "")
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
  }
}
