using System;
using System.Collections.Generic;
using System.ComponentModel;
using TaskManager.DB.Enums;
using TaskManager.DB.Models;
using Xceed.Document.NET;

namespace TaskManager.Utilities;

internal class Sorter
{
  internal class Filter(string name)
  {
    public string Name { get; set; } = name;
    public bool IsChecked { get; set; } = false;
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
  public List<Filter> StatusFilters { get; private set; }
  public List<Filter> PriorityFilters { get; private set; }
  public void UpdateSort()
  {
    this._view.SortDescriptions.Clear();
    this._view.SortDescriptions.Add(new SortDescription(this._selectedSortParameter.ToString(), this._selectedSortDirection));
  }

  public void ViewRefresh()
  {
    this._view.Refresh();
  }
  private bool Contains(object obj)
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

  public void ResetFilters()
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
  public bool CanRefresh(object parameter)
  {
    return (this._fromCreationTime == string.Empty || this._fromCreationTime.Length == 8) &&
           (this._toCreationTime == string.Empty || this._toCreationTime.Length == 8) &&
           (this._fromDeadLine == string.Empty || this._fromDeadLine.Length == 8) &&
           (this._toDeadLine == string.Empty || this._toDeadLine.Length == 8);
  }

  public Sorter(ICollectionView view)
  {
    this.ResetFilters();
    this._view = view;
    this._view.Filter = this.Contains;
  }
}