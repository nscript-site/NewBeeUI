# navalonia

提供 Avalonia 下的 FontAwesome7 icons 库，每个 icon 是一个单独的类，方便 Aot 下裁剪。

示例：

```xml
      <Button Classes="Icon Outlined">
        <PathIcon Data="{x:Static icons:RegularIcons.AddressBookIcon.Instance}" Foreground="{Binding Foreground, RelativeSource={RelativeSource FindAncestor, AncestorType=Button}}"/>
      </Button>
```

所有图标由程序解析 ![Font-Awesome](https://github.com/FortAwesome/Font-Awesome/tree/7.x/svgs-full) 下的 svg 文件自动生成。共 2806 个图标，在线浏览：https://fontawesome.com/icons

图标的 License: https://github.com/FortAwesome/Font-Awesome/blob/7.x/LICENSE.txt