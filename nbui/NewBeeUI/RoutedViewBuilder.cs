namespace NewBeeUI;

public interface IRoutedViewBuilder
{
    public RoutedView Build();
}

public class RoutedView
{
    public string Name { get; internal set; }
    public string Route { get; internal set; }
    public Control View { get; internal set; }

    public RoutedView(string name, string path, Control view)
    {
        Name = name;
        Route = path;
        View = view;
    }
}

public class RoutedViewBuilder : IRoutedViewBuilder
{
    public string Route { get; private set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    private Control? _view;

    private Func<Control>? _viewBuilder;

    public PathIcon? Icon { get; set; } = null;

    public RoutedView Build()
    {
        var view = GetOrCreateView();
        return new RoutedView(Name, Route, view);
    }

    public bool IsEmpty()
    {
        return _view == null && _viewBuilder == null;
    }

    public Control GetOrCreateView()
    {
        if (_view != null) return _view;
        else if (_viewBuilder != null) return _viewBuilder();
        else throw new InvalidOperationException("RoutedView is not set and no factory is provided.");
    }

    public RoutedViewBuilder()
    {
    }

    public RoutedViewBuilder(string name, Control view)
    {
        Name = name;
        _view = view;
    }

    public RoutedViewBuilder(string name, string route, Control view):this(name, view)
    {
        Route = route;
    }

    public RoutedViewBuilder(string name, Func<Control> builder)
    {
        Name = name;
        _viewBuilder = builder;
    }

    public RoutedViewBuilder(string name, string route, Func<Control> builder): this(name, builder)
    {
        Route = route;
    }
}
