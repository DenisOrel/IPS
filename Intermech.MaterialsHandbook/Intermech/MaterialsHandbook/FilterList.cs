// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.FilterList
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class FilterList
{
  private Dictionary<Guid, SortamentFilter> _dict = new Dictionary<Guid, SortamentFilter>();

  internal int Count => this._dict.Count;

  internal Dictionary<Guid, SortamentFilter> Dict => this._dict;

  internal SortamentFilter this[Guid g]
  {
    get
    {
      return (this._dict.ContainsKey(g) ? this._dict[g] : (SortamentFilter) null) ?? new SortamentFilter(Guid.Empty, Condition.None, (object) null);
    }
  }

  internal void Clear() => this._dict.Clear();

  internal void SetValue(Guid g, Condition cond, object value)
  {
    if (!(g != Guid.Empty))
      return;
    if (this._dict.ContainsKey(g))
    {
      if (cond != Condition.None)
      {
        this._dict[g].Cond = cond;
        this._dict[g].Value = value;
      }
      else
        this._dict.Remove(g);
    }
    else
    {
      if (cond == Condition.None)
        return;
      this._dict.Add(g, new SortamentFilter(g, cond, value));
    }
  }
}
