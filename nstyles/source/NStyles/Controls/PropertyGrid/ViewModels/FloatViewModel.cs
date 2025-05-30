﻿using System.ComponentModel;
using System.Reflection;

namespace NStyles.Controls;

public sealed class FloatViewModel : PropertyViewModelBase<float?>
{
    public FloatViewModel(INotifyPropertyChanged viewmodel, string displayName, PropertyInfo propertyInfo)
        : base(viewmodel, displayName, propertyInfo)
    {
    }
}