using System.Linq;
using System.Net;
using System.Net.Mail;
using TaskManager.DB;
using TaskManager.DB.Models;
using Xceed.Document.NET;
using Task = System.Threading.Tasks.Task;

namespace TaskManager.DataOut;

public static class NotificationManager
{
  private const string EmailSmtp = "smtp.office365.com";
  private const int SmtpPort = 587;
  //нужно создать почту
  private const string Email = "TaskManagere@outlook.com";
  private const string Password = "JCW-XEr-F4x-t7Z";
  private const string Subject = "Изменение статуса задачи.";

  public static async Task NotifyUsersAsync(DB.Models.Task task, User changer)
  {
    await NotifyObserversAsync(task, changer);
    await NotifyResponsibleAsync(task, changer);
  }

  private static async Task NotifyObserversAsync(DB.Models.Task task, User changer)
  {
    var users = await DBAPI.GetTaskObservers(task);
    foreach (var user in users.Where(user => user.Email is not null))
    {
      using var mm = new MailMessage($"{user.Name} <{Email}>", $"{user.Email}");
      mm.Subject = $"{Subject}";
      mm.Body = $"У отслеживаемой вами задачи \"{task.Name}\", пользователь \"{changer.Login}\" изменил стаус с \"{(task.Status - 1).EnumDescription()}\" на \"{task.Status.EnumDescription()}\".";
      mm.IsBodyHtml = false;
      using var sc = new SmtpClient($"{EmailSmtp}", SmtpPort);
      sc.EnableSsl = true;
      sc.DeliveryMethod = SmtpDeliveryMethod.Network;
      sc.UseDefaultCredentials = false;
      sc.Credentials = new NetworkCredential($"{Email}", $"{Password}");
      await sc.SendMailAsync(mm);
    }
  }

  private static async Task NotifyResponsibleAsync(DB.Models.Task task, User changer)
  {
    if (task.Responsible is null) 
      return;
    var user = await DBAPI.GetItem<User>(task.Responsible.Value);
    if (user?.Email is null) 
      return;
    using var mm = new MailMessage($"{user.Name} <{Email}>", $"{user.Email}");
    mm.Subject = $"{Subject}";
    mm.Body = $"У вашей задачи \"{task.Name}\", пользователь \"{changer.Login}\" изменил стаус с \"{(task.Status - 1).EnumDescription()}\" на \"{task.Status.EnumDescription()}\".";
    mm.IsBodyHtml = false;
    using var sc = new SmtpClient($"{EmailSmtp}", SmtpPort);
    sc.EnableSsl = true;
    sc.DeliveryMethod = SmtpDeliveryMethod.Network;
    sc.UseDefaultCredentials = false;
    sc.Credentials = new NetworkCredential($"{Email}", $"{Password}");
    await sc.SendMailAsync(mm);
  }
}