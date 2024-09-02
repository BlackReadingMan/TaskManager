using System.Windows;
using TaskManager.DB.Models;
using TaskManager.Windows.DialogWindows;
using TaskManager.Windows.MainWindows;

namespace TaskManager;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
  public static User CurrentUser { get; private set; }
  internal static void ShowError(string message)
  {
    MessageBox.Show(message);
  }
  private void Application_Startup(object sender, StartupEventArgs e)
  {
    try
    {
      var logWindow = new LoginWindow();
      logWindow.ShowDialog();
      if (logWindow.ReturnData is null)
        return;
      CurrentUser = logWindow.ReturnData;
      new MainWindow().ShowDialog();
    }
    finally
    {
      this.Shutdown();
    }
  }
}