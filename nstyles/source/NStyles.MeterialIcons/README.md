# navalonia

提供 Avalonia 下的 material icons 库，每个 icon 是一个单独的类，方便 Aot 下裁剪。

示例：

```xml
      <Button Classes="Icon Outlined">
        <PathIcon Data="{x:Static icons:SearchIcon.Instance}" Foreground="{Binding Foreground, RelativeSource={RelativeSource FindAncestor, AncestorType=Button}}"/>
      </Button>
```

所有图标由程序解析 ![MaterialDesign-SVG](https://github.com/Templarian/MaterialDesign-SVG) 下的 svg 文件自动生成。共 7447 个图标，可在线浏览：https://pictogrammers.com/library/mdi/

图标的 License: https://github.com/Templarian/MaterialDesign-SVG/blob/master/LICENSE