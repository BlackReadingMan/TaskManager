using System;
using System.Collections.Generic;
using TaskManager.DB.Enums;
using TaskManager.DB.Models;
using TaskManager.ViewModels.BaseViewModels;

namespace TaskManager.ViewModels.DialogViewModels;

internal class AddTaskWindowViewModel : DialogWindowViewModel<Task>
{
  private string? _name;
  public string Name
  {
    set => this._name = value;
  }
  private string? _description;
  public string Description
  {
    set => this._description = value;
  }
  private string? _deadLine;
  public string DeadLine
  {
    set => this._deadLine = value;
  }
  private int? _selectedResponsible;
  public int SelectedResponsible
  {
    set => this._selectedResponsible = value;
  }
  private int _selectedPriority = 0;
  public int SelectedPriority
  {
    get => this._selectedPriority;
    set => this._selectedPriority = value;
  }
  public List<string> Responsible { get; private set; } = [];
  public List<TaskPriority> Priority { get; private set; } =
  [
    TaskPriority.Low,
    TaskPriority.High,
    TaskPriority.Critical
  ];

  protected override void AddSubject()
  {
    var task = new Task()
    {
      Name = this._name,
      Description = this._description,
      Deadline = _deadLine is null ? null : DateOnly.Parse(this._deadLine),
      Priority = (TaskPriority)this._selectedPriority,
      Responsible = this._selectedResponsible,
      Status = 0,
      CreationTime = DateOnly.FromDateTime(DateTime.Now)
    };
  }

  protected override bool CanExecute(object parameter)
  {
    return this._name is not null &&
           this._description is not null;
  }
}