using System.Collections;
using DB.Models;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace DB;
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

  public static async Task<bool> CheckAuthorization(string login, string password)
  {
    await using var context = new TaskManagerContext();
    return context.Users.FirstOrDefault(x => x.Login == login && x.Password == password) != null;
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
}

