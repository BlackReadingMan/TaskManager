using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskManager.DB.Models;
using Task = System.Threading.Tasks.Task;

namespace TaskManager.DB;
/// <summary>
/// ����� ��� ������ � ��.
/// </summary>
public static class DBAPI
{
  //Scaffold-DbContext "Host=46.147.178.158;Port=5432;Database=TaskManager;Username=tasker;Password=asdfgh" Npgsql.EntityFrameworkCore.PostgreSQL -Force -Context TaskManagerContext -OutputDir Models
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
  public static async Task<bool> IsLoginExists(string login)
  {
    await using var context = new TaskManagerContext();
    return await context.Users.FirstOrDefaultAsync(x => x.Login == login) is not null;
  }
  public static async Task<User?> CheckAuthorization(string login, string password)
  {
    await using var context = new TaskManagerContext();
    return await context.Users.FirstOrDefaultAsync(x => x.Login == login && x.Password == password);
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
  public static async Task<Observer?> IsUserObserveTask(Models.Task task, User user)
  {
    await using var context = new TaskManagerContext();
    return await context.Observers.FirstOrDefaultAsync(x => x.IdTask == task.Id && x.IdUser == user.Id);
  }

  public static async Task<Dictionary<string, int>> GetUsersLogins()
  {
    await using var context = new TaskManagerContext();
    return await context.Users.ToDictionaryAsync(x => x.Login + " - " + x.Name, x => x.Id);
  }
}

