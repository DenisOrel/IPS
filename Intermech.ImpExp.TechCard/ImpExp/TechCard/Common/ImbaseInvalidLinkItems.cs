// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Common.ImbaseInvalidLinkItems
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.TechCard.Common;

internal class ImbaseInvalidLinkItems
{
  private readonly int _maxCount;
  private ImbaseInvalidLinkItems.ImbaseInvalidLinkItem _searchRec = new ImbaseInvalidLinkItems.ImbaseInvalidLinkItem(0, 0);
  private readonly List<ImbaseInvalidLinkItems.ImbaseInvalidLinkItem> _items;

  public ImbaseInvalidLinkItems(int maxCount = 30)
  {
    this._maxCount = maxCount;
    this._items = new List<ImbaseInvalidLinkItems.ImbaseInvalidLinkItem>(maxCount);
  }

  public bool Contains(int catalogId, int folderId)
  {
    this._searchRec.CatalogId = catalogId;
    this._searchRec.FolderId = folderId;
    return this._items.Contains(this._searchRec);
  }

  public void Add(int catalogId, int folderId)
  {
    this._items.Insert(0, new ImbaseInvalidLinkItems.ImbaseInvalidLinkItem(catalogId, folderId));
    if (this._items.Count <= this._maxCount)
      return;
    this._items.RemoveAt(this._maxCount);
  }

  private struct ImbaseInvalidLinkItem(int catalogId, int folderId) : 
    IEquatable<ImbaseInvalidLinkItems.ImbaseInvalidLinkItem>
  {
    public int CatalogId = catalogId;
    public int FolderId = folderId;

    public bool Equals(ImbaseInvalidLinkItems.ImbaseInvalidLinkItem other)
    {
      return this.CatalogId.Equals(other.CatalogId) && this.FolderId.Equals(other.FolderId);
    }
  }
}
