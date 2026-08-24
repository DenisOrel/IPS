// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Imbase.ItemFactories.ImCatalogItem
// Assembly: Intermech.ImpExp.Imbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 14B82A62-153A-4D0C-8A5E-F24874681A1E
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Imbase.dll

using Intermech.Interfaces.SelectionService;
using System;

#nullable disable
namespace Intermech.ImpExp.Imbase.ItemFactories;

internal class ImCatalogItem
{
  public int RecKey;
  public int RecOwner;
  public int RecLevel;
  public string RecNAME = "";
  public int RecSORT;
  public int RecMASK;
  public int RecTag1;
  public int RecTag2;
  public int RecTextID;
  public int RecGraphID;
  public DateTime RecCreated = DateTime.Now;
  public string RecUser = "";
  public int RecTag3;
  public int RecTag4;
  public string ClassifierKey = "";
  public Guid Guid;
  protected string classifierKeyGen = "";
  public long ObjectId;

  public string GetNextChildKey()
  {
    this.classifierKeyGen = ClassifierKeyValueGenerator.GetNextKeyValue(this.classifierKeyGen);
    return this.ClassifierKey + this.classifierKeyGen;
  }
}
