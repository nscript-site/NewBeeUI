using System;

namespace NStyles.Toasts;

public class SukiToastManagerEventArgs : EventArgs
{
    public ISukiToast Toast { get; set; }

    public SukiToastManagerEventArgs(ISukiToast toast)
    {
        Toast = toast;
    }
}

public delegate void SukiToastManagerEventHandler(object sender, SukiToastManagerEventArgs args);