using Avalonia.Controls.Shapes;
using Avalonia.Threading;

namespace NewBeeUI.Demo.Views;

public class SliderView : BaseView
{
    private System.Timers.Timer? _timer;

    protected override object Build()
    {
        return VStack([
                new Slider().Width(200),
                new ProgressBar().Width(200).Value(50),
                new Slider().Width(200).Value(0).Ref(out Slider slider),
                new Button().Text("Play").Width(100).OnClick(_ => 
                {
                    slider.Value = 0;
                    StartSliderTimer(slider);
                }),
            ]);
    }

    private void StartSliderTimer(Slider slider)
    {
        _timer?.Stop();
        _timer?.Dispose();

        _timer = new System.Timers.Timer(100); // 100ms 间隔
        _timer.Elapsed += (s, e) =>
        {
            Dispatcher.UIThread.Post(() =>
            {
                if (slider.Value < 100)
                {
                    slider.Value += 1;
                }
                else
                {
                    _timer?.Stop();
                }
            });
        };
        _timer.AutoReset = true;
        _timer.Start();
    }
}
