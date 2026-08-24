// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.Menu.ECOCommandProvider
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Intermech.DataFormats;
using Intermech.ECO.Client;
using Intermech.Interfaces;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Interfaces;
using System;

#nullable disable
namespace Intermech.MRP2.Menu;

internal class ECOCommandProvider : ICommandsProvider
{
  public ECOCommandProvider(IFactory factory)
  {
    MenuTemplate contextMenuTemplate = factory.ContextMenuTemplate;
    contextMenuTemplate.BeginUpdate();
    contextMenuTemplate.Nodes.Add(new MenuTemplateNode(MRP2Consts.cmdIndicateApplicability, "Указать применяемость в ПВ", -1, 110, 10));
    contextMenuTemplate.Nodes.Add(new MenuTemplateNode(MRP2Consts.cmdApplyChangesByEco, "Провести изменения в группе ПВ", -1, 110, 11));
    contextMenuTemplate.EndUpdate();
  }

  public CommandsInfo GetMergedCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    if (!(viewServices.GetService(typeof (IViewState)) is IViewState service1))
      return CommandsInfo.Empty;
    CommandsInfo mergedCommands = new CommandsInfo();
    if (items.GetItemData(0, typeof (IDBTypedObjectID)) is IDBTypedObjectID itemData && MetaDataHelper.IsObjectTypeChildOf(itemData.ObjectType, new Guid("cad00348-306c-11d8-b4e9-00304f19f545")))
      mergedCommands.Add(MRP2Consts.cmdApplyChangesByEco, new CommandInfo(0, new ClickEventHandler(ApplyChangesByEcoCommand.Handler)));
    if ((service1.ViewState & ViewStateFlags.ReadOnly) != ViewStateFlags.None || !(viewServices.GetService(typeof (ECOAncestorForm)) is ECOAncestorForm service2) || service2.ReadOnly)
      return mergedCommands;
    mergedCommands.Add(MRP2Consts.cmdIndicateApplicability, new CommandInfo(0, new ClickEventHandler(IndicateApplicabilityCommand.Handler)));
    return mergedCommands;
  }

  public CommandsInfo GetGroupCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    return CommandsInfo.Empty;
  }
}
