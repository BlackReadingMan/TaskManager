using System.Windows;
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
  private Visibility _addButtonVisible;
  public Visibility AddButtonVisible
  {
    get => this._addButtonVisible;
    set
    {
      this._addButtonVisible = value;
      this.OnPropertyChanged();
    }
  }
  private ICommand? _reportButtonClick;
  public ICommand ReportButtonClick => this._reportButtonClick ??= new RelayCommand(async f =>
  {
    var path = GetPath();
    if (path is null) return;
    await this._reportWriter.WriteReport(path);
  }, this.CanExecute);

  protected override async System.Threading.Tasks.Task UpdateData(object sender)
  {
    await DBAPI.LoadTable<Task>(this.CurrentCollection);
  }

  protected override void AddSubject()
  {
    var window = new AddTaskWindow();
    window.ShowDialog();
    var task = window.ReturnData;
    if (task is null) return;
    this.CurrentCollection.Add(task);
    DBAPI.AddItem(task);
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
    if (App.CurrentUser is null)
    {
      this.AddButtonVisible = Visibility.Hidden;
    }
    else
    {
      this.AddButtonVisible = App.CurrentUser.Root ? Visibility.Visible : Visibility.Hidden;
    }
  }
}