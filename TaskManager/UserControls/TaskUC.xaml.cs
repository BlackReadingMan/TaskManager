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
    this._observer = await DBAPI.IsUserObserveTask(this.CurrentClass, App.CurrentUser);
    this.SubscribeButton.Content = this._observer is not null ? "Не отслеживать" : "Отслеживать";
    this.TaskName.Content = this.CurrentClass.Name;
    this.Description.Text = this.CurrentClass.Description;
    this.CreationTime.Content = this.CurrentClass.CreationTime;
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
    }
    else
    {
      await DBAPI.AddItem(new Observer { IdTask = this.CurrentClass.Id, IdUser = App.CurrentUser.Id });
      this.SubscribeButton.Content = "Не отслеживать";
    }
  }

  private void SetStateChangerContent()
  {
    switch (this.CurrentClass.Status)
    {
      case TaskState.NotAcceptedForWork:
        this.StateChanger.Content = "Взять в работу";
        this.StateChanger.IsEnabled = !this.IsResponsible();
        break;
      case TaskState.TakenIntoWork:
        this.StateChanger.Content = "Отправить на проверку";
        this.StateChanger.IsEnabled = !this.IsResponsible();
        break;
      case TaskState.UnderReview:
        this.StateChanger.Content = "Подтвердить выполнение";
        this.StateChanger.IsEnabled = this.IsResponsible();
        break;
      case TaskState.Completed:
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
    await NotificationManager.NotifyUsersAsync(this.CurrentClass, ++this.CurrentClass.Status);
    await DBAPI.EditItem(this.CurrentClass);
    this.SetStateChangerContent();
  }
}