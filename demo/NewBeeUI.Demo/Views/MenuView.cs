using Avalonia.Controls.Templates;
using Avalonia.Markup.Declarative;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewBeeUI.Demo.Views;

public class MenuView : BaseView
{
    protected override object Build()
    {
        return VStack([
             new Menu().Align(-1,-1).Items([
                new MenuItem().CornerRadius(6).Header("Menu")
                    .HeaderTemplate(new MemuIconHeader(MenuIcon.Instance))
                    .Items([
                        new MenuItem().Header("Menu2"),
                        new MenuItem().Header("Menu3"),
                        new MenuItem().Header("Menu4"),
                        new Separator(),
                        new MenuItem().Header("Menu5").Items([
                            new MenuItem().Header("Menu6"),
                            new MenuItem().Header("Menu7"),
                            new MenuItem().Header("Menu8")
                        ]),
                    ])
            ]),
             new Menu().Align(-1,-1).Items([
                new MenuItem().CornerRadius(6).Header("Menu")
                    //.HeaderTemplate(new MemuIconHeader(MenuIcon.Instance))
                    .Items([
                        new MenuItem().Header("Menu2"),
                        new MenuItem().Header("Menu3"),
                        new MenuItem().Header("Menu4"),
                        new Separator(),
                        new MenuItem().Header("Menu5").Items([
                            new MenuItem().Header("Menu6"),
                            new MenuItem().Header("Menu7"),
                            new MenuItem().Header("Menu8")
                        ]),
                    ])
            ]),
             TextBlock("Context Menu").Align(-1,-1).ContextMenu(
                new ContextMenu().CornerRadius(6)
                    .Items([
                        new MenuItem().Header("Menu2"),
                        new MenuItem().Header("Menu3"),
                        new MenuItem().Header("Menu4"),
                        new Separator(),
                        new MenuItem().Header("Menu5").Items([
                            new MenuItem().Header("Menu6"),
                            new MenuItem().Header("Menu7"),
                            new MenuItem().Header("Menu8")
                        ])
                    ])),
        ]).Align(0,0).Spacing(20);
    }
}
