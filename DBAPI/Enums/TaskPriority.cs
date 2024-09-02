using System.ComponentModel;

namespace TaskManager.DB.Enums;

public enum TaskPriority
{
  [Description("Не задан")]
  NotSet = 0,
  [Description("Низкий")]
  Low = 1,
  [Description("Средний")]
  Medium = 2,
  [Description("Высокий")]
  High = 3,
  [Description("Критический")]
  Critical = 4,
}