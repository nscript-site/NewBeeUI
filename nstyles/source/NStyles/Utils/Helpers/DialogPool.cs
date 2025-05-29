using NStyles.Controls;
using NStyles.Dialogs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NStyles.Helpers;

internal static class DialogPool
{
    private static readonly ConcurrentBag<ISukiDialog> Pool = new();

    internal static ISukiDialog Get()
    {
        var dialog = Pool.TryTake(out var item) ? item : new SukiDialog();
        return dialog;//.ResetToDefault();
    }

    internal static void Return(ISukiDialog toast) => Pool.Add(toast);

    internal static void Return(IEnumerable<ISukiDialog> dialogs)
    {
        foreach (var dialog in dialogs)
            Pool.Add(dialog);
    }
}