using TaskManager.DB.Models;

namespace TaskManager.Windows.DialogWindows;

/// <summary>
/// Логика взаимодействия для AddUserWindow.xaml
/// </summary>
public partial class AddUserWindow : DialogWindow<User>
{
  public AddUserWindow()
  {
    this.InitializeComponent();
  }
}