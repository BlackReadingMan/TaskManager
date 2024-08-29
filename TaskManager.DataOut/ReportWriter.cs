using System.Collections.Generic;
using Xceed.Words.NET;
using Task = TaskManager.DB.Models.Task;

namespace TaskManager.DataOut;

public class ReportWriter(IEnumerable<Task> tasks)
{
  public void WriteReport(string path)
  {
    var doc = DocX.Create(path);
    //пишет некто сигезмунд
  }
}