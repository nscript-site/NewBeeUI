using Avalonia.Styling;

namespace NewBeeUI.Demo.Views;

public class MobileSettingView : BaseView
{
    protected override void Build(out Control content)
    {
        VStack([
            TextBlock("当前为基础版, 解锁会员尽享下列权益:"),
            SettingVGrid([HGrid("*,*",[
                TextButton("无页数限制", FileMultipleOutlineIcon.Instance),
                TextButton("解锁高级功能", CrownOutlineIcon.Instance)
                ])]).CornerRadius(10).Background(R("SukiCardBackground")).Padding(10,10),
            TextButton("升级为会员").Height(46).FlatStyle().Align(null,0),
            SettingVGrid([
                SettingRow(WeatherNightIcon.Instance, "深色模式",BuildToggleButton()),
                SettingRow(PaletteOutlineIcon.Instance, "主题色",HStack(BuildColorThemeRadioButtons()).Spacing(4).Margin(0,0,10,0)),
            ]).CornerRadius(10).Background(R("SukiCardBackground")).Margin(0,10),
            SettingVGrid([
                SettingRow(HelpCircleOutlineIcon.Instance, "使用指南", RightArrow(), true),
                SettingRow(CommentMultipleOutlineIcon.Instance, "反馈建议", RightArrow(), true),
                SettingRow(AccountOutlineIcon.Instance, "用户协议", RightArrow(), true),
                SettingRow(ShieldAccountOutlineIcon.Instance, "隐私政策", RightArrow(), true),
                SettingRow(InformationOutlineIcon.Instance, "关于我们", RightArrow(), true),
            ]).CornerRadius(10).Background(R("SukiCardBackground")),
        ]).Margin(0, 20).Spacing(10).Align(null, null).Return(out content);
    }

    RadioButton[] BuildColorThemeRadioButtons()
    {
        const int size = 40;
        var themes = NTheme.GetInstance().ColorThemes;
        if (themes == null || themes.Count == 0) return new RadioButton[] { };
        var radioButtons = new RadioButton[themes.Count];
        for (int i = 0; i < themes.Count; i++)
        {
            var t = themes[i];
            var button = new RadioButton()
                    .Size(size)
                    .GroupName("RadioColorTheme").CornerRadius(size).Classes("GigaChips")
                    .OnClick(e => { e.Handled = true; NTheme.GetInstance().ColorTheme(t); })
                    .Content(
                        new Border().Margin(-30).Background(t.PrimaryBrush).CornerRadius(size)
                        );
            radioButtons[i] = button;
        }
        return radioButtons;
    }

    Control BuildToggleButton()
    {
        return new ToggleSwitch().Margin(0, 3).IsChecked(() => NTheme.GetInstance().ActiveBaseTheme.Key.ToString() != "Light")
            .WhenIsCheckedChanged((ts) =>
            {
                if (ts.IsChecked == true)
                {
                    NTheme.GetInstance().BaseTheme(ThemeVariant.Dark);
                }
                else
                {
                    NTheme.GetInstance().BaseTheme(ThemeVariant.Light);
                }
            });
    }

    protected Button RightArrow()
    {
        return IconButton(ChevronRightIcon.Instance, iconSize: 10);
    }

    protected Control TextButton(string text, StreamGeometry g)
    {
        var stack = VStack([new PathIcon().Data(g).Align(0).Foreground(R("SukiPrimaryColor")).Size(26).IsHitTestVisible(false),
            TextBlock(text).Align(0).LowTextStyle().IsHitTestVisible(false),
        ]).Background(Brushes.Transparent).Spacing(10);
        return stack;
    }
}
