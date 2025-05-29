using System;
using System.ComponentModel;
using System.Reflection;

namespace NStyles.Controls;

public sealed class DateTimeViewModel : PropertyViewModelBase<DateTime?>
{
    public DateTimeViewModel(INotifyPropertyChanged viewmodel, string displayName, PropertyInfo propertyInfo)
        : base(viewmodel, displayName, propertyInfo)
    {
    }
}