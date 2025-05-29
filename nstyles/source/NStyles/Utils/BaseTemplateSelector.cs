using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Metadata;

namespace NStyles;

public class BaseTemplateSelector : IDataTemplate
{
    // This Dictionary should store our shapes. We mark this as [Content], so we can directly add elements to it later.
    [Content]
    public Dictionary<string, IDataTemplate> AvailableTemplates { get; } = new Dictionary<string, IDataTemplate>();

    // Build the DataTemplate here
    public Control Build(object? data)
    {
        var key = GetKey(data);

        if (key == null || AvailableTemplates.ContainsKey(key) == false)
            throw new NotSupportedException();

        return AvailableTemplates[key].Build(data)!; // finally we look up the provided key and let the System build the DataTemplate for us
    }

    // Check if we can accept the provided data
    public bool Match(object? data)
    {
        var key = GetKey(data);
        return !string.IsNullOrEmpty(key)           // and the key must not be null or empty
               && AvailableTemplates.ContainsKey(key); // and the key must be found in our Dictionary
    }

    protected virtual string? GetKey(object? data)
    {
        return null;
    }
}