// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Common.DataCache.ImportingCategoryData
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Common.DataCache;

internal class ImportingCategoryData
{
  private readonly ImportingCategory[] _categories;
  private readonly IImportingData _data;

  public ImportingCategoryData(ImportingCategory[] categories, IImportingData data)
  {
    this._categories = categories;
    this._data = data;
  }

  public ImportingCategory[] Categories => this._categories;

  public IImportingData Data => this._data;
}
