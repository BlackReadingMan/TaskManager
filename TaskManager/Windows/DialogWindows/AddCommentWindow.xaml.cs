using TaskManager.DB.Models;

namespace TaskManager.Windows.DialogWindows;

/// <summary>
/// Логика взаимодействия для AddCommentWindow.xaml
/// </summary>
public partial class AddCommentWindow : DialogWindow<Comment>
{
  public AddCommentWindow()
  {
    this.InitializeComponent();
  }
}