using System.Windows.Controls;
using Task = TaskManager.DB.Models.Task;

namespace TaskManager.UserControls
{
  /// <summary>
  /// Логика взаимодействия для TaskUC.xaml
  /// </summary>
  public partial class TaskUc : UserControl
  {
    public Task CurrentTask { get; private set; }
    public TaskUc(Task task)
    {
      this.CurrentTask = task;
      this.InitializeComponent();
    }
  }
}
