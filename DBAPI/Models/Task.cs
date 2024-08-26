namespace DB.Models;

/// <summary>
/// Класс задач.
/// </summary>
public partial class Task
{
  public int Id { get; set; }

  public string? Name { get; set; }

  public string? Description { get; set; }

  public DateOnly? Deadline { get; set; }

  public int? Priority { get; set; }

  public int? Responsible { get; set; }

  public int? Status { get; set; }

  public virtual Observer? Observer { get; set; }

  public virtual User? ResponsibleNavigation { get; set; }

  public virtual ICollection<Сommet> Сommets { get; set; } = new List<Сommet>();
}
