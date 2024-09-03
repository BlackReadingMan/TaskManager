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

  public virtual ICollection<Comment> Comments { get; set; } = [];

  public virtual ICollection<Observer> Observers { get; set; } = [];

  public virtual User? ResponsibleNavigation { get; set; }

  public static Dictionary<TaskSortParameter, Comparison<Task>> PropertyComparers = new()
  {
    { TaskSortParameter.Id, CompareById },
    { TaskSortParameter.Deadline, CompareByDeadline },
    { TaskSortParameter.Priority, CompareByPriority },
    { TaskSortParameter.Status, CompareByStatus },
    { TaskSortParameter.CreationTime, CompareByCreationTime }
  };

  private static int CompareById(Task x, Task y)
  {
    return x.Id.CompareTo(y.Id);
  }

  private static int CompareByDeadline(Task x, Task y)
  {
    if (x.Deadline == null && y.Deadline == null)
      return 0;
    else if (x.Deadline == null)
      return -1;
    else if (y.Deadline == null)
      return 1;
    else
      return x.Deadline.Value.CompareTo(y.Deadline);
  }

  private static int CompareByPriority(Task x, Task y)
  {
    return x.Priority.CompareTo(y.Priority);
  }

  private static int CompareByStatus(Task x, Task y)
  {
    return x.Status.CompareTo(y.Status);
  }

  private static int CompareByCreationTime(Task x, Task y)
  {
    return x.CreationTime.CompareTo(y.CreationTime);
  }
}
