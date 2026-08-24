// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.ContainsNode
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces.Client;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Parts;
using Intermech.Navigator.Queries;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Pdm.Compositions;

internal class ContainsNode : ObjectNode, IContextAware
{
  public bool InProducts;
  public NodeColumnCollection Columns;
  public BackgroundReader Reader;
  public SearchSchemeID Scheme;
  public int[] SchemeObjectTypes;
  public int[] SchemeRelationTypes;
  public bool RealQuery = true;

  public long ObjectID => this._objID;

  public ContainsNode()
    : base(-1, 0L)
  {
  }

  public ContainsNode(int objectType, long objectID)
    : base(objectType, objectID)
  {
    this.SetOptions();
  }

  private void SetOptions()
  {
    this.options = NodeOptions.CanContainsComposition | NodeOptions.CanContainsRelationsList;
  }

  public INodeQuery GetReportQuery()
  {
    return (INodeQuery) new ContainsReportQuery((INodeQuerySupport) new ContainsPart(this._objTypeID, this.Services), this.ObjectID, this.Scheme, (BackgroundReader) null, this.InProducts, true, this.Columns, this.Reader);
  }

  protected override List<PartSlot> CreateNonFolderSlots() => (List<PartSlot>) null;

  protected override List<PartSlot> CreateFolderSlots()
  {
    return this.SlotsFromSinglePart((INodePart) new ContainsPart(this._objTypeID, this.Services));
  }

  public override IUpdateAnalyser GetAnalyser(
    NodeViewCapabilities capabilities,
    object sender,
    NotificationEventArgs e)
  {
    return e.EventName == "ObjectsCreated" ? (IUpdateAnalyser) null : base.GetAnalyser(capabilities, sender, e);
  }
}
