# navalonia

提供 Avalonia 下的 sf symbols 库，每个 icon 是一个单独的类，方便 Aot 下裁剪。

示例：

```xml
      <Button Classes="Icon Outlined">
        <PathIcon Data="{x:Static icons:CrossCaseCircleFillRegularIcon.Instance}" Foreground="{Binding Foreground, RelativeSource={RelativeSource FindAncestor, AncestorType=Button}}"/>
      </Button>
```

所有图标由程序解析 ![sfsymbols-svg](https://github.com/camille-semmel/sfsymbols-svg/tree/main/sf-symbols-svg) 下的 svg 文件自动生成。共 4494 个图标，在线浏览：https://developer.apple.com/sf-symbols/
