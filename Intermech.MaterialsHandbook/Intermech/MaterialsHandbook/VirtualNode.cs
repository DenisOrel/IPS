// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.VirtualNode
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.MaterialsHandbook;
using Intermech.Navigator;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Parts;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class VirtualNode : CompositeNode, IContextAware
{
  private AdvancedServiceContainer _services = new AdvancedServiceContainer();
  private int _parentCategoryID;
  private int _categoryID;
  private Dictionary<long, INode> _nodes = new Dictionary<long, INode>();
  private bool _canEdit;

  internal bool CanEdit => this._canEdit;

  public VirtualNode(int parentCategoryID, int categoryID)
  {
    this._parentCategoryID = parentCategoryID;
    this._categoryID = categoryID;
    this._canEdit = false;
  }

  public IServiceProvider Services
  {
    get => (IServiceProvider) this._services;
    set => this._services.AdvancedProvider = value;
  }

  protected override List<PartSlot> CreateNonFolderSlots() => new List<PartSlot>();

  protected override List<PartSlot> CreateFolderSlots()
  {
    List<PartSlot> folderSlots = (List<PartSlot>) null;
    if (IMHHelper.ChildNodesColl.ContainsKey(this._categoryID))
    {
      List<NodeInfo> nodeInfoList = IMHHelper.ChildNodesColl[this._categoryID];
      folderSlots = new List<PartSlot>(nodeInfoList.Count);
      DescriptorCollection chilNodesDescrColl = new DescriptorCollection();
      nodeInfoList.ForEach((Action<NodeInfo>) (x => chilNodesDescrColl.Add((IDescriptor) new VirtualNodeDescriptor(this._categoryID, x.ID, x.Caption))));
      folderSlots.Add(new PartSlot(Consts.IMHRootNodeGuid, (INodePart) new DescriptorsPart(chilNodesDescrColl)));
    }
    if (folderSlots == null)
    {
      (string, string) name = (string.Empty, string.Empty);
      if (this._categoryID == Consts.IMHMaterialsNodeCategoryID || this._categoryID == Consts.IMHStandardNodeCategoryID)
        name = ("BASE_MATERIALS_CTL", "Структура каталога для узла \"Материалы\"");
      else if (this._categoryID == Consts.IMHAssortmentNodeCategoryID)
        name = ("ASSORTMENT_FOLDER_NAME", "Структура папки для узла \"Сортамент\"");
      else if (this._categoryID == Consts.IMHProfilesNodeCategoryID)
        name = ("ADDITION_MATERIALS_CTL", "Структура каталога для узла \"Профили\"");
      else if (this._categoryID == Consts.IMHGluesHandbookNodeCategoryID)
        name = ("GLUE_FOLDER_NAME", "Структура папки для узла \"Справочник клеев\"");
      else if (this._categoryID == Consts.IMHCoatingsVarietiesNodeCategoryID)
        name = ("COATING_FOLDER_NAME", "Структура папки для узла \"Виды покрытий\"");
      else if (this._categoryID == Consts.IMHDetailsMaterialNodeCategoryID)
        name = ("MATERIAL_GROUPS_TABLE_NAME", "Таблица для узла \"Материал детали\"");
      else if (this._categoryID == Consts.IMHOilHandbookNodeCategoryID)
        name = ("OIL_FOLDER_NAME", "Структура папки для узла \"Масла и смазки\"");
      else if (this._categoryID == Consts.IMHVarnishHandbookNodeCategoryID)
        name = ("VARNISH_FOLDER_NAME", "Структура папки для узла \"Лакокрасочные материалы\"");
      if (!string.IsNullOrEmpty(name.Item1))
      {
        folderSlots = new List<PartSlot>(1);
        int objTypeID;
        long objectId = this.GetObjectID(name, out objTypeID, out this._canEdit);
        if (objectId != 0L)
        {
          int defaultRelationTypeId = MetaDataHelper.GetDefaultRelationTypeID(objTypeID);
          if (defaultRelationTypeId == -1)
            return folderSlots;
          VirtualNodeObjectPart part = new VirtualNodeObjectPart(this._parentCategoryID, this._categoryID, objTypeID, objectId, RelatedObjectsRole.Composition, defaultRelationTypeId, this.Services);
          folderSlots.Add(new PartSlot(objTypeID == Intermech.Imbase.Consts.ImbaseCatalogTypeID ? Intermech.Imbase.Consts.ImbaseCatalogTypeGUID : Intermech.Imbase.Consts.ImbaseFolderTypeGUID, (INodePart) part));
        }
      }
    }
    return folderSlots;
  }

  public override INode GetChild(INodeID nodeID)
  {
    INode node;
    if (nodeID is NodeID nodeId && nodeID.TypeID == Intermech.Imbase.Consts.ImbaseFolderTypeID)
    {
      if (this._nodes.ContainsKey(nodeId.ObjectID))
      {
        node = this._nodes[nodeId.ObjectID];
      }
      else
      {
        FolderNode folderNode = new FolderNode(this._categoryID, nodeId.ObjectID);
        this._nodes[nodeId.ObjectID] = (INode) folderNode;
        node = (INode) folderNode;
      }
    }
    else
      node = (INode) new VirtualNode(this._categoryID, nodeID.CategoryID);
    return node ?? base.GetChild(nodeID);
  }

  public override object GetData(INodeID nodeID, Type dataFormat)
  {
    object obj = (object) null;
    if (dataFormat == typeof (IIMHNode))
    {
      if (nodeID is NodeID nodeId1 && nodeId1.ObjectTypeID == Intermech.Imbase.Consts.ImbaseFolderTypeID)
      {
        List<long> tableRefIds = this._nodes != null ? (this._nodes.ContainsKey(nodeId1.ObjectID) ? (this._nodes[nodeId1.ObjectID] is FolderNode node ? node.TableRefIDs : (List<long>) null) : (List<long>) null) : (List<long>) null;
        obj = (object) new IMHNode(this._categoryID, nodeId1.ObjectTypeID, tableRefIds);
      }
      else
        obj = (object) new IMHNode(this._categoryID, nodeID.CategoryID, (List<long>) null);
    }
    else if (dataFormat == typeof (IDescriptor))
    {
      if (nodeID is NodeID nodeId2)
        obj = (object) new FolderNodeDescriptor(this._categoryID, nodeId2.ObjectID);
      if (nodeID is StandartFolderNodeID standartFolderNodeId)
        obj = (object) new FolderNodeDescriptor(standartFolderNodeId.Caption, this._categoryID);
    }
    else if (dataFormat == typeof (FolderNode) && nodeID is NodeID)
    {
      long objectId = ((NodeID) nodeID).ObjectID;
      obj = this._nodes == null || !this._nodes.ContainsKey(objectId) ? (object) new FolderNode(this._categoryID, ((NodeID) nodeID).ObjectID) : (object) this._nodes[objectId];
    }
    else if (dataFormat == typeof (ICanOpenInNewWindow))
    {
      if (nodeID is NodeID nodeId3 && nodeId3.ObjectTypeID == Intermech.Imbase.Consts.ImbaseFolderTypeID || this._categoryID == Consts.IMHStandardNodeCategoryID)
        return (object) null;
    }
    else if (dataFormat == typeof (IDBTypedObjectID))
    {
      long num = 0;
      obj = (object) new DBTypedObjectID(0, num, num, string.Empty, num, 0L, 0L, string.Empty, num);
    }
    return obj ?? base.GetData(nodeID, dataFormat);
  }

  public override NodeColumnCollection GetDefaultColumns(ContentType content)
  {
    bool flag = false;
    if (IMHHelper.ChildNodesColl.ContainsKey(this._categoryID))
      flag = true;
    else if (this._categoryID == Consts.IMHMaterialsNodeCategoryID || this._categoryID == Consts.IMHAssortmentNodeCategoryID)
      flag = this._parentCategoryID == Consts.IMHStandardNodeCategoryID;
    else if (this._categoryID == Consts.IMHDetailsMaterialNodeCategoryID)
      flag = true;
    NodeColumnCollection columnCollection = (NodeColumnCollection) null;
    if (flag)
    {
      IColumnSchemes service = ServiceUtils.GetService<IColumnSchemes>((object) ApplicationServices.Container, false);
      if (service != null)
      {
        columnCollection = new NodeColumnCollection();
        NodeColumn column = service.CreateColumn(Intermech.Navigator.Consts.NavigatorColumnSchemeGuid, (object) "F_CAPTION", NodeColumnSortOrder.Ascending, 0);
        columnCollection.Add(column, 500);
      }
    }
    return columnCollection ?? base.GetDefaultColumns(content);
  }

  private long GetObjectID((string, string) name, out int objTypeID, out bool canEdit)
  {
    long objectId = 0;
    objTypeID = -1;
    canEdit = false;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (sessionKeeper.Session.GetCustomService(typeof (IIMHSystemSettingsService)) is IIMHSystemSettingsService customService)
      {
        Guid objectGuidByName = customService.GetObjectGuidByName(name.Item1);
        if (Guid.Empty.Equals(objectGuidByName))
          return objectId;
        IDBObject dbObject = sessionKeeper.Session.GetObject(objectGuidByName, false);
        if (dbObject == null)
        {
          int num = (int) MessageBox.Show($"Не найден объект с глобальным идентификатором {objectGuidByName.ToString()}{Environment.NewLine}который используется для описания узла марочника:{Environment.NewLine}{name.Item2}", "Ошибка в настройке марочника", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return objectId;
        }
        objectId = dbObject.ObjectID;
        objTypeID = dbObject.ObjectType;
        if (dbObject is IDBSecurity dbSecurity)
          canEdit = dbSecurity.CheckAccess(ActionType.Edit, false, false);
      }
    }
    return objectId;
  }
}
