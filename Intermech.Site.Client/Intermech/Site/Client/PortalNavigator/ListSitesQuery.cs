// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalNavigator.ListSitesQuery
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.WebPortal;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Queries;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Intermech.Site.Client.PortalNavigator;

internal sealed class ListSitesQuery : BaseNodeQuery, IContextAware
{
  private readonly INodeQuerySupport _support;
  private readonly int _objectTypeId;
  private readonly List<SiteInfo> _items;
  private readonly List<SiteInfo> _rows = new List<SiteInfo>();
  private static readonly object[] _fieldsOrder = new object[1]
  {
    (object) "F_CAPTION"
  };

  public ListSitesQuery(INodeQuerySupport support, int objTypeID, IServiceProvider services)
  {
    this.Services = services;
    this._support = support;
    this._objectTypeId = objTypeID;
    ISitesCacheService customService = (ISitesCacheService) (ApplicationServices.Container.GetService(typeof (IMServerService)) as IMServerService).GetCustomService(typeof (ISitesCacheService));
    if (customService == null)
      return;
    this._items = customService.Sites;
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
        this._items.Sort((IComparer<SiteInfo>) new ListSitesQuery.DescSitesComparer());
    }
    int position1 = bookmark != null ? ((PositionBookmark) bookmark).Position : 0;
    if (position1 + count > this._items.Count)
      count = this._items.Count - position1;
    if (count <= 0)
      return NodeQueryResult.Empty;
    this._rows.Clear();
    for (int index = 0; index < count; ++index)
      this._rows.Add(this._items[position1 + index]);
    int position2 = position1 + count;
    return new NodeQueryResult(position2 < this._items.Count ? (object) new PositionBookmark(position2) : (object) (PositionBookmark) null, count, this.TotalRecordCount, ListSitesQuery._fieldsOrder);
  }

  protected override NodeQueryResult Execute(object[] recordIds, RecordMapping mapping)
  {
    this._rows.Clear();
    for (int index1 = 0; index1 < recordIds.Length; ++index1)
    {
      int index2 = this._items.IndexOf(recordIds[index1] as SiteInfo);
      if (index2 >= 0)
        this._rows.Add(this._items[index2]);
    }
    return new NodeQueryResult(this._rows.Count, this.TotalRecordCount, ListSitesQuery._fieldsOrder);
  }

  protected override object[] GetFieldValues(int index)
  {
    return new object[2]
    {
      (object) this._rows[index].Caption,
      (object) this._rows[index]
    };
  }

  public IServiceProvider Services { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

  private class DescSitesComparer : IComparer<SiteInfo>
  {
    public int Compare(SiteInfo x, SiteInfo y) => -x.Caption.CompareTo(y.Caption);
  }
}
