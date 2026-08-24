// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareObjectNode
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces.Pdm;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Parts;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.Pdm.Compositions;

internal sealed class CompareObjectNode : ObjectNode, IComparable, ICompareObjectNode
{
  private CompareObjectPart _part;
  private readonly CompareObjectsListPart _parent;

  public CompareObjectNode(
    CompareObjectsListPart parent,
    CompareObjectsInfo info,
    BackgroundReaderComparer reader,
    int objTypeID,
    long objID,
    List<Tuple<long, int>> compareObjects,
    Dictionary<long, bool> refreshColumns)
    : base(objTypeID, objID)
  {
    this.options = NodeOptions.CanContainsComposition | NodeOptions.CanContainsRelationsList;
    this.CompareObjects = compareObjects;
    this.Info = info;
    this.Reader = (ICompareBackgroundReader) reader;
    this.CurrentDifferences = new CompareDifferences();
    this.RefreshColumns = refreshColumns;
    this._parent = parent;
  }

  public List<Tuple<long, int>> CompareObjects { get; }

  public long ObjectID => this._objID;

  public CompareObjectsInfo Info { get; set; }

  public int ObjectType => this._objTypeID;

  public bool RealQuery { get; set; } = true;

  public bool FromCompareView
  {
    get => this._parent.FromCompareView;
    set => this._parent.FromCompareView = value;
  }

  public ICompareBackgroundReader Reader { get; set; }

  public CompareDifferences CurrentDifferences { get; set; }

  public Dictionary<long, bool> RefreshColumns { get; set; }

  public override object GetData(INodeID nodeID, Type dataFormat)
  {
    return this._part != null ? this._part.GetData(nodeID, dataFormat) : base.GetData(nodeID, dataFormat);
  }

  public override NodeColumnCollection GetDefaultColumns(ContentType content)
  {
    return this._part != null ? this._part.GetDefaultColumns() : base.GetDefaultColumns(content);
  }

  public override INode GetChild(INodeID nodeID) => (INode) null;

  public override INodeQuery GetQuery(ContentType content)
  {
    List<QuerySlot> subQueries = new List<QuerySlot>();
    for (int index = 0; index < this.FolderSlots.Count; ++index)
    {
      INodeQuery query = this.FolderSlots[index].Object.GetQuery();
      if (query != null)
        subQueries.Add(new QuerySlot(this.FolderSlots[index].UniqueId, query));
    }
    return subQueries.Count == 0 ? (INodeQuery) null : this.CreateCompositeQuery(subQueries);
  }

  public override NodeColumnCollection GetSupportedColumns(
    ContentType content,
    string ColumnSetName)
  {
    return (content & ContentType.NonFolders) == ContentType.NonFolders && this._part != null ? this._part.GetSupportedColumns(ColumnSetName) : base.GetSupportedColumns(content, ColumnSetName);
  }

  protected override List<PartSlot> CreateFolderSlots()
  {
    IViewState service = (IViewState) this.Services.GetService(typeof (IViewState));
    if (service != null && (service.ViewState & ViewStateFlags.NodeInTree) == ViewStateFlags.NodeInTree)
      return (List<PartSlot>) null;
    if (!this.FromCompareView)
      return base.CreateFolderSlots();
    if (this._part == null)
      this._part = new CompareObjectPart(this.Services, this.ObjectType);
    return this.SlotsFromSinglePart((INodePart) this._part);
  }

  public int CompareTo(object obj) => !this.Equals(obj) ? 1 : 0;

  public override bool Equals(object obj)
  {
    return obj is CompareObjectNode compareObjectNode && compareObjectNode._objID == this._objID;
  }

  public override int GetHashCode() => this._objID.GetHashCode();

  public void ClearResult() => this.Info.Result = new Dictionary<long, DataTable>();
}
