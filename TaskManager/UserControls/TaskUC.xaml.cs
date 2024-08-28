using System.Windows.Controls;
using Task = TaskManager.DB.Models.Task;

namespace TaskManager.UserControls
{
  /// <summary>
  /// Логика взаимодействия для TaskUC.xaml
  /// </summary>
  public partial class TaskUc : UserControl
  {
    public Task CurrentTask { get; }
    public TaskUc(Task task)
    {
      this.CurrentTask = task;
      this.InitializeComponent();
      this.UpdateData();
    }

    private void UpdateData()
    {
      this.Name.Content = this.CurrentTask.Name;
      this.Description.Text = this.CurrentTask.Description;
      this.CreationTime.Content = this.CurrentTask.CreationTime;
    }
  }
}
