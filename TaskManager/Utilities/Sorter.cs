using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TaskManager.DB.Enums;
using TaskManager.DB.Models;
using Xceed.Document.NET;

namespace TaskManager.Utilities;

internal class Sorter : INotifyPropertyChanged
{
  internal class Filter(string name)
  {
    public string Name { get; set; } = name;
    public bool IsChecked { get; set; } = false;
  }
  private readonly ICollectionView _view;

  private ICommand? _filterSetButtonClick;
  public ICommand FilterSetButtonClick => this._filterSetButtonClick ??= new RelayCommand(f => this.ViewRefresh(), this.CanRefresh);

  private ICommand? _filterResetButtonClick;
  public ICommand FilterResetButtonClick => this._filterResetButtonClick ??= new RelayCommand(async f =>
  {
    this.ResetFilters();
    this.ViewRefresh();
  });
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

  private DateTime? _fromCreationTime;
  public DateTime? FromCreationTime
  {
    get => this._fromCreationTime;
    set
    {
      this._fromCreationTime = value;
      this.OnPropertyChanged();
    }
  }

  private DateTime? _toCreationTime;
  public DateTime? ToCreationTime
  {
    get => this._toCreationTime;
    set
    {
      this._toCreationTime = value;
      this.OnPropertyChanged();
    }
  }

  private DateTime? _fromDeadLine;
  public DateTime? FromDeadLine
  {
    get => this._fromDeadLine;
    set
    {
      this._fromDeadLine = value;
      this.OnPropertyChanged();
    }
  }

  private DateTime? _toDeadLine;
  public DateTime? ToDeadLine
  {
    get => this._toDeadLine;
    set
    {
      this._toDeadLine = value;
      this.OnPropertyChanged();
    }
  }

  private List<Filter> _statusFilters;

  public List<Filter> StatusFilters
  {
    get => this._statusFilters;
    private set
    {
      this._statusFilters = value;
      this.OnPropertyChanged();
    }
  }

  private List<Filter> _priorityFilters;

  public List<Filter> PriorityFilters
  {
    get => this._priorityFilters;
    private set
    {
      this._priorityFilters = value;
      this.OnPropertyChanged();
    }
  }
  private void UpdateSort()
  {
    this._view.SortDescriptions.Clear();
    this._view.SortDescriptions.Add(new SortDescription(this._selectedSortParameter.ToString(), this._selectedSortDirection));
  }

  private void ViewRefresh()
  {
    this._view.Refresh();
  }
  private bool Contains(object obj)
  {
    if (obj is not Task task) return false;
    return ((!this.StatusFilters[0].IsChecked && !this.StatusFilters[1].IsChecked && !this.StatusFilters[2].IsChecked && !this.StatusFilters[3].IsChecked) ||
            (this.StatusFilters[0].IsChecked && task.Status == 0) ||
           (this.StatusFilters[1].IsChecked && task.Status == (TaskState)1) ||
           (this.StatusFilters[2].IsChecked && task.Status == (TaskState)2) ||
           (this.StatusFilters[3].IsChecked && task.Status == (TaskState)3)) &&
           ((!this.PriorityFilters[0].IsChecked && !this.PriorityFilters[1].IsChecked && !this.PriorityFilters[2].IsChecked && !this.PriorityFilters[3].IsChecked && !this.PriorityFilters[4].IsChecked) ||
            (this.PriorityFilters[0].IsChecked && task.Priority == 0) ||
            (this.PriorityFilters[1].IsChecked && task.Priority == (TaskPriority)1) ||
            (this.PriorityFilters[2].IsChecked && task.Priority == (TaskPriority)2) ||
            (this.PriorityFilters[3].IsChecked && task.Priority == (TaskPriority)3) ||
            (this.PriorityFilters[4].IsChecked && task.Priority == (TaskPriority)4)) &&
           (this._fromCreationTime is null || task.CreationTime > DateOnly.FromDateTime(this._fromCreationTime.Value)) &&
           (this._toCreationTime is null || task.CreationTime < DateOnly.FromDateTime(this._toCreationTime.Value)) &&
           (task.Deadline is null || this._fromDeadLine is null || task.Deadline > DateOnly.FromDateTime(this._fromDeadLine.Value)) &&
           (task.Deadline is null || this._toDeadLine is null || task.Deadline < DateOnly.FromDateTime(this._toDeadLine.Value));
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
    this.FromCreationTime = null;
    this.ToCreationTime = null;
    this.FromDeadLine = null;
    this.ToDeadLine = null;
  }
  private bool CanRefresh(object parameter)
  {
    return (this._fromCreationTime is null || this._fromCreationTime.HasValue) &&
           (this._toCreationTime is null || this._toCreationTime.HasValue) &&
           (this._fromDeadLine is null || this._fromDeadLine.HasValue) &&
           (this._toDeadLine is null || this._toDeadLine.HasValue);
  }
  public event PropertyChangedEventHandler? PropertyChanged;
  public void OnPropertyChanged([CallerMemberName] string prop = "")
  {
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
  }
  public Sorter(ICollectionView view)
  {
    this.ResetFilters();
    this._view = view;
    this._view.Filter = this.Contains;
    this.UpdateSort();
  }
}