namespace NewBeeUI;

public struct ViewRouteUpdateEvent
{
    public RoutedView? Old { get; set; }
    public RoutedView? New { get; set; }
}

public class ViewRouter : BaseView
{
    internal protected Dictionary<string, IRoutedViewBuilder> ViewBuilders { get; } = new Dictionary<string, IRoutedViewBuilder>();

    public Action<ViewRouteUpdateEvent>? OnRouteUpdate { get; set; }

    public Action<RoutedView>? OnViewLeave { get; set; }

    public RoutedView? CurrentView { get; private set; }

    public IRoutedViewBuilder? CurrentViewBuilder { get; private set; }

    public string? CurrentViewName
    {
        get
        {
            if (CurrentView != null)
                return CurrentView.Name;
            return null;
        }
    }

    public Control? EmptyView { get; set; }

    public Func<Exception, Control>? OnError { get; set; }

    protected Stack<IRoutedViewBuilder> RouteHistory { get; } = new Stack<IRoutedViewBuilder>();

    public void ClearRouteHistory()
    {
        RouteHistory.Clear();
    }

    public bool GoBack()
    {
        if (RouteHistory.Count > 0)
        {
            var lastLocator = RouteHistory.Pop();
            Load(lastLocator);
            return true;
        }
        return false;
    }

    public ViewRouter Goto(string route, bool remember = false)
    {
        try
        {
            if (string.IsNullOrEmpty(route))
            {
                throw new ArgumentException("Route cannot be null or empty.", nameof(route));
            }
            if (!ViewBuilders.TryGetValue(route, out var locator))
            {
                throw new KeyNotFoundException($"Route '{route}' is not registered.");
            }

            return Goto(locator, remember);
        }
        catch(Exception ex)
        {
            LoadError(ex);
        }

        return this;
    }

    public ViewRouter Goto(IRoutedViewBuilder locator, bool remember = false)
    {
        if (CurrentViewBuilder != null && remember == true)
            RouteHistory.Push(CurrentViewBuilder);

        Load(locator);
        return this;
    }

    protected void LoadError(Exception ex)
    {
        RoutedView? errView = null;

        if (ViewBuilders.TryGetValue("Error", out var locator))
        {
            errView = locator.Build();
            CurrentViewBuilder = locator;
        }
        else if (OnError != null)
        {
            var ctrl = OnError(ex);
            if (ctrl != null)
            {
                errView = new RoutedView("Error", "Error", ctrl);
                CurrentViewBuilder = null;
            }
        }

        if (CurrentView != null) OnViewLeave?.Invoke(CurrentView);

        CurrentView = errView;

        if (this.IsLoaded == true)
            this.Reload();
    }

    protected void Load(IRoutedViewBuilder builder)
    {
        var oldView = CurrentView;

        if(CurrentView != null) OnViewLeave?.Invoke(CurrentView);
        
        CurrentView = builder.Build();
        CurrentViewBuilder = builder;
        OnRouteUpdate?.Invoke(new ViewRouteUpdateEvent { Old = oldView, New = CurrentView });
        if (this.IsLoaded == true)
            this.Reload();
    }

    protected override object Build()
    {
        return GetCurrentView();
    }

    protected Control GetCurrentView()
    {
        var view = CurrentView?.View ?? EmptyView;
        if (view == null)
        {
            view = new TextBlock().Text("").Align(0, 0);
        }
        return view;
    }
}

public static class ViewRouterExtentions
{
    public static ViewRouter AddRoute(this ViewRouter router, string route, string name, Control routedView)
    {
        if (router.ViewBuilders.ContainsKey(route))
        {
            throw new InvalidOperationException($"Route '{route}' is already registered.");
        }
        router.ViewBuilders[route] = new RoutedViewBuilder(route, routedView) { Name = name };
        return router;
    }

    public static ViewRouter AddRoute(this ViewRouter router, params (string route, string name, Control routedView)[] values)
    {
        foreach (var item in values)
        {
            if (router.ViewBuilders.ContainsKey(item.route))
            {
                throw new InvalidOperationException($"Route '{item.route}' is already registered.");
            }
            router.ViewBuilders[item.route] = new RoutedViewBuilder(item.route, item.routedView) { Name = item.name };
        }
        return router;
    }


    public static ViewRouter AddRoute(this ViewRouter router, string route, string name, Func<Control> routedViewFactory)
    {
        if (router.ViewBuilders.ContainsKey(route))
        {
            throw new InvalidOperationException($"Route '{route}' is already registered.");
        }
        router.ViewBuilders[route] = new RoutedViewBuilder(route, routedViewFactory) { Name = name };
        return router;
    }

    public static ViewRouter AddRoute(this ViewRouter router, params (string route, string name, Func<Control> routedViewFactory)[] values)
    {
        foreach (var item in values)
        {
            if (router.ViewBuilders.ContainsKey(item.route))
            {
                throw new InvalidOperationException($"Route '{item.route}' is already registered.");
            }
            router.ViewBuilders[item.route] = new RoutedViewBuilder(item.route, item.routedViewFactory) { Name = item.name };
        }
        return router;
    }

    public static ViewRouter OnRouteUpdate(this ViewRouter router, Action<ViewRouteUpdateEvent> action)
    {
        router.OnRouteUpdate = action;
        return router;
    }

    public static ViewRouter EmptyView(this ViewRouter router, BaseView view)
    {
        router.EmptyView = view;
        return router;
    }
}
