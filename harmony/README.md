鸿蒙下的测试

## 测试步骤

1，参考下面步骤，搭建鸿蒙开发环境，并在本目录下 git clone 相关代码

https://openharmony-net.github.io/docs/zh-cn/articles/avalonia/introduction.html?tabs=physical

2，在 `OpenHarmony.Avalonia` 解决方案中，把 `NewBeeUI.Demo` 项目添加进来

3，在 `Entry` 项目中，添加对 `NewBeeUI.Demo` 的引用，并修改 `XComponentEntry.cs` 的代码为:

```csharp
public static void OnSurfaceCreated(OH_NativeXComponent* component, void* window)
{
    try
    {
        ...
        xComponent = new AvaloniaXComponent<NewBeeUI.Demo.App>((nint)component, (nint)window);
        ...
    }
    ...
}
```

4, 在本目录下，运行 build.ps1

5, 用 DevEco Studio 打开 OpenHarmony.Avalonia 目录下的 OHOS_Project 项目，编译后运行即可

## 存在问题

1, TextBox 的语音输入存在问题

