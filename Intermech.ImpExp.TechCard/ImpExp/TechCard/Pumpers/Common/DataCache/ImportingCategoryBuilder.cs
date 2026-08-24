// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Common.DataCache.ImportingCategoryBuilder
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Common.DataCache;

internal class ImportingCategoryBuilder
{
  public static ImportingCategory[] DiffCategories(params ImportingCategory[][] values)
  {
    List<ImportingCategory> importingCategoryList = new List<ImportingCategory>(((IEnumerable<ImportingCategory[]>) values).Sum<ImportingCategory[]>((Func<ImportingCategory[], int>) (t => t.Length)));
    foreach (ImportingCategory[] collection in values)
      importingCategoryList.AddRange((IEnumerable<ImportingCategory>) collection);
    importingCategoryList.Sort();
    for (int index = importingCategoryList.Count - 1; index > 0; --index)
    {
      if (importingCategoryList[index] == importingCategoryList[index - 1])
        importingCategoryList.RemoveAt(index);
    }
    return importingCategoryList.ToArray();
  }
}
