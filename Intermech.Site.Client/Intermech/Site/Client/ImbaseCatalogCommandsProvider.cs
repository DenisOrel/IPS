// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.ImbaseCatalogCommandsProvider
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.WebPortal;
using Intermech.Kernel.Search;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Site.Client;

internal class ImbaseCatalogCommandsProvider : ICommandsProvider
{
  public CommandsInfo GetMergedCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    return CommandsInfo.Empty;
  }

  public CommandsInfo GetGroupCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    CommandsInfo groupCommands = new CommandsInfo();
    ISitesCacheService customService = (ISitesCacheService) (ApplicationServices.Container.GetService(typeof (IMServerService)) as IMServerService).GetCustomService(typeof (ISitesCacheService));
    if (items.Count == 1)
    {
      if ((items.GetItemData(0, typeof (IDBTypedObjectID)) as IDBTypedObjectID).ObjectType == Intermech.Imbase.Consts.ImbaseCatalogTypeID)
      {
        int code = (int) customService.Info.Code;
        groupCommands.Add(SiteClientConsts.CommandSetEnterPoint, new CommandInfo(0, new ClickEventHandler(this.SetEnterPoint)));
      }
      groupCommands.Add(SiteClientConsts.CommandPublishTableLinks, new CommandInfo(0, new ClickEventHandler(this.PublishTableLinks)));
    }
    return groupCommands;
  }

  private void PublishTableLinks(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    IDBTypedObjectID itemData = items.GetItemData(0, typeof (IDBTypedObjectID)) as IDBTypedObjectID;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject dbObject = sessionKeeper.Session.GetObject(itemData.ObjectID);
      int attributeTypeId = MetaDataHelper.GetAttributeTypeID("cad0014d-306c-11d8-b4e9-00304f19f545");
      IDBAttribute attributeById = dbObject.GetAttributeByID(attributeTypeId);
      if (attributeById != null && attributeById.AsString != string.Empty)
      {
        string asString = attributeById.AsString;
        DBRecordSetParams paramSet = new DBRecordSetParams(new ConditionStructure[1]
        {
          new ConditionStructure(attributeTypeId, RelationalOperators.StartString, (object) asString, LogicalOperators.AND, 0, false)
        }, new ColumnDescriptor[1]
        {
          new ColumnDescriptor((object) ObligatoryObjectAttributes.F_OBJECT_ID, SortOrders.NONE, 0)
        });
        DataTable dataTable = sessionKeeper.Session.GetObjectCollection(Intermech.Imbase.Consts.ImbaseTableRefTypeID).Select(paramSet);
        if (dataTable.Rows.Count == 0)
        {
          int num1 = (int) MessageBox.Show($"Ярлыки не найдены в составе {dbObject.NameInMessages}", SiteClientConsts.CommandPublishTableLinksCaption, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          List<Tuple<long, int>> items1 = new List<Tuple<long, int>>(dataTable.Rows.Count);
          foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
            items1.Add(new Tuple<long, int>(Convert.ToInt64(row[0]), Intermech.Imbase.Consts.ImbaseTableRefTypeID));
          int num2 = (int) UnitedPublishForm.ShowForm(items1);
        }
      }
      else
      {
        int num = (int) MessageBox.Show($"Невозможно определить \"Ключ папки классификатора\" для {dbObject.NameInMessages}", SiteClientConsts.CommandPublishTableLinksCaption, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }
  }

  private void SetEnterPoint(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    using (SelectEnterPointForm selectEnterPointForm = new SelectEnterPointForm())
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        IDBObjectID itemData = items.GetItemData(0, typeof (IDBObjectID)) as IDBObjectID;
        IDBAttribute attributeByGuid = sessionKeeper.Session.GetObject(itemData.Value).GetAttributeByGuid(PortalConsts.attributeEnterPoint);
        char? selected = new char?();
        if (attributeByGuid != null && !string.IsNullOrEmpty(attributeByGuid.AsString))
          selected = new char?(attributeByGuid.AsString[0]);
        selectEnterPointForm.Init(sessionKeeper.Session, selected);
        if (selectEnterPointForm.ShowDialog() != DialogResult.OK)
          return;
        IDBObject dbObject = sessionKeeper.Session.GetObject(itemData.Value);
        IDBAttribute dbAttribute = dbObject.GetAttributeByGuid(PortalConsts.attributeEnterPoint);
        if (!selectEnterPointForm.SelectedSite.HasValue)
        {
          dbAttribute?.Delete(0L);
        }
        else
        {
          if (dbAttribute == null)
            dbAttribute = dbObject.Attributes.AddAttribute(MetaDataHelper.GetAttributeTypeID(PortalConsts.attributeEnterPoint), false);
          dbAttribute.Value = (object) selectEnterPointForm.SelectedSite;
        }
      }
    }
  }
}
