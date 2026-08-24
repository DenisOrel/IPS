// Decompiled with JetBrains decompiler
// Type: Intermech.Sales.KeyProcessor
// Assembly: Intermech.Sales, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D9A9043-6548-439B-99F7-AF22F44A5D2B
// Assembly location: D:\IPS\Client\Intermech.Sales.dll

using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Kernel.Search;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Interfaces;
using System;
using System.Data;

#nullable disable
namespace Intermech.Sales;

internal class KeyProcessor : ICommandsProvider
{
  public CommandsInfo GetMergedCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    CommandsInfo mergedCommands = new CommandsInfo();
    mergedCommands.Add(SalesClientConsts.mnuCreateKey4Request, new CommandInfo(16 /*0x10*/, new ClickEventHandler(this.CreateKey4Request)));
    return mergedCommands;
  }

  public CommandsInfo GetGroupCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    return CommandsInfo.Empty;
  }

  public void CreateKey4Request(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    if (items.Count == 0 || !(items.GetItemData(0, typeof (IDBTypedObjectID)) is IDBTypedObjectID itemData))
      return;
    long objectId = itemData.ObjectID;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      foreach (DataRow row in (InternalDataCollectionBase) sessionKeeper.Session.GetRelationCollection(MetaDataHelper.GetRelationTypeID(new Guid("cad00151-306c-11d8-b4e9-00304f19f545"))).ConsistFrom(new DBRecordSetParams((ConditionStructure[]) null, new ColumnDescriptor[1]
      {
        new ColumnDescriptor((object) ObligatoryObjectAttributes.F_OBJECT_ID, AttributeSourceTypes.Object, ColumnContents.Value, ColumnNameMapping.ID, SortOrders.NONE, 0)
      }), objectId).Rows)
        Convert.ToInt64(row[0]);
      sessionKeeper.Session.GetObjectCollection(new Guid("cad01510-306c-11d8-b4e9-00304f19f545"));
    }
  }
}
