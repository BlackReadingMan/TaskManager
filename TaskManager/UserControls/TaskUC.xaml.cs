using System.Windows.Input;
using TaskManager.DB;
using TaskManager.DB.Models;
using TaskManager.Windows.MainWindows;
using Task = TaskManager.DB.Models.Task;

namespace TaskManager.UserControls;

/// <summary>
/// Логика взаимодействия для TaskUC.xaml
/// </summary>
public sealed partial class TaskUc : ListedUc<Task>
{
  private Observer? _observer;
  public TaskUc()
  {
    this.InitializeComponent();
  }
  protected override async void UpdateData()
  {
    this._observer = await DBAPI.IsUserObserveTask(this.CurrentClass, App.CurrentUser);
    this.SubscribeButton.Content = this._observer is not null ? "Не отслеживать" : "Отслеживать";
    this.Name.Content = this.CurrentClass.Name;
    this.Description.Text = this.CurrentClass.Description;
    this.CreationTime.Content = this.CurrentClass.CreationTime;
  }

  private void TaskUc_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
  {
    new CommentWindow(this.CurrentClass).ShowDialog();
  }

  private async void SubscribeButton_Click(object sender, System.Windows.RoutedEventArgs e)
  {
    if (this._observer is not null)
    {
      await DBAPI.RemoveItem(this._observer);
      this.SubscribeButton.Content = "Отслеживать";
    }
    else
    {
      await DBAPI.AddItem(new Observer { IdTask = this.CurrentClass.Id, IdUser = App.CurrentUser.Id });
      this.SubscribeButton.Content = "Не отслеживать";
    }
  }
}