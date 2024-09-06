using System.Windows;
using System.Windows.Input;
using TaskManager.ViewModels.MainViewModels;
using Task = TaskManager.DB.Models.Task;

namespace TaskManager.Windows.MainWindows;

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
    private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (e.ButtonState == MouseButtonState.Pressed)

        {
            DragMove();
        }
    }

    private void Close_Window(object sender, RoutedEventArgs e)
    {
        Close();
    }

}