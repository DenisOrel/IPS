// Decompiled with JetBrains decompiler
// Type: Intermech.GTC.Client.GtcCommandsProvider
// Assembly: Intermech.GTC.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 539B70F6-18D3-4230-8795-0EE95CBE5B1C
// Assembly location: D:\IPS\Client\Intermech.GTC.Client.dll

using ImSSP;
using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Kernel.Search;
using Intermech.Navigator;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Controls;
using Intermech.Navigator.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace Intermech.GTC.Client;

internal class GtcCommandsProvider : ICommandsProvider
{
  private IDBTypedObjectID _rootSelectedItem;
  private long _newRelationId = -1;

  public CommandsInfo GetMergedCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    return CommandsInfo.Empty;
  }

  public CommandsInfo GetGroupCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    ViewStateFlags viewStateFlags = viewServices.GetService(typeof (IViewState)) is IViewState service ? service.ViewState : ViewStateFlags.None;
    if (items.Count != 1)
      return CommandsInfo.Empty;
    CommandsInfo groupCommands = new CommandsInfo();
    IDBTypedObjectID itemData = items.GetItemData(0, typeof (IDBTypedObjectID)) as IDBTypedObjectID;
    IDBTypedObjectID parentData = items.GetParentData(0, typeof (IDBTypedObjectID)) as IDBTypedObjectID;
    List<string> classificatorKeys = this.GetGtcCatalogClassificatorKeys();
    if (itemData != null)
    {
      if (items is NavigatorTreeViewSelectedItem || items is NodeItems || (viewStateFlags & ViewStateFlags.NodeInTree) != ViewStateFlags.None)
      {
        this._rootSelectedItem = itemData;
        if ((itemData.ObjectType == Const.ImbaseCatalogObjectTypeId || itemData.ObjectType == Const.ImbaseFolderObjectTypeId) && this.IsCatalogOrFolderGtc(classificatorKeys, itemData.ObjectID))
        {
          groupCommands.Add("CreateGtcCatalogRecord", new CommandInfo(0));
          groupCommands.Add("CreateAdaptiveItem", new CommandInfo(0, new ClickEventHandler(this.CreateNewAdaptiveItemCommand)));
          groupCommands.Add("CreateInstrumentalItem", new CommandInfo(0, new ClickEventHandler(this.CreateNewInstrumentalItemCommand)));
          groupCommands.Add("CreateCuttingItem", new CommandInfo(0, new ClickEventHandler(this.CreateNewCuttingItemCommand)));
          groupCommands.Suppress("CreateProto", 1);
          groupCommands.Suppress("Navigator.CreateObjectType", 6);
          groupCommands.Suppress("CreateCatalogRecordsNode", 1);
          groupCommands.Suppress("CreateTablesRefNode", 1);
          groupCommands.Suppress("CreateCompositionByPrototype", 1);
        }
      }
      else
      {
        this._rootSelectedItem = parentData;
        if (parentData != null && (parentData.ObjectType == Const.ImbaseCatalogObjectTypeId || parentData.ObjectType == Const.ImbaseFolderObjectTypeId) && this.IsCatalogOrFolderGtc(classificatorKeys, itemData.ObjectID))
        {
          groupCommands.Add("CreateGtcCatalogRecord", new CommandInfo(0));
          groupCommands.Add("CreateAdaptiveItem", new CommandInfo(0, new ClickEventHandler(this.CreateNewAdaptiveItemCommand)));
          groupCommands.Add("CreateInstrumentalItem", new CommandInfo(0, new ClickEventHandler(this.CreateNewInstrumentalItemCommand)));
          groupCommands.Add("CreateCuttingItem", new CommandInfo(0, new ClickEventHandler(this.CreateNewCuttingItemCommand)));
          groupCommands.Suppress("CreateProto", 1);
          groupCommands.Suppress("Navigator.CreateObjectType", 6);
          groupCommands.Suppress("CreateCatalogRecordsNode", 1);
          groupCommands.Suppress("CreateTablesRefNode", 1);
          groupCommands.Suppress("CreateCompositionByPrototype", 1);
        }
      }
    }
    return groupCommands;
  }

  private void CreateNewCatalogRecordGtc(int objectType)
  {
    if (!(ServicesManager.GetService(typeof (IObjectCreatorService)) is IObjectCreatorService service))
      return;
    this._newRelationId = -1L;
    service.AfterDraftCreatedEvent += new AfterDraftCreatedEventHandler(this.OncDlg_ObjectCreatorDraftCreatedEvent);
    long objectByTypeDialog;
    try
    {
      objectByTypeDialog = service.CreateObjectByTypeDialog(objectType);
    }
    finally
    {
      service.AfterDraftCreatedEvent -= new AfterDraftCreatedEventHandler(this.OncDlg_ObjectCreatorDraftCreatedEvent);
    }
    if (objectByTypeDialog != -1L && objectByTypeDialog != 0L && this._newRelationId != -1L)
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        IDBRelation relation = sessionKeeper.Session.GetRelation(this._newRelationId);
        this.SendNotification(this._rootSelectedItem.ObjectID, objectByTypeDialog, relation.RelationID, relation.RelationType);
      }
    }
    this._rootSelectedItem = (IDBTypedObjectID) null;
    this._newRelationId = -1L;
  }

  private void OncDlg_ObjectCreatorDraftCreatedEvent(object sender, AfterDraftCreatedEventArgs e)
  {
    if (this._rootSelectedItem == null || e.ObjectID == 0L)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IMSObjectType objectType = MetaDataHelper.GetObjectType(this._rootSelectedItem.ObjectType);
      IDBObject objectActualCopy = sessionKeeper.Session.GetObjectActualCopy(e.ObjectID, false);
      if (!MetaDataHelper.HasApplicability(this._rootSelectedItem.ObjectType, objectActualCopy.ObjectType, objectType.DefaultRelation))
      {
        int num = (int) MessageBox.Show(string.Format(sc_7268.ssp_imbase_7269(), (object) objectActualCopy.Caption, (object) this._rootSelectedItem.Caption), "", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
        this._newRelationId = sessionKeeper.Session.GetRelationCollection(objectType.DefaultRelation).Create(this._rootSelectedItem.ObjectID, objectActualCopy.ObjectID).RelationID;
    }
  }

  private void SendNotification(
    long parentsId,
    long objectsId,
    long relationsId,
    int relationsTypesId)
  {
    if (!(ServicesManager.GetService(typeof (INotificationService)) is INotificationService service) || objectsId == 0L || objectsId == -1L)
      return;
    service.FireEvent((object) this, (NotificationEventArgs) new DBObjectsEventArgs("ObjectsCreated", objectsId));
    service.FireEvent((object) this, (NotificationEventArgs) new DBRelationsEventArgs("RelationsCreated", relationsId, parentsId, relationsTypesId));
  }

  private void CreateNewAdaptiveItemCommand(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    if (items == null || items.Count == 0)
      return;
    this.CreateNewCatalogRecordGtc(Const.AdaptiveItemObjectTypeId);
  }

  private void CreateNewInstrumentalItemCommand(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    if (items == null || items.Count == 0)
      return;
    this.CreateNewCatalogRecordGtc(Const.ToolItemObjectTypeId);
  }

  private void CreateNewCuttingItemCommand(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    if (items == null || items.Count == 0)
      return;
    this.CreateNewCatalogRecordGtc(Const.CuttingItemObjectTypeId);
  }

  private List<string> GetGtcCatalogClassificatorKeys()
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      return sessionKeeper.Session.GetObjectCollection(Const.ImbaseCatalogObjectTypeId).Select(new DBRecordSetParams(new ConditionStructure[1]
      {
        new ConditionStructure(Const.CatalogTypeAttributeTypeId, RelationalOperators.Equal, (object) "Каталоги GTC", LogicalOperators.NONE, 0, false)
      }, new object[1]
      {
        (object) Const.ClassifFolderKeyAttributeTypeId
      })).AsEnumerable().Select<DataRow, string>((System.Func<DataRow, string>) (x => x[0].ToString())).ToList<string>();
  }

  private bool IsCatalogOrFolderGtc(List<string> gtcCatalogsList, long objId)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBAttribute attributeById = sessionKeeper.Session.GetObject(objId).GetAttributeByID(Const.ClassifFolderKeyAttributeTypeId);
      if (attributeById == null || attributeById.AsString == string.Empty || attributeById.AsString.Length < 2)
        return false;
      string str = attributeById.AsString.Substring(0, 2);
      return gtcCatalogsList.Contains(str);
    }
  }
}
