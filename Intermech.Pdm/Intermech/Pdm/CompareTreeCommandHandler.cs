// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.CompareTreeCommandHandler
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using ImSSP;
using Intermech.Client.Core;
using Intermech.Controls;
using Intermech.DataFormats;
using Intermech.Docking;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Localization;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Interfaces;
using Intermech.Pdm.Compositions.CompareTree;
using System;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Pdm;

internal static class CompareTreeCommandHandler
{
  public static void CompareMethod(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBTypedObjectID itemData = items.GetItemData(0, typeof (IDBTypedObjectID)) as IDBTypedObjectID;
      IDBTypedObjectID dbTypedObjectId;
      if (items.Count == 1)
      {
        if (itemData.ObjectID < 0L)
        {
          IDBObject dbObject = sessionKeeper.Session.GetObject(Math.Abs(itemData.ObjectID));
          dbTypedObjectId = (IDBTypedObjectID) new DBTypedObjectID(dbObject.ObjectType, dbObject.ObjectID, dbObject.ID, dbObject.Caption, dbObject.OwnerID, (long) dbObject.VersionID, dbObject.IsBaseVersion ? 1L : 0L, dbObject.SiteID, dbObject.ModificationID);
        }
        else
        {
          int num = (int) IMMessageBox.Show(MessageDialogs.msgWarning, LocalizationHolder.rm.GetString(sc_16560.ssp_pdm_16561()), MessageBoxButtons.OK, IMMessageBoxImage.Warning);
          return;
        }
      }
      else
        dbTypedObjectId = items.GetItemData(1, typeof (IDBTypedObjectID)) as IDBTypedObjectID;
      DockManager service = (DockManager) ServicesManager.GetService(typeof (DockManager));
      if (service == null)
        return;
      CompareTreeWindow compareTreeWindow = new CompareTreeWindow(sessionKeeper.Session, itemData, dbTypedObjectId);
      compareTreeWindow.Show(service);
      compareTreeWindow.Activate();
    }
  }

  public static void CompareTreeVersion(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    if (!(items.GetItemData(0, typeof (IDBObjectID)) is IDBObjectID itemData))
      return;
    long versionForCompareId = VersionComparison.GetVersionForCompareId(viewServices, itemData);
    if (versionForCompareId == 0L)
      return;
    if (versionForCompareId == Math.Abs(itemData.Value))
      CompareTreeCommandHandler.CompareMethod(Services.GetItems(itemData.Value), viewServices, additionalInfo);
    else
      CompareTreeCommandHandler.CompareMethod(Services.GetItems(itemData.Value, versionForCompareId), viewServices, additionalInfo);
  }
}
