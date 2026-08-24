// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.ProductionCopyMenuProvider
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Intermech.Interfaces;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Interfaces;
using System;

#nullable disable
namespace Intermech.MRP2;

internal class ProductionCopyMenuProvider : ICommandsProvider
{
  private IFactory factory;

  public ProductionCopyMenuProvider(IFactory factory, INamedImageList _images)
  {
    this.factory = factory;
    MenuTemplate contextMenuTemplate = factory.ContextMenuTemplate;
    contextMenuTemplate.BeginUpdate();
    contextMenuTemplate.Nodes.Add(new MenuTemplateNode("MRP2.ApplyCopyInPL", "Применить копию в других ПВ", -1, 41, 22));
    contextMenuTemplate.EndUpdate();
  }

  public CommandsInfo GetMergedCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    CommandsInfo mergedCommands = new CommandsInfo();
    mergedCommands.Add("MRP2.ApplyCopyInPL", new CommandInfo(0, new ClickEventHandler(ApplyChangesInAnotherPL.Handler)));
    mergedCommands.Suppress("EditDocument", 0);
    return mergedCommands;
  }

  public CommandsInfo GetGroupCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    return CommandsInfo.Empty;
  }
}
