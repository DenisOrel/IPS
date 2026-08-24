// Decompiled with JetBrains decompiler
// Type: Intermech.ImShape.Client.ImShapeCommandProvider
// Assembly: Intermech.ImShape.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: EAEE73DE-1C1F-4401-8BB6-D181BFA32870
// Assembly location: D:\IPS\Client\Intermech.ImShape.Client.dll

using Intermech.Interfaces;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Interfaces;
using System;

#nullable disable
namespace Intermech.ImShape.Client;

public class ImShapeCommandProvider : ICommandsProvider
{
  public CommandsInfo GetMergedCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    CommandsInfo mergedCommands = new CommandsInfo();
    if (items == null)
      return mergedCommands;
    INodeID itemId = items.GetItemID(0);
    if (itemId == null)
      return mergedCommands;
    int objectTypeId = MetaDataHelper.GetObjectTypeID("cad00250-306c-11d8-b4e9-00304f19f545");
    if (MetaDataHelper.IsObjectTypeChildOf(itemId.TypeID, objectTypeId))
    {
      if (items.Count == 1)
        mergedCommands.Add("IMShapeSearch", new CommandInfo(0, new ClickEventHandler(this.ImShapeSearchConfigurationClick)));
    }
    else
    {
      mergedCommands.Add("IMShapeAdd", new CommandInfo(0, new ClickEventHandler(this.ImShapeAddDocClick)));
      if (items.Count == 1)
        mergedCommands.Add("IMShapeSearch", new CommandInfo(0, new ClickEventHandler(this.ImShapeSearchDocClick)));
    }
    return mergedCommands;
  }

  public CommandsInfo GetGroupCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    return new CommandsInfo();
  }

  protected void ImShapeAddDocClick(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    ImShapeCom.AddDoc(items);
  }

  protected void ImShapeSearchDocClick(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    ImShapeCom.SearchDoc(items);
  }

  protected void ImShapeSearchConfigurationClick(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    ImShapeCom.SearchConfiguration(items);
  }
}
