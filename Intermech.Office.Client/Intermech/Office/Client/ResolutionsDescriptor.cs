// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.ResolutionsDescriptor
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Diagnostics;
using Intermech.Kernel.Search;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.VirtualNodes;
using System;

#nullable disable
namespace Intermech.Office.Client;

internal class ResolutionsDescriptor : HiveDescriptor
{
  private readonly long _objectID;
  private readonly int _objectType;
  [CanBeNull]
  private readonly IServiceProvider _provider;

  public ResolutionsDescriptor(long objectID, int objectType, [CanBeNull] IServiceProvider provider)
    : base(OfficeClientConsts.CategoryResolutionsRoot, 0, "Поручения")
  {
    Intermech.Diagnostics.Check.Argument(objectID != 0L, "objectID != Consts.UnknownObjectId");
    this._objectID = objectID;
    this._objectType = objectType;
    this._provider = provider;
  }

  [CanBeNull]
  public override object MapColumnToField(NodeColumn column)
  {
    object field = base.MapColumnToField(column);
    if (field != null)
      return field;
    return (column.SchemeGuid == Intermech.Navigator.Consts.ObjectColumnSchemeGuid || column.SchemeGuid == Intermech.Navigator.Consts.CurrentObjectColumnSchemeGuid || column.SchemeGuid == Intermech.Navigator.Consts.ObjectObligatoryColumnSchemeGuid) && column.ID.Equals((object) ObligatoryObjectAttributes.CAPTION) ? (object) "F_CAPTION" : (object) null;
  }

  [NotNull]
  public override INode GetChild(INodeID nodeID)
  {
    return (INode) new ResolutionsRootNode(this._objectID, this._objectType, this._provider);
  }
}
