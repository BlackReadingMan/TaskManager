using System.Collections;
using Microsoft.EntityFrameworkCore;
using TaskManager.DB.Models;
using Task = System.Threading.Tasks.Task;

namespace TaskManager.DB;
/// <summary>
/// Класс для работы с БД.
/// </summary>
public static class DBAPI
{
  //Scaffold-DbContext "Host=localhost;Port=5432;Database=TaskManager;Username=postgres;Password=1111" Npgsql.EntityFrameworkCore.PostgreSQL -Force -Context TaskManagerContext -OutputDir Models
  public static async Task LoadTable<T>(IList table) where T : class
  {
    await using var context = new TaskManagerContext();
    var list = await context.Set<T>().ToListAsync();
    table.Clear();
    if (list.Count == 0) return;
    foreach (var t in list)
      table.Add(t);
  }

  public static async Task AddItem<T>(T item) where T : class
  {
    await using var context = new TaskManagerContext();
    await context.AddAsync(item);
    await context.SaveChangesAsync();
  }

  public static async Task EditItem<T>(T item) where T : class
  {
    await using var context = new TaskManagerContext();
    context.Update(item);
    await context.SaveChangesAsync();
  }

  public static async Task RemoveItem<T>(T item) where T : class
  {
    await using var context = new TaskManagerContext();
    context.Remove(item);
    await context.SaveChangesAsync();
  }
  public static async Task<T?> GetItem<T>(int id) where T : class
  {
    await using var context = new TaskManagerContext();
    return await context.FindAsync<T>(id);
  }
  public static async Task<User?> CheckAuthorization(string login, string password)
  {
    await using var context = new TaskManagerContext();
    return context.Users.FirstOrDefault(x => x.Login == login && x.Password == password);
  }

  public static async Task<List<Comment>> GetTaskComments(Models.Task task)
  {
    await using var context = new TaskManagerContext();
    return await context.Comments.Where(x => x.IdTask == task.Id).ToListAsync();
  }
  public static async Task<List<User>> GetTaskObservers(Models.Task task)
  {
    await using var context = new TaskManagerContext();
    return await context.Observers.Where(x => x.IdTask == task.Id).
      Join(context.Users, f => f.IdUser, s => s.Id,
      (f, s) => new User
      {
        Name = s.Name,
        Email = s.Email,
      }).ToListAsync();
  }
}

