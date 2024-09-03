using System.ComponentModel;

namespace TaskManager.DB.Enums;

public enum TaskSortParameter
{
  [Description("Id")]
  Id,
  [Description("Дэдлайн")]
  Deadline,
  [Description("Приоритет")]
  Priority,
  [Description("Статус")]
  Status,
  [Description("Дата создания")]
  CreationTime
}