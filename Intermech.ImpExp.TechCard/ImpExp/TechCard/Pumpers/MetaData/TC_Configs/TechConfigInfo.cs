// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.TC_Configs.TechConfigInfo
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.TC_Configs;

[Serializable]
internal class TechConfigInfo
{
  private readonly int _key;
  private readonly int _id;
  private readonly string _config;
  private readonly int _production;
  private readonly int _userId;
  private readonly string _bigData;

  public TechConfigInfo(
    int key,
    int id,
    string config,
    int production,
    int userId,
    string bigData)
  {
    this._key = key;
    this._id = id;
    this._config = config;
    this._production = production;
    this._userId = userId;
    this._bigData = bigData;
  }

  public int Key => this._key;

  public int Id => this._id;

  public string Config => this._config;

  public int Production => this._production;

  public int UserId => this._userId;

  public string BigData => this._bigData;
}
