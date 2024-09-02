using System.Windows.Input;
using Microsoft.Win32;
using TaskManager.DataOut;
using TaskManager.DB;
using TaskManager.Utilities;
using TaskManager.ViewModels.BaseViewModels;
using TaskManager.Windows.DialogWindows;
using Task = TaskManager.DB.Models.Task;

namespace TaskManager.ViewModels.MainViewModels;

internal sealed class MainWindowViewModel : ListWindowViewModel<Task>
{

  private readonly ReportWriter _reportWriter;

  private Task? _selectedTask;

  public Task SelectedTask
  {
    set => this._selectedTask = value;
  }
  private ICommand? _reportButtonClick;
  public ICommand ReportButtonClick => this._reportButtonClick ??= new RelayCommand(async f =>
  {
    var path = GetPath();
    if (path is null) return;
    await this._reportWriter.WriteReport(path);
  });

  private ICommand? _removeButtonClick;
  public ICommand RemoveButtonClick => this._removeButtonClick ??= new RelayCommand(async f =>
  {
    if(_selectedTask is null) return;
    await DBAPI.RemoveItem(this._selectedTask);
    this.CurrentCollection.Remove(this._selectedTask);
  }, this.CanAddExecute);
  protected override async System.Threading.Tasks.Task UpdateData(object sender)
  {
    await DBAPI.LoadTable<Task>(this.CurrentCollection);
  }

  protected override async void AddSubject()
  {
    var window = new AddTaskWindow();
    window.ShowDialog();
    var task = window.ReturnData;
    if (task is null) return;
    this.CurrentCollection.Add(task);
    await DBAPI.AddItem(task);
  }

  protected override bool CanAddExecute(object parameter)
  {
    return App.CurrentUser.Root;
  }

  private static string? GetPath()
  {
    var dialog = new SaveFileDialog
    {
      FileName = "Tasks",
      DefaultExt = ".docx",
      Filter = "Документ Word (.docx)|*.docx"
    };

    var result = dialog.ShowDialog();

    string? path = null;
    if (result == true)
    {
      path = dialog.FileName;
    }
    return path;
  }

  public MainWindowViewModel()
  {
    this._reportWriter = new ReportWriter(this.CurrentCollection);
  }
}