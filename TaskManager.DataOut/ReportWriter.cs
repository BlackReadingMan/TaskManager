using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.DB;
using TaskManager.DB.Models;
using Xceed.Document.NET;
using Xceed.Words.NET;
using TaskEntity = TaskManager.DB.Models.Task;
using Task = System.Threading.Tasks.Task;

namespace TaskManager.DataOut;

public class ReportWriter
{
    private readonly IEnumerable<TaskEntity> _tasks;

    public ReportWriter(IEnumerable<TaskEntity> tasks)
    {
        _tasks = tasks;
    }

    public async Task WriteReport(string path)
    {
        var tasksList = _tasks.ToList();
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

            doc.InsertParagraph(task.Name ?? "Отсутствует") 
                .FontSize(16)
                .Bold()
                .SpacingAfter(10);

            var table = doc.AddTable(9, 2);
            table.Design = TableDesign.LightShadingAccent1;

            table.Rows[0].Cells[0].Paragraphs[0].Append("Время создания");
            table.Rows[0].Cells[1].Paragraphs[0].Append(task.CreationTime.ToString()); 

            table.Rows[1].Cells[0].Paragraphs[0].Append("Дедлайн");
            table.Rows[1].Cells[1].Paragraphs[0].Append(task.Deadline?.ToString() ?? "Отсутствует"); 

            table.Rows[2].Cells[0].Paragraphs[0].Append("Описание задачи");
            table.Rows[2].Cells[1].Paragraphs[0].Append(task.Description ?? "Отсутствует"); 

            table.Rows[3].Cells[0].Paragraphs[0].Append("Название задачи");
            table.Rows[3].Cells[1].Paragraphs[0].Append(task.Name ?? "Отсутствует");

            table.Rows[4].Cells[0].Paragraphs[0].Append("Приоритет задачи");
            var priority = GetStringPriority(task.Priority);
            table.Rows[4].Cells[1].Paragraphs[0].Append(priority);

            table.Rows[5].Cells[0].Paragraphs[0].Append("Ответственный за задачу");
            var responsible = task.Responsible == null ? "Отсутствует" : await GetResponsible(task.Responsible.Value);
            table.Rows[5].Cells[1].Paragraphs[0].Append(responsible);

            table.Rows[6].Cells[0].Paragraphs[0].Append("Комментарии");
            var comments = await DBAPI.GetTaskComments(task);
            if (comments.Count > 0)
            {
                await GenerateCommentInfo(comments, table);
            }
            else
            {
                table.Rows[6].Cells[1].Paragraphs[0].Append("Отсутствуют");
            }

            doc.InsertParagraph().InsertTableAfterSelf(table);

            doc.InsertParagraph()
                .AppendLine(new string('-', 50))
                .FontSize(12)
                .SpacingAfter(10);
        }

        doc.Save();
    }

    private async Task GenerateCommentInfo(List<Comment> comments, Table table)
    {
        for (int i = 0; i < comments.Count; i++)
        {
            var str = new StringBuilder();
            var comment = comments[i];
            var commentCreator = await DBAPI.GetItem<User>(comment.IdCreator);
            var creationTime = comment.CreationTime.ToString();
            var description = comment.Description ?? "Отсутствует"; 
            var creatorName = commentCreator?.Name ?? "Неизвестный пользователь"; 

            str.Append($"{creationTime} - {description} - {creatorName}");

            if (i > 0)
            {
                table.InsertRow();
            }

            table.Rows[6 + i].Cells[1].Paragraphs[0].Append(str.ToString());
        }
    }

    private async Task<string> GetResponsible(int id)
    {
        var responsible = await DBAPI.GetItem<User>(id);
        return responsible?.Name ?? "Неизвестный пользователь";
    }

    private string GetStringPriority(int? priority)
    {
        if (!priority.HasValue)
            return "Отсутствует";

        return priority.Value switch
        {
            0 => "Низкий",
            1 => "Средний",
            2 => "Высокий",
            _ => "Неизвестный приоритет"
        };
    }
}
