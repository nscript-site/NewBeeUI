using System.ComponentModel;
using System.Reflection;

namespace NStyles.Controls;

public sealed class StringViewModel : PropertyViewModelBase<string?>
{
    public StringViewModel(INotifyPropertyChanged viewmodel, string displayName, PropertyInfo propertyInfo)
        : base(viewmodel, displayName, propertyInfo)
    {
    }
}