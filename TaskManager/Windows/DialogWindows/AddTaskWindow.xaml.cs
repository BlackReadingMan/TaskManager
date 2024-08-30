using TaskManager.DB.Models;

namespace TaskManager.Windows.DialogWindows;

/// <summary>
/// Логика взаимодействия для AddTaskWindow.xaml
/// </summary>
public partial class AddTaskWindow : DialogWindow<Task>
{
  public AddTaskWindow()
  {
    this.InitializeComponent();
  }
}