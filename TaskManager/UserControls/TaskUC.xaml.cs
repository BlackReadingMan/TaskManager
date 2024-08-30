using System.Windows.Input;
using TaskManager.Windows.MainWindows;
using Task = TaskManager.DB.Models.Task;

namespace TaskManager.UserControls;

/// <summary>
/// Логика взаимодействия для TaskUC.xaml
/// </summary>
public sealed partial class TaskUc : ListedUc<Task>
{
  public TaskUc()
  {
    this.InitializeComponent();
  }
  protected override void UpdateData()
  {
    this.Name.Content = this.CurrentClass.Name;
    this.Description.Text = this.CurrentClass.Description;
    this.CreationTime.Content = this.CurrentClass.CreationTime;
  }

  private void TaskUc_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
  {
    new CommentWindow(this.CurrentClass).ShowDialog();
  }
}