using TaskManager.DB.Models;

namespace TaskManager.Windows.DialogWindows;

/// <summary>
/// Логика взаимодействия для LoginWindow.xaml
/// </summary>
public partial class LoginWindow : DialogWindow<User>
{
  public LoginWindow()
  {
    this.InitializeComponent();
  }
}