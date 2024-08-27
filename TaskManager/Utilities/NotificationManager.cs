using System.Net.Mail;
using System.Net;
using DB;
using DB.Models;
using Task = DB.Models.Task;

namespace TaskManager.Utilities
{
  internal static class NotificationManager
  {
    private const string EmailSmtp = "smtp.mail.ru";
    private const int SmtpPort = 25;
    private const string Email = "";
    private const string Password = "";
    private const string Subject = "Изменение статуса задачи.";
    private const string Body = "";

    public static async void NotifyUsersAsync(Task task, int newStatus)
    {
      using var sc = new SmtpClient($"{EmailSmtp}",SmtpPort)
      {
        EnableSsl = true,
        DeliveryMethod = SmtpDeliveryMethod.Network,
        UseDefaultCredentials = false,
        Credentials = new NetworkCredential($"{Email}", $"{Password}")
      };

      var users = await GetUsers(task);
      foreach (var user in users)
      {
        using var mm = new MailMessage($"{user.Name} <{Email}>", $"{user.Email}");
        mm.Subject = $"{Subject}";
        mm.Body = $"{Body}";
        mm.IsBodyHtml = false;
        sc.Send(mm);
      }
    }

    private static async Task<List<User>> GetUsers(Task task)
    {
      var observers = await DBAPI.GetTaskObservers(task);
      var users = observers.Select(observer => observer.IdUserNavigation).ToList();
      if (task.ResponsibleNavigation != null) users.Add(task.ResponsibleNavigation);
      return users;
    }
  }
}
