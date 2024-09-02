using System.ComponentModel;

namespace TaskManager.DB.Enums;

public enum TaskState
{
  [Description("Не взята в работу")]
  NotAcceptedForWork = 0,
  [Description("Взята в работу")]
  TakenIntoWork = 1,
  [Description("Проверяется")]
  UnderReview = 2,
  [Description("Выполнена")]
  Completed = 3
}