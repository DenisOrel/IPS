// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.IMHRootNode
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.MaterialsHandbook;
using Intermech.Navigator;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Parts;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class IMHRootNode : CompositeNode, IContextAware
{
  private AdvancedServiceContainer _services = new AdvancedServiceContainer();

  public IServiceProvider Services
  {
    get => (IServiceProvider) this._services;
    set => this._services.AdvancedProvider = value;
  }

  protected override List<PartSlot> CreateFolderSlots()
  {
    List<PartSlot> folderSlots = (List<PartSlot>) null;
    if (IMHHelper.ChildNodesColl.ContainsKey(Consts.IMHRootNodeCategoryID))
    {
      List<NodeInfo> nodeInfoList = IMHHelper.ChildNodesColl[Consts.IMHRootNodeCategoryID];
      folderSlots = new List<PartSlot>(nodeInfoList.Count);
      DescriptorCollection chilNodesDecrColl = new DescriptorCollection();
      nodeInfoList.ForEach((Action<NodeInfo>) (x => chilNodesDecrColl.Add((IDescriptor) new VirtualNodeDescriptor(Consts.IMHRootNodeCategoryID, x.ID, x.Caption))));
      folderSlots.Add(new PartSlot(Consts.IMHRootNodeGuid, (INodePart) new DescriptorsPart(chilNodesDecrColl)));
    }
    return folderSlots;
  }

  public override INode GetChild(INodeID nodeID)
  {
    return nodeID == null ? base.GetChild(nodeID) : (INode) new VirtualNode(Consts.IMHRootNodeCategoryID, nodeID.CategoryID);
  }

  public override object GetData(INodeID nodeID, Type dataFormat)
  {
    object obj = (object) null;
    if (dataFormat == typeof (IIMHNode))
      obj = (object) new IMHNode(Consts.IMHRootNodeCategoryID, nodeID.CategoryID, (List<long>) null);
    else if (dataFormat == typeof (IDBTypedObjectID))
    {
      long num = 0;
      obj = (object) new DBTypedObjectID(0, num, num, string.Empty, num, 0L, 0L, string.Empty, num);
    }
    return obj ?? base.GetData(nodeID, dataFormat);
  }

  public override NodeColumnCollection GetDefaultColumns(ContentType content)
  {
    IColumnSchemes service = ServiceUtils.GetService<IColumnSchemes>((object) ApplicationServices.Container, false);
    NodeColumnCollection defaultColumns;
    if (service != null)
    {
      defaultColumns = new NodeColumnCollection();
      NodeColumn column = service.CreateColumn(Intermech.Navigator.Consts.NavigatorColumnSchemeGuid, (object) "F_CAPTION", NodeColumnSortOrder.Ascending, 0);
      defaultColumns.Add(column, 500);
    }
    else
      defaultColumns = base.GetDefaultColumns(content);
    return defaultColumns;
  }
}
