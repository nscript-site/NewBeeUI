using System;
using System.ComponentModel;

namespace NStyles.Controls;

public interface IPropertyViewModel : INotifyPropertyChanged, IDisposable
{
    object? Value { get; set; }
}