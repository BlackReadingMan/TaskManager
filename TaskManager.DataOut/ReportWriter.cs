using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.DB;
using TaskManager.DB.Enums;
using TaskManager.DB.Models;
using Xceed.Document.NET;
using Xceed.Words.NET;
using Task = System.Threading.Tasks.Task;
using TaskEntity = TaskManager.DB.Models.Task;

namespace TaskManager.DataOut;

public class ReportWriter(IEnumerable<TaskEntity> tasks)
{
  public async Task WriteReport(string path)
  {
    var tasksList = tasks.ToList();
    var doc = DocX.Create(path);

    doc.InsertParagraph("Отчет по задачам")
        .FontSize(20)
        .Bold()
        .Alignment = Alignment.center;

    foreach (var task in tasksList)
    {
      doc.InsertParagraph()
          .AppendLine(new string('-', 50))
          .FontSize(12)
          .SpacingAfter(10);

            doc.InsertParagraph(task.Name) 
          .FontSize(16)
          .Bold()
          .SpacingAfter(10);

      var table = doc.AddTable(9, 2);
      table.Design = TableDesign.LightShadingAccent1;

      table.Rows[0].Cells[0].Paragraphs[0].Append("Время создания");
      table.Rows[0].Cells[1].Paragraphs[0].Append(task.CreationTime.ToString());

      table.Rows[1].Cells[0].Paragraphs[0].Append("Дедлайн");
      table.Rows[1].Cells[1].Paragraphs[0].Append(task.Deadline?.ToString() ?? "Не назначен");

      table.Rows[2].Cells[0].Paragraphs[0].Append("Описание задачи");
            table.Rows[2].Cells[1].Paragraphs[0].Append(task.Description); 

      table.Rows[3].Cells[0].Paragraphs[0].Append("Название задачи");
            table.Rows[3].Cells[1].Paragraphs[0].Append(task.Name);

      table.Rows[4].Cells[0].Paragraphs[0].Append("Приоритет задачи");
      var priority = GetStringPriority(task.Priority);
      table.Rows[4].Cells[1].Paragraphs[0].Append(priority);

      table.Rows[5].Cells[0].Paragraphs[0].Append("Ответственный за задачу");
      var responsible = task.Responsible == null ? "Не назначен" : await GetResponsibleName(task.Responsible.Value);
      table.Rows[5].Cells[1].Paragraphs[0].Append(responsible);

      table.Rows[6].Cells[0].Paragraphs[0].Append("Статус задачи");
      var status = GetStringStatus(task.Status);
      table.Rows[6].Cells[1].Paragraphs[0].Append(status);

      doc.InsertParagraph().InsertTableAfterSelf(table);

      doc.InsertParagraph()
          .AppendLine(new string('-', 50))
          .FontSize(12)
          .SpacingAfter(10);
    }

    doc.Save();
  }

  private static async Task<string> GetResponsibleName(int id)
  {
    var responsible = await DBAPI.GetItem<User>(id);
    return responsible?.Name ?? "Неизвестный пользователь";
  }

  private static string GetStringPriority(TaskPriority priority)
  {
    var result = priority switch
    {
      >= 0 when (int)priority <= 4 => priority.EnumDescription(),
      _ => "Неизвестный приоритет"
    };
    return result;
  }
  private static string GetStringStatus(TaskState status)
  {
    var result = status switch
    {
      >= 0 when (int)status <= 3 => status.EnumDescription(),
      _ => "Неизвестный приоритет"
    };
    return result;
  }
}
