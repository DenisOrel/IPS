// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.ProductionListsCache
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using Intermech.ImpExp.Interface;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

#nullable disable
namespace Intermech.ImpExp.SearchData;

internal sealed class ProductionListsCache
{
  private readonly List<Tuple<int, int, int>> _articles;
  private readonly List<int> _categories;

  public ProductionListsCache()
  {
    this._articles = new List<Tuple<int, int, int>>();
    using (IDataReader dataReader = BasePumpHelper.S4Query("select z.zakaz_id, z.part_aid, z.part_ver, a.art_ver_id from zpc z, articles a where z.part_aid = a.art_id and z.zakaz_id > 0"))
    {
      while (dataReader.Read())
        this._articles.Add(new Tuple<int, int, int>(Convert.ToInt32(dataReader["zakaz_id"]), Convert.ToInt32(dataReader["part_aid"]), ProductionListsCache.GetVersionNo(Convert.ToInt32(dataReader["part_ver"]), Convert.ToInt32(dataReader["art_ver_id"]))));
    }
    this._categories = new List<int>();
  }

  public void Close()
  {
    if (this._categories.Count <= 0)
      return;
    (ServicesManager.GetService(typeof (ICache)) as ICache).ReleaseCache(this._categories.ToArray());
  }

  public void AddArticle(ImportingObject importingObject, int artID, int artVerID)
  {
    foreach (int zakazID in this._articles.Where<Tuple<int, int, int>>((System.Func<Tuple<int, int, int>, bool>) (x => x.Item2 == artID && x.Item3 == artVerID)).Select<Tuple<int, int, int>, int>((System.Func<Tuple<int, int, int>, int>) (x => x.Item1)))
      this.AddToCache(zakazID, importingObject, artID, artVerID);
  }

  private void AddToCache(int zakazID, ImportingObject importingObject, int artID, int artVerID)
  {
    long oldKey = ProductionListsCache.CacheKey(artID, artVerID);
    ICache service = ServicesManager.GetService(typeof (ICache)) as ICache;
    int categoryId = ProductionListsCache.GetCategoryID(zakazID);
    int[] numArray = new int[1]{ categoryId };
    IImportingData cache = service.GetCache(numArray);
    if (!this._categories.Contains(categoryId))
      this._categories.Add(categoryId);
    if (cache.GetNewKey((object) oldKey) != 0L)
      return;
    cache.AddValue((object) oldKey, long.MinValue, (ITagImportObject) new ImportingObjectTag(importingObject));
  }

  public static int GetVersionNo(int part_ver, int art_ver_id)
  {
    int versionNo = part_ver;
    if (versionNo == -1)
      versionNo = art_ver_id;
    else if (versionNo < -1)
      versionNo = -1 * versionNo - 2;
    return versionNo;
  }

  public static long CacheKey(int id, int versionId)
  {
    return Convert.ToInt64(id) << 32 /*0x20*/ | (long) (uint) versionId;
  }

  public static int GetCategoryID(int zakazID) => int.MaxValue - zakazID;
}
