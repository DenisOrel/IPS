// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.FolderObjectPart
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.Kernel.Search;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Queries;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class FolderObjectPart : RelatedObjectsPart
{
  private FolderNode _parent;

  public int ParentCategoryID { get; }

  public FolderObjectPart(
    FolderNode parent,
    int parentCategoryID,
    long objID,
    RelatedObjectsRole role,
    int relTypeID,
    IServiceProvider services)
    : base(Intermech.Imbase.Consts.ImbaseFolderTypeID, objID, role, relTypeID, services)
  {
    this._parent = parent;
    this.ParentCategoryID = parentCategoryID;
  }

  public override object GetData(INodeID nodeID, Type dataFormat)
  {
    object obj = (object) null;
    if (dataFormat == typeof (IDescriptor) && nodeID is NodeID)
      obj = (object) new FolderNodeDescriptor(this.ParentCategoryID, ((NodeID) nodeID).ObjectID);
    return obj ?? base.GetData(nodeID, dataFormat);
  }

  protected override INodeQuery GetQuery(ConditionStructure[] conditions)
  {
    if (this.ParentCategoryID == Consts.IMHMaterialsNodeCategoryID)
      conditions = ConditionStructure.Join(new ConditionStructure[2]
      {
        new ConditionStructure(-7, RelationalOperators.Equal, (object) Intermech.Imbase.Consts.ImbaseFolderTypeID, LogicalOperators.OR, 1, false),
        new ConditionStructure(-7, RelationalOperators.Equal, (object) Intermech.Imbase.Consts.ImbaseTableRefTypeID, LogicalOperators.NONE, -1, false)
      }, conditions);
    else
      conditions = ConditionStructure.Join(new ConditionStructure(-7, RelationalOperators.Equal, (object) Intermech.Imbase.Consts.ImbaseFolderTypeID, LogicalOperators.NONE, 0, false), conditions);
    return (INodeQuery) new FolderQuery((INodeQuerySupport) this, this._objID, this._objTypeID, this._role, this._relTypeID, conditions);
  }

  public void SetTableRefs(List<long> list) => this._parent.TableRefIDs = list;
}
