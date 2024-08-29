using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using TaskManager.DataOut;
using TaskManager.DB;
using TaskManager.Utilities;
using Task = TaskManager.DB.Models.Task;

namespace TaskManager.ViewModels;

internal sealed class MainWindowViewModel : ListViewModel<Task>
{

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
  public ICommand ReportButtonClick => this._reportButtonClick ??= new RelayCommand(f =>
  {
    var path = GetPath();
    if (path is null) return;
    var reportWriter = new ReportWriter(this.CurrentCollection);
    reportWriter.WriteReport(path);
  }, this.CanExecute);

  protected override async System.Threading.Tasks.Task OnLoad(object sender)
  {
    var tasks = new List<Task>();
    await DBAPI.LoadTable<Task>(tasks);
    this.CurrentCollection = new ObservableCollection<Task>(tasks);
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