// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.PumpCache
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using Intermech.ImpExp.Interface;
using Intermech.Interfaces.Client;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp;

public static class PumpCache
{
  private static CategoryList _categoryList = new CategoryList();

  public static CategoryList Category => PumpCache._categoryList;

  public static void CloseAll()
  {
    Dictionary<ImportingCategory, CacheCategory>.KeyCollection keys = PumpCache._categoryList.Keys;
    ImportingCategory[] array = new ImportingCategory[keys.Count];
    keys.CopyTo(array, 0);
    (ServicesManager.GetService(typeof (ICache)) as ICache).ReleaseCache(array);
    PumpCache._categoryList.Clear();
  }
}
