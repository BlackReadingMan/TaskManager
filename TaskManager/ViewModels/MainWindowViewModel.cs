using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using TaskManager.DataOut;
using TaskManager.DB;
using TaskManager.Utilities;
using Task = TaskManager.DB.Models.Task;

namespace TaskManager.ViewModels;

internal class MainWindowViewModel : INotifyPropertyChanged
{
  private ObservableCollection<Task> _tasks;
  public ObservableCollection<Task> Tasks
  {
    get => this._tasks;
    set
    {
      this._tasks = value;
      this.OnPropertyChanged();
    }
  }

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
  private ICommand? _loadedCommand;
  public ICommand LoadedCommand => this._loadedCommand ??= new RelayCommand(async f =>
  {
    var tasks = new List<Task>();
    await DBAPI.LoadTable<Task>(tasks);
    this.Tasks = new ObservableCollection<Task>(tasks);
  }, CanExecute);
  private ICommand? _reportButtonClick;
  public ICommand ReportButtonClick => this._reportButtonClick ??= new RelayCommand(f =>
  {
    var path = GetPath();
    if (path is null) return;
    var reportWriter = new ReportWriter(this.Tasks);
    reportWriter.WriteReport(path);
  }, CanExecute);
  private ICommand? _addButtonClick;
  public ICommand AddButtonClick => this._addButtonClick ??= new RelayCommand(async f =>
  {

  }, CanExecute);
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
  private static bool CanExecute(object parameter)
  {
    return App.CurrentUser is not null;
  }

  public event PropertyChangedEventHandler? PropertyChanged;
  public void OnPropertyChanged([CallerMemberName] string prop = "")
  {
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
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