// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalNavigator.PortalTypesQuery
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.WebPortal;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Queries;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Site.Client.PortalNavigator;

public class PortalTypesQuery : BaseNodeQuery
{
  private readonly INodeQuerySupport _support;
  protected List<PortalObjectType> items;
  private readonly List<PortalObjectType> rows = new List<PortalObjectType>();
  private static readonly object[] _fieldsOrder = new object[1]
  {
    (object) "F_CAPTION"
  };
  private readonly int _parentID;

  public PortalTypesQuery(INodeQuerySupport support, int parentID)
  {
    this._support = support;
    this._parentID = parentID;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IPortalMetadata service = (IPortalMetadata) ServicesManager.GetService(typeof (IPortalMetadata));
      if (service == null)
        return;
      PortalObjectType[] childObjectTypes = service.GetChildObjectTypes(sessionKeeper.Session, this._parentID, false);
      this.items = childObjectTypes != null ? new List<PortalObjectType>((IEnumerable<PortalObjectType>) childObjectTypes) : new List<PortalObjectType>();
    }
  }

  protected override INodeQuerySupport Support => this._support;

  protected override NodeQueryResult Execute(object bookmark, int count, RecordMapping mapping)
  {
    if (mapping != null && mapping.SortFields != null && mapping.SortFields.Length != 0)
    {
      bool flag = false;
      NodeColumnSortOrder nodeColumnSortOrder = NodeColumnSortOrder.None;
      for (int index = 0; index < mapping.SortFields.Length; ++index)
      {
        flag = mapping.SortFields[index].Equals((object) "F_CAPTION");
        if (flag)
        {
          nodeColumnSortOrder = mapping.SortOrders == null || mapping.SortOrders.Length == 0 ? NodeColumnSortOrder.Ascending : mapping.SortOrders[index];
          break;
        }
      }
      if (flag && nodeColumnSortOrder == NodeColumnSortOrder.Descending)
        this.items.Sort((IComparer<PortalObjectType>) new PortalTypesQuery.DescPortalObjectTypeComparer());
    }
    int position1 = bookmark != null ? ((PositionBookmark) bookmark).Position : 0;
    if (position1 + count > this.items.Count)
      count = this.items.Count - position1;
    if (count <= 0)
      return NodeQueryResult.Empty;
    this.rows.Clear();
    for (int index = 0; index < count; ++index)
      this.rows.Add(this.items[position1 + index]);
    int position2 = position1 + count;
    return new NodeQueryResult(position2 < this.items.Count ? (object) new PositionBookmark(position2) : (object) (PositionBookmark) null, count, this.TotalRecordCount, PortalTypesQuery._fieldsOrder);
  }

  protected override NodeQueryResult Execute(object[] recordIds, RecordMapping mapping)
  {
    this.rows.Clear();
    for (int index1 = 0; index1 < recordIds.Length; ++index1)
    {
      int index2 = this.items.IndexOf(recordIds[index1] as PortalObjectType);
      if (index2 >= 0)
        this.rows.Add(this.items[index2]);
    }
    return new NodeQueryResult(this.rows.Count, this.TotalRecordCount, PortalTypesQuery._fieldsOrder);
  }

  protected override object[] GetFieldValues(int index)
  {
    return new object[2]
    {
      (object) this.rows[index].Name,
      (object) this.rows[index]
    };
  }

  private class DescPortalObjectTypeComparer : IComparer<PortalObjectType>
  {
    public int Compare(PortalObjectType x, PortalObjectType y) => -x.Name.CompareTo(y.Name);
  }
}
