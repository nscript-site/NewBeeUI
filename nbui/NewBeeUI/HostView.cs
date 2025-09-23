using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewBeeUI;

public class HostView : BaseView
{
    public HostView() : base() { }

    private Control? InnerView;

    private Panel _Hosts;

    public Panel Hosts => _Hosts;

    public HostView(Control? view) : base()
    { 
        InnerView = view;
        _Hosts = Panel();
    }

    protected override object Build()
    {
        return Panel(InnerView, _Hosts);
    }
}
