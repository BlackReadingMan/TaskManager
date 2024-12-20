﻿using System;

namespace TaskManager.DB.Models;

/// <summary>
/// Класс комментариев.
/// </summary>
public partial class Comment
{
  public int Id { get; set; }

  public int IdCreator { get; set; }

  public int IdTask { get; set; }

  public string? Description { get; set; }

  public DateOnly CreationTime { get; set; }

  public virtual User IdCreatorNavigation { get; set; } = null!;

  public virtual Task IdTaskNavigation { get; set; } = null!;
}
