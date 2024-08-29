using System.Windows;
using TaskManager.ViewModels;
using Task = TaskManager.DB.Models.Task;

namespace TaskManager.Windows;

/// <summary>
/// Логика взаимодействия для CommentWindow.xaml
/// </summary>
public partial class CommentWindow : Window
{
  public CommentWindow(Task task)
  {
    this.InitializeComponent();
    this.DataContext = new CommentWindowViewModel(task);
  }
}