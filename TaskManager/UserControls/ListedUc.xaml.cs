﻿using System.Windows;
using System.Windows.Controls;

namespace TaskManager.UserControls;

public abstract class ListedUc<T> : UserControl
{
  public T CurrentClass
  {
    get => (T)this.GetValue(CurrentClassProperty);
    set => this.SetValue(CurrentClassProperty, value);
  }

  public static readonly DependencyProperty CurrentClassProperty =
    DependencyProperty.Register(nameof(CurrentClass), typeof(T), typeof(ListedUc<T>),
      new FrameworkPropertyMetadata(CurrentClassPropertyChanged));

  protected abstract void UpdateData();
  private static void CurrentClassPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (d is not ListedUc<T> listedUc) return;
    listedUc.UpdateData();
  }
}