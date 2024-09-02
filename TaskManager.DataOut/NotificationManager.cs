using System.Net.Mail;
using TaskManager.DB;
using TaskManager.DB.Enums;
using TaskManager.DB.Models;
using Task = System.Threading.Tasks.Task;

namespace TaskManager.DataOut;

public static class NotificationManager
{
  private const string EmailSmtp = "smtp.mail.ru";
  private const int SmtpPort = 25;
  //нужно создать почту
  private const string Email = "";
  private const string Password = "";
  private const string Subject = "Изменение статуса задачи.";
  private const string BodyForObserver = "Test";
  private const string BodyForResponsible = "Test";

  public static async Task NotifyUsersAsync(DB.Models.Task task, TaskState newStatus)
  {
    //using var sc = new SmtpClient($"{EmailSmtp}", SmtpPort)
    //{
    //  EnableSsl = true,
    //  DeliveryMethod = SmtpDeliveryMethod.Network,
    //  UseDefaultCredentials = false,
    //  Credentials = new NetworkCredential($"{Email}", $"{Password}")
    //};
    //await NotifyObserversAsync(task, newStatus, sc);
    //await NotifyResponsibleAsync(task, newStatus, sc);
  }

  private static async Task NotifyObserversAsync(DB.Models.Task task, TaskState newStatus, SmtpClient sc)
  {
    var users = await DBAPI.GetTaskObservers(task);
    foreach (var user in users)
    {
      using var mm = new MailMessage($"{user.Name} <{Email}>", $"{user.Email}");
      mm.Subject = $"{Subject}";
      mm.Body = $"{BodyForObserver}";
      mm.IsBodyHtml = false;
      sc.Send(mm);
    }
  }

  private static async Task NotifyResponsibleAsync(DB.Models.Task task, TaskState newStatus, SmtpClient sc)
  {
    if (task.Responsible is null) return;
    var user = await DBAPI.GetItem<User>((int)task.Responsible);
    if (user == null) return;
    using var mm = new MailMessage($"{user.Name} <{Email}>", $"{user.Email}");
    mm.Subject = $"{Subject}";
    mm.Body = $"{BodyForResponsible}";
    mm.IsBodyHtml = false;
    sc.Send(mm);
  }
}