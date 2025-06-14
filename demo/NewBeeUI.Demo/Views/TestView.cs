namespace NewBeeUI.Demo.Views;

public class TestView : BaseView
{
    protected string? msg;

    protected override object Build()
    {
        return VStack(0, 0).Spacing(10).Children([
            TextBlock().Text(()=>msg??"Empty String"),
            TextButton("Test").OnClick(_=>{ Test(); }),
        ]);
    }

    int idx = 0;

    private void Test()
    {
        string? msg1 = "This is a test message.";
        string? msg2 = null;
        Func<string?> func = () => "msg by func";

        int flag = idx % 3;
        idx++;

        switch (flag)
        {
            case 0:
                Test(msg1);
                break;
            case 1:
                Test(msg2);
                break;
            case 2:
                Test(func);
                break;
        }
        var box1= new Box<string>(msg1);
    }

    private void Test(Box<string> box)
    {
        var str = box.Unbox();
        msg = str;
        this.UpdateState();
    }
}
