// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Common.InvalidKeyItems
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.TechCard.Common;

internal class InvalidKeyItems
{
  private readonly int _maxCount;
  private InvalidKeyItem _searchRec = new InvalidKeyItem(ImportingCategory.None, (object) 0);
  private readonly List<InvalidKeyItem> _items;

  public InvalidKeyItems(int maxCount = 30)
  {
    this._maxCount = maxCount;
    this._items = new List<InvalidKeyItem>(maxCount);
  }

  public bool Contains(ImportingCategory category, object key)
  {
    this._searchRec.Category = category;
    this._searchRec.Key = key;
    return this._items.Contains(this._searchRec);
  }

  public void Add(ImportingCategory category, object key)
  {
    this._items.Insert(0, new InvalidKeyItem(category, key));
    if (this._items.Count <= this._maxCount)
      return;
    this._items.RemoveAt(this._maxCount);
  }
}
