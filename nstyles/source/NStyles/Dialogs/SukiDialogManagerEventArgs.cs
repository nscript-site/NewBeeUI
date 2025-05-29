using System;

namespace NStyles.Dialogs;

public class SukiDialogManagerEventArgs : EventArgs
{
    public ISukiDialog Dialog { get; set; }

    public SukiDialogManagerEventArgs(ISukiDialog dialog)
    {
        Dialog = dialog;
    }
}

public delegate void SukiDialogManagerEventHandler(object sender, SukiDialogManagerEventArgs args);