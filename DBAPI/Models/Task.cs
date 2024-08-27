namespace DB.Models;

public partial class Task
{
  public int Id { get; set; }

  public string Name { get; set; } = null!;

  public string Description { get; set; } = null!;

  public DateOnly? Deadline { get; set; }

  public int Priority { get; set; }

  public int? Responsible { get; set; }

  public int Status { get; set; }

  public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

  public virtual Observer? Observer { get; set; }

  public virtual User? ResponsibleNavigation { get; set; }
}
