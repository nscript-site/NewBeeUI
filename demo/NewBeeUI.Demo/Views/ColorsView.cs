using Avalonia.Data;

namespace NewBeeUI.Demo.Views;

public class ColorsView : BaseView
{
    protected override void Build(out Control content)
    {
        VStack([
                DemoColor(R_PrimaryColor, "PrimaryColor"),
                DemoColor(R_PrimaryDarkColor, "PrimaryDarkColor"),
                DemoColor(R_AccentColor, "AccentColor"),
                DemoColor(R_AccentDarkColor, "AccentDarkColor"),
                DemoColor(R_TextColor, "TextColor"),
                DemoColor(R_LowTextColor, "LowTextColor"),
                DemoColor(R_DisabledTextColor, "DisabledTextColor"),
                DemoColor(R_LightBorderBrush, "LightBorderBrush"),
                DemoColor(R_SukiBackground, "SukiBackground"),
                DemoColor(R_SukiStrongBackground, "SukiStrongBackground"),
                DemoColor(R_SukiCardBackground, "SukiCardBackground"),
                DemoColor(R_SukiLightBackground, "SukiLightBackground"),
                DemoColor(R_SukiPopupBackground, "SukiPopupBackground"),
                DemoColor(R_SukiGlassCardBackground, "SukiGlassCardBackground"),
                DemoColor(R_SukiGlassCardOpaqueBackground, "SukiGlassCardOpaqueBackground"),
                DemoColor(R_SukiControlTouchBackground, "SukiControlTouchBackground"),
                DemoColor(R_SukiDialogBackground, "SukiDialogBackground"),
                DemoColor(R_SukiBorderBrush, "SukiBorderBrush"),
                DemoColor(R_SukiControlBorderBrush, "SukiControlBorderBrush"),
                DemoColor(R_SukiMediumBorderBrush, "SukiMediumBorderBrush"),
                DemoColor(R_SukiLightBorderBrush, "SukiLightBorderBrush"),
                DemoColor(R_SukiMenuBorderBrush, "SukiMenuBorderBrush"),
                DemoColor(R_GlassBorderBrush, "GlassBorderBrush"),
                DemoViewCodeView(),
            ]).Return(out content);
    }

    protected DemoColorView DemoColor(BindingBase binding, string name = "")
    {
        return new DemoColorView() { Name = name, Color = binding };
    }
}
