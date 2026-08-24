// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.CategoryList
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using Intermech.ImpExp.Interface;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp;

public class CategoryList : Dictionary<ImportingCategory, CacheCategory>
{
  public new CacheCategory this[ImportingCategory category]
  {
    get
    {
      if (this.ContainsKey(category))
        return base[category];
      CacheCategory cacheCategory = new CacheCategory(category);
      this.Add(category, cacheCategory);
      return cacheCategory;
    }
  }
}
