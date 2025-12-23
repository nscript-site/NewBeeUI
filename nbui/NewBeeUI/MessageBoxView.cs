using System;

namespace NewBeeUI;

public class MessageBoxView : BaseView
{
    public string Title { get; set; } = String.Empty;
    public string Message { get; set; } = String.Empty;

    public Button[] Buttons { get; set; } = [];

    public int ButtonsHAligh { get; set; } = 0;
    public int TitleHAligh { get; set; } = 0;

    public Control? IconContent { get; set; } = null;

    private TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

    public MessageBoxView():base()
    {
        this.MinWidth = 300;
    }

    protected override object Build()
    {
        var title = TextBlock(Title).Classes("Caption").Classes("big");
        Control header = IconContent != null ?
            HStack([IconContent.Align(null,0), title.Align(-1,0)])
            : title;

        var grid = VGrid("Auto,*,Auto",[
            header.Align(TitleHAligh),
            TextBlock(Message).
                Margin(0,20,0,20).TextWrapping(TextWrapping.Wrap),
            HStack(Buttons).Align(ButtonsHAligh)
            ]);

        var border = Border(grid).MinWidth(this.MinWidth)
            .Padding(20)
            .Background(R("SukiPopupBackground"))
            .BorderBrush(R("SukiBorderBrush"))
            .BorderThickness(1)
            .CornerRadius(6);

        return border;
    }

    public void WaitClosed()
    {
        tcs.Task.ConfigureAwait(false);
        tcs.Task.Wait();
    }

    public Button CreateButton(string text, string? classes = null, StreamGeometry ? icon = null, double? iconSize = 14, Action? onClosed = null)
    {
        var btn =  icon == null ? TextButton(text) 
            : IconButton(text, icon, iconSize:iconSize);

        btn.OnClick(_ =>
            {
                this.RemoveFromOverlay();
                onClosed?.Invoke();
                tcs.SetResult(true);
            });
        if(classes != null) 
            btn = btn.Classes(classes);
        return btn;
    }

    public static MessageBoxView Show(BaseView owner, string message, string title = "消息",  
        string closeButtonText = "关闭",
        string? closeButtonClasses = null,
        StreamGeometry? iconCloseButton = null,
        Control? iconContent = null,
        bool modal = true,
        Action<MessageBoxView>? onCreate = null)
    {
        var msgBox = CreateMessageBoxView(owner, message, title, iconContent);
        msgBox.Buttons = [msgBox.CreateButton(closeButtonText,closeButtonClasses,iconCloseButton)];

        onCreate?.Invoke(msgBox);

        msgBox.ShowInOverlay(owner, modal);
        return msgBox;
    }

    private static MessageBoxView CreateMessageBoxView(BaseView owner, string message, string title, Control? iconContent = null)
    {
        var msgBox = new MessageBoxView
        {
            Title = title,
            Message = message, IconContent = iconContent
        };

        var bounds = owner.GetDesktopWindow()?.Bounds;
        if (bounds.HasValue)
            msgBox.MaxWidth = bounds.Value.Width * 0.9;
        msgBox.Align(0, 0);
        return msgBox;
    }

    public static async Task ShowOkCancel(BaseView owner, string message, Action<bool> onClose, 
        string title = "确认", 
        string okButtonText = "确定", string cancelButtonText = "取消", 
        string? okButtonClasses = null, string? cancelButtonClasses = null,
        StreamGeometry? iconOkButton = null, StreamGeometry? iconCancelButton = null,
        Control? iconContent = null,
        bool modal = true,
        Action<MessageBoxView>? onCreate = null)
    {        
        var msgBox = CreateMessageBoxView(owner, message, title,iconContent);
        var okButton = msgBox.CreateButton(okButtonText, okButtonClasses, iconOkButton, onClosed: () => onClose?.Invoke(true));
        var cancelButton = msgBox.CreateButton(cancelButtonText, cancelButtonClasses, iconCancelButton, onClosed: () => onClose?.Invoke(false));
        msgBox.Buttons = [okButton,cancelButton];
        onCreate?.Invoke(msgBox);
        msgBox.ShowInOverlay(owner,modal);
        await msgBox.tcs.Task.WaitAsync(CancellationToken.None);
    }
}
