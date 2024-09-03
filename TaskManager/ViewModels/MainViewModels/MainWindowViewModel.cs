using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Microsoft.Win32;
using TaskManager.DataOut;
using TaskManager.DB;
using TaskManager.DB.Enums;
using TaskManager.Utilities;
using TaskManager.ViewModels.BaseViewModels;
using TaskManager.Windows.DialogWindows;
using Xceed.Document.NET;
using Task = TaskManager.DB.Models.Task;

namespace TaskManager.ViewModels.MainViewModels;

internal sealed class MainWindowViewModel : ListWindowViewModel<Task>
{
  private Task? _selectedTask;
  public Task SelectedTask
  {
    set => this._selectedTask = value;
  }

  public List<string> SortParameters { get; private set; } =
  [
    TaskSortParameter.Id.EnumDescription(),
    TaskSortParameter.Deadline.EnumDescription(),
    TaskSortParameter.Priority.EnumDescription(),
    TaskSortParameter.Status.EnumDescription(),
    TaskSortParameter.CreationTime.EnumDescription()
  ];

  public List<string> SortDirection { get; private set; } =
  [
    "По возрастанию",
    "По убыванию"
  ];

  private TaskSortParameter _selectedSortParameter = TaskSortParameter.Id;
  public int SelectedSortParameter
  {
    get => (int)this._selectedSortParameter;
    set
    {
      this._selectedSortParameter = (TaskSortParameter)value;
      this.UpdateSort();
    }
  }

  private ListSortDirection _selectedSortDirection = ListSortDirection.Ascending;
  public int SelectedSortDirection
  {
    get => (int)this._selectedSortDirection;
    set
    {
      this._selectedSortDirection = (ListSortDirection)value;
      this.UpdateSort();
    }
  }

  private ICommand? _reportButtonClick;
  public ICommand ReportButtonClick => this._reportButtonClick ??= new RelayCommand(async f =>
  {
    var path = GetPath();
    if (path is null) return;
    await ReportWriter.WriteReport(this.CurrentCollection, path);
  });

  private ICommand? _removeButtonClick;
  public ICommand RemoveButtonClick => this._removeButtonClick ??= new RelayCommand(async f =>
  {
    if (this._selectedTask is null) return;
    await DBAPI.RemoveItem(this._selectedTask);
    this.CurrentCollection.Remove(this._selectedTask);
  }, this.CanAddExecute);
  protected override async System.Threading.Tasks.Task UpdateData(object sender)
  {
    await DBAPI.LoadTable<Task>(this.CurrentCollection);
    this.UpdateSort();
  }

  private void UpdateSort()
  {
    var list = this.CurrentCollection.ToList();
    list.Sort(Task.PropertyComparers[this._selectedSortParameter]);
    if (this._selectedSortDirection == ListSortDirection.Descending)
      list.Reverse();
    this.CurrentCollection = new ObservableCollection<Task>(list);
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
}