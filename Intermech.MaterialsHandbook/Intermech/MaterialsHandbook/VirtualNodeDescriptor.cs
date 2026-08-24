// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.VirtualNodeDescriptor
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Persistence;
using Intermech.Navigator.VirtualNodes;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class VirtualNodeDescriptor : HiveDescriptor
{
  private int _parentCategoryID = Consts.IMHEmptyCategoryID;

  public override string Caption => this._caption;

  public VirtualNodeDescriptor(int parentCategoryID, int categoryID, string caption)
    : base(categoryID, -1, caption)
  {
    this._parentCategoryID = parentCategoryID;
  }

  protected VirtualNodeDescriptor(PersistentState state)
    : base(state)
  {
  }

  public override INode GetChild(INodeID nodeID)
  {
    return nodeID == null ? base.GetChild(nodeID) : (INode) new VirtualNode(this._parentCategoryID, nodeID.CategoryID);
  }

  public override object GetData(INodeID nodeID, Type dataFormat)
  {
    object obj = (object) null;
    if (dataFormat == typeof (IDescriptor))
      obj = (object) new VirtualNodeDescriptor(this._parentCategoryID, nodeID.CategoryID, this._caption);
    else if (dataFormat == typeof (IIMHNode))
      obj = (object) new IMHNode(this._parentCategoryID, nodeID.CategoryID, (List<long>) null);
    else if (dataFormat == typeof (ICanOpenInNewWindow))
      obj = (object) new CanOpenInNewWindow();
    return obj ?? base.GetData(nodeID, dataFormat);
  }
}
