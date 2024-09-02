using System;
using System.Collections.Generic;
using TaskManager.DB.Enums;

namespace TaskManager.DB.Models;

public partial class Task
{
  public int Id { get; set; }

  public string Name { get; set; } = null!;

  public string Description { get; set; } = null!;

  public DateOnly? Deadline { get; set; }

  public TaskPriority Priority { get; set; }

  public int? Responsible { get; set; }

  public TaskState Status { get; set; }

  public DateOnly CreationTime { get; set; }

  public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

  public virtual ICollection<Observer> Observers { get; set; } = new List<Observer>();

  public virtual User? ResponsibleNavigation { get; set; }
}
