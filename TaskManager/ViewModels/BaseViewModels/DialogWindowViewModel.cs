namespace TaskManager.ViewModels.BaseViewModels;

internal abstract class DialogWindowViewModel<T> : BaseViewModel
{
  private T? _dialogResult;
  public T? DialogResult
  {
    get => this._dialogResult;
    set
    {
      this._dialogResult = value;
      this.OnPropertyChanged();
    }
  }
}