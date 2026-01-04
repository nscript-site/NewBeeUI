namespace NewBeeUI.Demo.Views.Components;

public class CodeUrlView : BaseView
{
    public string Url { get; set; } = String.Empty;

    public string BaseUrl { get; set; } = "https://github.com/nscript-site/NewBeeUI/tree/main/demo/NewBeeUI.Demo/Views/";

    protected override void Build(out Control content)
    {
        new HyperlinkButton()
            .Text(Url)
            .NavigateUri(new Uri($"{BaseUrl}{Url}"))
            .Align(0, 0)
            .Return(out content);
    }
}
