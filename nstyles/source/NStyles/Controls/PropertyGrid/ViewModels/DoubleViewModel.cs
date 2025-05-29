using System.ComponentModel;
using System.Reflection;

namespace NStyles.Controls;

public sealed class DoubleViewModel : PropertyViewModelBase<double?>
{
    public DoubleViewModel(INotifyPropertyChanged viewmodel, string displayName, PropertyInfo propertyInfo)
        : base(viewmodel, displayName, propertyInfo)
    {
    }
}