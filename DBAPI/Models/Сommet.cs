namespace DB.Models;
/// <summary>
/// Класс комментариев.
/// </summary>
public partial class Сommet
{
  public int Id { get; set; }

  public int IdCreator { get; set; }

  public int? IdTask { get; set; }

  public string? Description { get; set; }

  public virtual User IdCreatorNavigation { get; set; } = null!;

  public virtual Task? IdTaskNavigation { get; set; }
}
