using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TaskManager.DB;
using TaskManager.DB.Enums;
using TaskManager.Utilities;
using TaskManager.ViewModels.BaseViewModels;
using Task = TaskManager.DB.Models.Task;

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
  private string? _selectedResponsible;
  public string SelectedResponsible
  {
    set => this._selectedResponsible = value;
  }
  private int _selectedPriority = 0;
  public int SelectedPriority
  {
    set => this._selectedPriority = value;
  }

  private Dictionary<string, int> responsibleDictionary;

  private ObservableCollection<string> _responsible;

  public ObservableCollection<string> Responsible
  {
    get => this._responsible;
    private set
    {
      this._responsible = value;
      this.OnPropertyChanged();
    }
  }

  public List<TaskPriority> Priority { get; private set; } =
  [
    TaskPriority.NotSet,
    TaskPriority.Low,
    TaskPriority.Medium,
    TaskPriority.High,
    TaskPriority.Critical
  ];
  private ICommand? _loadCommand;
  public ICommand LoadCommand => this._loadCommand ??= new RelayCommand(async f => await this.GetUsers());

  private async System.Threading.Tasks.Task GetUsers()
  {
    this.responsibleDictionary = await DBAPI.GetUsersLogins();
    this.Responsible = new ObservableCollection<string>(this.responsibleDictionary.Keys);
    this.Responsible.Insert(0, "Не назначен");
  }

  protected override void AddSubject()
  {
    var task = new Task
    {
      Name = this._name,
      Description = this._description,
      Deadline = this._deadLine is null ? null : DateOnly.Parse(this._deadLine),
      Priority = (TaskPriority)this._selectedPriority,
      Responsible = this._selectedResponsible is null || !this.responsibleDictionary.TryGetValue(this._selectedResponsible, out var value) ? null : value,
      Status = 0,
      CreationTime = DateOnly.FromDateTime(DateTime.Now)
    };
    this.DialogResult = task;
  }

  protected override bool CanAddExecute(object parameter)
  {
    return this._name is not null &&
           this._description is not null && App.CurrentUser is not null;
  }
}