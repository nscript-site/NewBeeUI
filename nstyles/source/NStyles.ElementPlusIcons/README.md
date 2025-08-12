# navalonia

提供 Avalonia 下的 element plus icons 库，每个 icon 是一个单独的类，方便 Aot 下裁剪。

示例：

```xml
      <Button Classes="Icon Outlined">
        <PathIcon Data="{x:Static icons:AddLocationIcon.Instance}" Foreground="{Binding Foreground, RelativeSource={RelativeSource FindAncestor, AncestorType=Button}}"/>
      </Button>
```

所有图标由程序解析 ![element-plus-icons](https://github.com/element-plus/element-plus-icons/tree/main/packages/svg) 下的 svg 文件自动生成。共 294 个图标，可在线浏览：https://icon-sets.iconify.design/ep/

图标的 License(MIT): https://github.com/element-plus/element-plus-icons/blob/main/LICENSE