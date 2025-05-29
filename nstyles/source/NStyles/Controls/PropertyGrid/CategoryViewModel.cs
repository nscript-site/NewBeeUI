using Avalonia.Collections;

namespace NStyles.Controls;

public class CategoryViewModel : SukiObservableObject
{
    public string DisplayName { get; }

    public IAvaloniaReadOnlyList<IPropertyViewModel> Properties { get; }

    public CategoryViewModel(string categoryDisplayName, AvaloniaList<IPropertyViewModel> properties)
    {
        if (string.IsNullOrEmpty(categoryDisplayName))
        {
            throw new ArgumentException($"'{nameof(categoryDisplayName)}' cannot be null or empty.", nameof(categoryDisplayName));
        }

        DisplayName = categoryDisplayName;
        Properties = properties;
    }
}