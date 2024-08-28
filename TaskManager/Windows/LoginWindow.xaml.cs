using System.Windows;
using TaskManager.DB.Models;

namespace TaskManager.Windows
{
  /// <summary>
  /// Логика взаимодействия для LoginWindow.xaml
  /// </summary>
  public partial class LoginWindow : Window
  {
    public LoginWindow()
    {
      this.InitializeComponent();
    }

    public User? AuthorizedUser { get; set; }
  }
}
