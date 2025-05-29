using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewBeeUI;

public class IconView : BaseView
{
    //public const string Classed_Icon_Button = "IconButton";
    public const string Classed_Icon_Button_Selected = "IconButton_Selected";
    public const string Classed_IconView_Border = "IconView_Border";


    Button? _button = null;
    Action<IconView>? _onClick_Action = null;
    Border? _border = null;

    protected override object Build()
    {
        return CreateIcon(Path, Tooltip).Ref(out _button)!;
    }

    protected Button CreateIcon(PathIcon? path, string? tooltip)
    {
        if (path == null) return new Button();

        var button = new Button().Classes("Icon").Classes(Classed_Icon_Button)
            .Content(
                new Panel()
                    .Children(
                    new Border().CornerRadius(5).VerticalAlignment(VerticalAlignment.Stretch).HorizontalAlignment(HorizontalAlignment.Stretch)
                        .BorderThickness(new Thickness(0))
                        .Ref(out _border)!
                        .Classes(Classed_IconView_Border).Margin(-8),
                        path.Ref(out PathIcon icon)
                    ))
            .Observable(Button.ForegroundProperty, fg => icon.Foreground = fg);

        _button = button;

        UpdateDisplay();

        if (_onClick_Action != null)
        {
            button.OnClick(e => _onClick_Action(this));
        }

        return button;
    }

    public string? Tooltip { get; set; } = null;
    public string? SelectedTooltip { get; set; } = null;

    private bool _selected;
    public bool Selected
    {
        get => _selected;
        set
        {
            if (_selected == value) return;
            _selected = value;

            UpdateDisplay();
        }
    }

    public PathIcon? Path { get; set; }

    private IconView UpdateDisplay()
    {
        var toolTip = this.Tooltip;
        if (this.Selected == true && string.IsNullOrEmpty(SelectedTooltip) == false)
        {
            toolTip = SelectedTooltip;
        }

        if (_button != null && string.IsNullOrEmpty(toolTip) == false)
        {
            ToolTip.SetTip(_button, toolTip);
        }

        if (_border == null) return this;

        if (this.Selected == true)
        {
            this._border.Opacity = 0.99;
        }
        else
        {
            this._border.Opacity = 0.0;
        }

        return this;
    }

    public IconView OnClick(Action<IconView> action)
    {
        _onClick_Action = action;
        return this;
    }
}

