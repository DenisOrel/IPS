// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareObjectsListPart
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces.Pdm;
using Intermech.Kernel.Search;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.DBObjectTypes;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Queries;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Pdm.Compositions;

internal sealed class CompareObjectsListPart : ObjectsPart
{
  private List<Tuple<long, int>> _compareObjects;
  private CompareObjectsInfo _info;
  public BackgroundReaderComparer _reader;
  private Dictionary<long, bool> _refreshColumns;
  private bool _fromCompareView;

  public CompareObjectsListPart(
    BackgroundReaderComparer reader,
    List<Tuple<long, int>> compareObjects,
    IServiceProvider services,
    Dictionary<int, bool> relationTypes,
    CompareObjectsInfo info,
    Dictionary<long, bool> refreshColumns)
    : base(services)
  {
    this._compareObjects = compareObjects;
    this._info = info;
    this.objTypeID = compareObjects[0].Item2;
    this._reader = reader;
    this._refreshColumns = refreshColumns;
  }

  protected override INodeQuery GetQuery(ConditionStructure[] conditions)
  {
    IServiceProvider services = this.Owner is IContextAware owner ? owner.Services : (IServiceProvider) null;
    IObjectTypeNodeOptionsHolder service = services != null ? services.GetService(typeof (IObjectTypeNodeOptionsHolder)) as IObjectTypeNodeOptionsHolder : (IObjectTypeNodeOptionsHolder) null;
    ObjectTypeNodeOptions objectTypeNodeOptions = ObjectTypeNodeOptions.None;
    if (service != null)
      objectTypeNodeOptions = service.Options;
    if ((objectTypeNodeOptions & ObjectTypeNodeOptions.EmptyQuery) == ObjectTypeNodeOptions.EmptyQuery)
      return (INodeQuery) null;
    long[] conditionValue = new long[this._compareObjects.Count];
    int num = 0;
    foreach (Tuple<long, int> compareObject in this._compareObjects)
      conditionValue[num++] = compareObject.Item1;
    return (INodeQuery) new CompareObjectsListQuery((INodeQuerySupport) this, this.objTypeID, ConditionStructure.Join(new ConditionStructure(-2, RelationalOperators.In, (object) conditionValue, LogicalOperators.AND, 0, false), conditions), services);
  }

  public override INode GetChild(INodeID nodeID)
  {
    return (INode) new CompareObjectNode(this, this._info, this._reader, ((NodeID) nodeID).ObjectTypeID, ((NodeID) nodeID).ObjectID, this._compareObjects, this._refreshColumns);
  }

  public override INodeID CreateNodeId(object[] fieldValues, RecordAdapter adapter)
  {
    NodeID nodeId = (NodeID) base.CreateNodeId(fieldValues, adapter);
    return (INodeID) new CompareObjectNodeID(nodeId.ObjectTypeID, nodeId.ObjectID, nodeId.ID, nodeId.CheckedOutBy, nodeId.PrjLinkID, nodeId.LCStepID, nodeId.Caption, nodeId.RelationTypeID, nodeId.Owner, nodeId.Sorting, nodeId.State, nodeId.Version, nodeId.BaseVersion, nodeId.SiteID, nodeId.ModificationID);
  }

  public override object GetData(INodeID nodeID, Type dataFormat)
  {
    return dataFormat == typeof (IPDMCompareObject) ? (object) true : base.GetData(nodeID, dataFormat);
  }

  public bool FromCompareView
  {
    get => this._fromCompareView;
    set => this._fromCompareView = value;
  }
}
