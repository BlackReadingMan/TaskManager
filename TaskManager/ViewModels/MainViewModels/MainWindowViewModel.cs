using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
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
  internal class Filter(string name)
  {
    public string Name { get; set; } = name;
    public bool IsChecked { get; set; } = false;
  }
  private Task? _selectedTask;
  public Task SelectedTask
  {
    set => this._selectedTask = value;
  }

  private readonly ICollectionView _view;
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

  private string _fromCreationTime;
  public string FromCreationTime
  {
    set => this._fromCreationTime = value;
  }

  private string _toCreationTime;
  public string ToCreationTime
  {
    set => this._toCreationTime = value;
  }

  private string _fromDeadLine;
  public string FromDeadLine
  {
    set => this._fromDeadLine = value;
  }

  private string _toDeadLine;
  public string ToDeadLine
  {
    set => this._toDeadLine = value;
  }

  private bool _isFilterExpanded;

  public bool IsFilterExpanded
  {
    get => this._isFilterExpanded;
    set
    {
      this._isFilterExpanded = value;
      this.OnPropertyChanged();
    }
  }

  public List<Filter> StatusFilters { get; private set; }
  public List<Filter> PriorityFilters { get; private set; }

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

  private ICommand? _filterSetButtonClick;
  public ICommand FilterSetButtonClick => this._filterSetButtonClick ??= new RelayCommand(f => this._view.Refresh(), this.CanRefresh);

  private ICommand? _filterResetButtonClick;
  public ICommand FilterResetButtonClick => this._filterResetButtonClick ??= new RelayCommand(async f =>
  {
    this.ResetFilters();
    this._view.Refresh();
  });

  private ICommand? _filterMouseLostClick;
  public ICommand FilterMouseLostClick => this._filterMouseLostClick ??= new RelayCommand(async f =>
  {
    this.IsFilterExpanded = false;
  });
  protected override async System.Threading.Tasks.Task UpdateData(object sender)
  {
    await DBAPI.LoadTable<Task>(this.CurrentCollection);
    this.UpdateSort();
  }

  private void UpdateSort()
  {
    this._view.SortDescriptions.Clear();
    this._view.SortDescriptions.Add(new SortDescription(this._selectedSortParameter.ToString(), this._selectedSortDirection));
  }

  public bool Contains(object obj)
  {
    if (obj is not Task task) return false;
    return (!this.StatusFilters[0].IsChecked || task.Status == 0) &&
           (!this.StatusFilters[1].IsChecked || task.Status == (TaskState)1) &&
           (!this.StatusFilters[2].IsChecked || task.Status == (TaskState)2) &&
           (!this.StatusFilters[3].IsChecked || task.Status == (TaskState)3) &&
           (!this.PriorityFilters[0].IsChecked || task.Priority == 0) &&
           (!this.PriorityFilters[1].IsChecked || task.Priority == (TaskPriority)1) &&
           (!this.PriorityFilters[2].IsChecked || task.Priority == (TaskPriority)2) &&
           (!this.PriorityFilters[3].IsChecked || task.Priority == (TaskPriority)3) &&
           (!this.PriorityFilters[4].IsChecked || task.Priority == (TaskPriority)4) &&
           (this._fromCreationTime == string.Empty || task.CreationTime > DateOnly.Parse(this._fromCreationTime)) &&
           (this._toCreationTime == string.Empty || task.CreationTime < DateOnly.Parse(this._toCreationTime)) &&
           (task.Deadline is null || this._fromDeadLine == string.Empty || task.Deadline > DateOnly.Parse(this._fromDeadLine)) &&
           (task.Deadline is null || this._toDeadLine == string.Empty || task.Deadline < DateOnly.Parse(this._toDeadLine));
  }

  private void ResetFilters()
  {
    this.StatusFilters =
    [
      new Filter(TaskState.NotAcceptedForWork.EnumDescription()),
      new Filter(TaskState.TakenIntoWork.EnumDescription()),
      new Filter(TaskState.UnderReview.EnumDescription()),
      new Filter(TaskState.Completed.EnumDescription()),
    ];
    this.PriorityFilters =
    [
      new Filter(TaskPriority.NotSet.EnumDescription()),
      new Filter(TaskPriority.Low.EnumDescription()),
      new Filter(TaskPriority.Medium.EnumDescription()),
      new Filter(TaskPriority.High.EnumDescription()),
      new Filter(TaskPriority.Critical.EnumDescription())
    ];
    this._fromDeadLine = string.Empty;
    this._toDeadLine = string.Empty;
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

  private bool CanRefresh(object parameter)
  {
    return (this._fromCreationTime == string.Empty || this._fromCreationTime.Length == 8) &&
           (this._toCreationTime == string.Empty || this._toCreationTime.Length == 8) &&
      (this._fromDeadLine == string.Empty || this._fromDeadLine.Length == 8) &&
      (this._toDeadLine == string.Empty || this._toDeadLine.Length == 8);
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
    this.ResetFilters();
    this._view = CollectionViewSource.GetDefaultView(this.CurrentCollection);
    this._view.Filter = this.Contains;
  }
}