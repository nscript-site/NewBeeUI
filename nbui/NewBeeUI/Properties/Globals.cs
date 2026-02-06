namespace NewBeeUI;

public static class Globals
{
    public static int VStackDefaultSpacing = 10;
    public static int HStackDefaultSpacing = 10;
}

public enum PointerAction
{
    Enter,
    Leave,
    Pressed,
}

public static class PointerAction_ClassHelper
{
    public static bool IsEnterOrPressed(this PointerAction action)
    {
        return (action == PointerAction.Enter || action == PointerAction.Pressed);
    }
}