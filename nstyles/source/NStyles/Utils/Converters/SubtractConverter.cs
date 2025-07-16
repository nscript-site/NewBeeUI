using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NStyles.Converters;

public class SubtractConverter : IMultiValueConverter
{
    public object Convert(IList<object> values, Type targetType, object parameter, CultureInfo culture)
    {
        // 确保所有值都是有效的 double 类型
        if (values.Count < 2 || values.Any(v => v == null || !(v is double)))
            return AvaloniaProperty.UnsetValue;

        double result = (double)values[0]; // 初始值为父容器宽度

        // 减去后续所有控件的宽度
        for (int i = 1; i < values.Count; i++)
        {
            result -= (double)values[i];
        }

        result -= 40; // 减去 20 像素的间距

        // 确保结果不小于 0
        return Math.Max(0, result);
    }
}
