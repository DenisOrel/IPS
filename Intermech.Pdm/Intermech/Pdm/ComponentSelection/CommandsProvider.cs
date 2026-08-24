// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.ComponentSelection.CommandsProvider
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Pdm;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Interfaces;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Pdm.ComponentSelection;

internal class CommandsProvider : ICommandsProvider
{
  private int _enableRelationType = MetaDataHelper.GetRelationTypeID("cad00023-306c-11d8-b4e9-00304f19f545");

  public CommandsInfo GetMergedCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    return new CommandsInfo();
  }

  public CommandsInfo GetGroupCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    if (items == null || items.Count != 1 || !(items.GetItemData(0, typeof (IDBTypedObjectID)) is IDBTypedObjectID itemData1) || !CommandService.EnabledTypes.Contains(itemData1.ObjectType))
      return CommandsInfo.Empty;
    CommandsInfo groupCommands = new CommandsInfo();
    if (items.GetItemData(0, typeof (IDBRelationID)) is IDBRelationID itemData2 && itemData2.RelationType == this._enableRelationType && itemData2.Value != -1L)
    {
      groupCommands.Add(Intermech.Pdm.ComponentSelection.ContextMenu.cmdCreateNew, new CommandInfo(0, new ClickEventHandler(this.CreateNew)));
      groupCommands.Add(Intermech.Pdm.ComponentSelection.ContextMenu.cmdAddExisting, new CommandInfo(0, new ClickEventHandler(this.AddExisting)));
      groupCommands.Add(Intermech.Pdm.ComponentSelection.ContextMenu.cmdAddFromImbase, new CommandInfo(0, new ClickEventHandler(this.AddFromImbase)));
      groupCommands.Add(Intermech.Pdm.ComponentSelection.ContextMenu.cmdReset, new CommandInfo(0, new ClickEventHandler(this.Reset)));
    }
    return groupCommands;
  }

  private void AddFromImbase(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    IDBRelationID itemData = items.GetItemData(0, typeof (IDBRelationID)) as IDBRelationID;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      ((IComponentSelectionCommandService) ServicesManager.GetService(typeof (IComponentSelectionCommandService))).AddFromImbase(sessionKeeper.Session, new long[1]
      {
        itemData.ProjID
      }, new Guid[1]{ itemData.RelGuid });
  }

  private void AddExisting(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    IDBRelationID itemData = items.GetItemData(0, typeof (IDBRelationID)) as IDBRelationID;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      ((IComponentSelectionCommandService) ServicesManager.GetService(typeof (IComponentSelectionCommandService))).AddExisting(sessionKeeper.Session, new long[1]
      {
        itemData.ProjID
      }, new Guid[1]{ itemData.RelGuid });
  }

  private void CreateNew(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    IDBRelationID itemData = items.GetItemData(0, typeof (IDBRelationID)) as IDBRelationID;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      ((IComponentSelectionCommandService) ServicesManager.GetService(typeof (IComponentSelectionCommandService))).CreateNew(sessionKeeper.Session, new long[1]
      {
        itemData.ProjID
      }, new Guid[1]{ itemData.RelGuid });
  }

  private void Reset(ISelectedItems items, IServiceProvider viewServices, object additionalInfo)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      long[] projectIDs = new long[items.Count];
      Guid[] relationGuids = new Guid[items.Count];
      List<long> longList = new List<long>();
      for (int index = 0; index < items.Count; ++index)
      {
        IDBRelationID itemData = items.GetItemData(index, typeof (IDBRelationID)) as IDBRelationID;
        projectIDs[index] = itemData.ProjID;
        relationGuids[index] = itemData.RelGuid;
        if (!longList.Contains(itemData.ProjID))
        {
          sessionKeeper.Session.GetObject(itemData.ProjID).CheckEdit();
          longList.Add(itemData.ProjID);
        }
      }
      ((IComponentSelectionCommandService) ServicesManager.GetService(typeof (IComponentSelectionCommandService))).Reset(sessionKeeper.Session, projectIDs, relationGuids);
    }
  }
}
