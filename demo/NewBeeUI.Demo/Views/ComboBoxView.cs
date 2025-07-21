using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewBeeUI.Demo.Views;

public class ComboBoxView : BaseView
{
    protected override object Build()
    {
        return new ComboBox().Width(200).ItemsSource(new String[] { "Red", "Blue" }).SelectedIndex(1);
    }
}
