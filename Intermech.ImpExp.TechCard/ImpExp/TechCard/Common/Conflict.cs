// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Common.Conflict
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;

#nullable disable
namespace Intermech.ImpExp.TechCard.Common;

internal class Conflict
{
  private readonly Guid _tpTypeGuid;
  private readonly string _caption;
  private readonly string _description;
  private readonly string _comments;
  private int _key;

  public Conflict(int key, Guid tpTypeGuid, string caption, string description, string comments)
  {
    this._caption = caption;
    this._comments = comments;
    this._description = description;
    this._tpTypeGuid = tpTypeGuid;
    this._key = key;
  }

  public int Key
  {
    get => this._key;
    set => this._key = value;
  }

  public string Caption => this._caption;

  public string Comments => this._comments;

  public string Description => this._description;

  public Guid TP_type_Guid => this._tpTypeGuid;
}
