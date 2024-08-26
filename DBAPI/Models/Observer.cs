namespace DB.Models;
/// <summary>
/// Класс наблюдателя.
/// </summary>
public partial class Observer
{
  public int Id { get; set; }

  public int IdTask { get; set; }

  public int? IdUser { get; set; }

  public virtual Task IdTaskNavigation { get; set; } = null!;

  public virtual User? IdUserNavigation { get; set; }
}
