using System.Windows;

namespace TaskManager.Windows.DialogWindows;

public abstract class DialogWindow<T> : Window
{
  public T? ReturnData
  {
    get => (T?)this.GetValue(InteractionResultProperty);
    set => this.SetValue(InteractionResultProperty, value);
  }
  public static readonly DependencyProperty InteractionResultProperty =
    DependencyProperty.Register(
      nameof(ReturnData),
      typeof(T?),
      typeof(DialogWindow<T>), new FrameworkPropertyMetadata(OnInteractionResultChanged));

  private static void OnInteractionResultChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((DialogWindow<T>)d).Close();
  }
}