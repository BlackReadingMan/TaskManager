using System.Windows.Input;
using TaskManager.DataOut;
using TaskManager.DB;
using TaskManager.DB.Enums;
using TaskManager.DB.Models;
using TaskManager.Windows.MainWindows;
using Xceed.Document.NET;
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

  protected override async System.Threading.Tasks.Task UpdateData()
  {
    if (this.CurrentClass.Responsible.HasValue && this.CurrentClass.Responsible == App.CurrentUser.Id)
    {
      this.SubscribeButton.IsEnabled = false;
      this.SubscribeButton.Content = "Отслеживать";
    }
    else
    {
      this.SubscribeButton.IsEnabled = true;
      this._observer = await DBAPI.IsUserObserveTask(this.CurrentClass, App.CurrentUser);
      this.SubscribeButton.Content = this._observer is not null ? "Не отслеживать" : "Отслеживать";
    }
    this.TaskName.Content = this.CurrentClass.Name;
    this.Description.Text = this.CurrentClass.Description;
    this.CreationTime.Content = this.CurrentClass.CreationTime;
    this.Responsible.Content = this.CurrentClass.Responsible is null ? "Не назначен" : (await DBAPI.GetItem<User>(this.CurrentClass.Responsible.Value))?.Login;
    this.DeadLine.Content = this.CurrentClass.Deadline;
    this.Status.Content = this.CurrentClass.Status.EnumDescription();
    this.Priority.Content = this.CurrentClass.Priority.EnumDescription();
    this.SetStateChangerContent();
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
      this._observer = null;
    }
    else
    {
      this._observer = new Observer { IdTask = this.CurrentClass.Id, IdUser = App.CurrentUser.Id };
      await DBAPI.AddItem(this._observer);
      this.SubscribeButton.Content = "Не отслеживать";
    }
  }

  private void SetStateChangerContent()
  {
    switch (this.CurrentClass.Status)
    {
      case TaskStatus.NotAcceptedForWork:
        this.StateChanger.Content = "Взять в работу";
        this.StateChanger.IsEnabled = !this.IsResponsible();
        break;
      case TaskStatus.TakenIntoWork:
        this.StateChanger.Content = "Отправить на проверку";
        this.StateChanger.IsEnabled = !this.IsResponsible();
        break;
      case TaskStatus.UnderReview:
        this.StateChanger.Content = "Подтвердить выполнение";
        this.StateChanger.IsEnabled = this.IsResponsible();
        break;
      case TaskStatus.Completed:
        this.StateChanger.Content = "Завершена";
        this.StateChanger.IsEnabled = false;
        break;
    }
  }

  private bool IsResponsible()
  {
    return this.CurrentClass.Responsible == App.CurrentUser.Id;
  }
  private async void StateChanger_Click(object sender, System.Windows.RoutedEventArgs e)
  {
    this.CurrentClass.Status++;
    this.SetStateChangerContent();
    await NotificationManager.NotifyUsersAsync(this.CurrentClass, App.CurrentUser);
    await DBAPI.EditItem(this.CurrentClass);

  }
}