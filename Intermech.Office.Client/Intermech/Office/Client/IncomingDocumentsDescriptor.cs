// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.IncomingDocumentsDescriptor
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Diagnostics;
using Intermech.Interfaces;
using Intermech.Kernel.Search;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.VirtualNodes;
using Intermech.Office.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.Office.Client;

internal sealed class IncomingDocumentsDescriptor : HiveDescriptor
{
  private readonly long _unitID;
  [NotNull]
  private readonly string _unitCaption;

  public IncomingDocumentsDescriptor(long unitID, [NotNull] string unitCaption)
    : base(OfficeClientConsts.CategoryIncomingDocuments, 0, "Входящие " + unitCaption)
  {
    this._unitID = unitID;
    this._unitCaption = unitCaption;
  }

  [NotNull]
  public override INode GetChild(INodeID nodeID)
  {
    return (INode) new IncomingDocumentsNode(this._unitID, this.GetObjects());
  }

  public override bool Equals(object obj)
  {
    return obj == null || obj.GetType() != typeof (IncomingDocumentsDescriptor) ? base.Equals(obj) : this._unitID == ((IncomingDocumentsDescriptor) obj)._unitID;
  }

  public override int GetHashCode() => base.GetHashCode();

  [NotNull]
  private List<long> GetObjects()
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      DataTable dataTable = sessionKeeper.Session.GetObjectCollection(OfficeConsts.ObjtypeDocumentsID).Select(new DBRecordSetParams(new ConditionStructure[2]
      {
        new ConditionStructure(0, RelationalOperators.InFiltrationTable, (object) this._unitID, LogicalOperators.AND, 0, false),
        new ConditionStructure(OfficeConsts.AttrOfficeDocumentTypeID, RelationalOperators.Equal, (object) 0, LogicalOperators.AND, 0, false)
      }, new object[1]{ (object) -2 }));
      List<long> objects = new List<long>();
      foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
        objects.Add(Convert.ToInt64(row[0]));
      return objects;
    }
  }

  public override object GetData(INodeID nodeID, Type dataFormat)
  {
    if (dataFormat == typeof (IDescriptor))
      return (object) new IncomingDocumentsDescriptor(this._unitID, this._unitCaption);
    return !(dataFormat == typeof (ICanOpenInNewWindow)) ? base.GetData(nodeID, dataFormat) : (object) new CanOpenInNewWindow();
  }

  [NotNull]
  public override INodeID GetRecordNodeID() => (INodeID) new IncomingDocumentsNodeID(this._unitID);
}
