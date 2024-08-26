using System.Windows.Controls;
using Task = DB.Models.Task;

namespace TaskManager.UserControls
{
  /// <summary>
  /// Логика взаимодействия для TaskUC.xaml
  /// </summary>
  public partial class TaskUC : UserControl
  {
    public TaskUC(Task task)
    {
      this.InitializeComponent();
    }
  }
}
