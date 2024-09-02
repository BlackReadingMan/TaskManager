using TaskManager.DB;
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

  protected override async System.Threading.Tasks.Task UpdateData()
  {
    var user = await DBAPI.GetItem<User>(this.CurrentClass.IdCreator);
    this.CreatorLogin.Content = user is null ? "Пользователь удалён" : user.Login;
    this.CreationTime.Content = this.CurrentClass.CreationTime;
    this.Description.Text = this.CurrentClass.Description;
  }
}