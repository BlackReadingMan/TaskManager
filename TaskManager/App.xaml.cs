using System.Windows;
using DB.Models;
using TaskManager.Windows;

namespace TaskManager
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    public static User? CurrentUser;
    private void Application_Startup(object sender, StartupEventArgs e)
    {
      try
      {
        if (new LoginWindow().ShowDialog() != true) return;
        MessageBox.Show("Пользователь успешно авторизован.");
        new MainWindow().ShowDialog();
      }
      finally
      {
        Shutdown();
      }
    }
  }

}
