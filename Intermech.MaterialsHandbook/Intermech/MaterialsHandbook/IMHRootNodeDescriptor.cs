// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.IMHRootNodeDescriptor
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.Localization;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Persistence;
using Intermech.Navigator.VirtualNodes;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class IMHRootNodeDescriptor : HiveDescriptor
{
  public static string RootNodeDescriptorCaption
  {
    get => LocalizationHolder.rm.GetString("IMH_RootNode_Caption");
  }

  public IMHRootNodeDescriptor()
    : base(Consts.IMHRootNodeCategoryID, -1, IMHRootNodeDescriptor.RootNodeDescriptorCaption)
  {
  }

  protected IMHRootNodeDescriptor(PersistentState state)
    : base(state)
  {
  }

  public override object GetData(INodeID nodeID, Type dataFormat)
  {
    object obj = (object) null;
    if (dataFormat == typeof (IDescriptor))
      obj = (object) new IMHRootNodeDescriptor();
    else if (dataFormat == typeof (IIMHNode))
      obj = (object) new IMHNode(Consts.IMHEmptyCategoryID, nodeID.CategoryID, (List<long>) null);
    else if (dataFormat == typeof (ICanOpenInNewWindow))
      obj = (object) new CanOpenInNewWindow();
    return obj ?? base.GetData(nodeID, dataFormat);
  }
}
