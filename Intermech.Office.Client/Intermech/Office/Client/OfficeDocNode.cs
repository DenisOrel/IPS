// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.OfficeDocNode
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Parts;
using Intermech.Office.Interfaces;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Office.Client;

internal class OfficeDocNode : ObjectNode
{
  public bool Resolutions { get; set; }

  public OfficeDocNode(int objTypeID, long objID)
    : base(objTypeID, objID)
  {
    this.Options = NodeOptions.None;
  }

  protected override List<PartSlot> CreateNonFolderSlots()
  {
    return this.SlotsFromSinglePart((INodePart) new RelatedObjectsPart(this._objTypeID, this._objID, RelatedObjectsRole.Composition, this.Resolutions ? OfficeConsts.ReltypeOfficeCompositionID : -1, this.Services));
  }

  public override NodeColumnCollection GetDefaultColumns(ContentType content)
  {
    return (content & ContentType.NonFolders) != ContentType.NonFolders ? TreeResolutionsView.DefaultTreeColumns : base.GetDefaultColumns(content);
  }

  public override NodeColumnCollection GetSupportedColumns(
    ContentType content,
    string columnSetName)
  {
    return (content & ContentType.NonFolders) != ContentType.NonFolders ? TreeResolutionsView.SupportedTreeColumns : base.GetSupportedColumns(content, columnSetName);
  }
}
