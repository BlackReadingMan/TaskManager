using TaskManager.DB.Models;

namespace TaskManager.UserControls;

/// <summary>
/// Логика взаимодействия для CommentUc.xaml
/// </summary>
public sealed partial class CommentUc : ListedUc<Comment>
{
  public CommentUc()
  {
    this.InitializeComponent();
  }

  protected override void UpdateData()
  {

  }
}