namespace TaskManager.DB.Models;

/// <summary>
/// Класс пользователя.
/// </summary>
public partial class User
{
  public int Id { get; set; }

  public string Name { get; set; } = null!;

  public string Login { get; set; } = null!;

  public string Password { get; set; } = null!;

  public string Email { get; set; } = null!;

  public bool Root { get; set; }

  public virtual Comment? Comment { get; set; }

  public virtual ICollection<Observer> Observers { get; set; } = new List<Observer>();

  public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
