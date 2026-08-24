// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.VirtualExemplars.VirtualExemplar
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using System;

#nullable disable
namespace Intermech.Pdm.VirtualExemplars;

internal class VirtualExemplar
{
  public long ArticleID;
  public int ExemplarObjectType = -1;
  public string Name = string.Empty;
  public string ObjectsName = string.Empty;
  public ArticlesInManufacture ArticlesInManufacture;
  private long _exemplarID;
  private long _saveExemplarID;
  public bool SetActive;
  public Guid Guid = Guid.NewGuid();

  public long ExemplarID
  {
    get => this._exemplarID;
    set
    {
      this._saveExemplarID = this._exemplarID;
      this._exemplarID = value;
    }
  }

  public VirtualExemplar(
    long articleID,
    int exemplarObjectType,
    ArticlesInManufacture articlesInManufacture)
  {
    this.ArticleID = articleID;
    this.ExemplarObjectType = exemplarObjectType;
    this.ArticlesInManufacture = articlesInManufacture;
  }

  public override string ToString()
  {
    return !(this.ObjectsName != string.Empty) ? this.Name : $"{this.ObjectsName} \"{this.Name}\"";
  }

  public void Rollback() => this._exemplarID = this._saveExemplarID;
}
