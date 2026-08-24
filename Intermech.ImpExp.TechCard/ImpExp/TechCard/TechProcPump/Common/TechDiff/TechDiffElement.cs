// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechProcPump.Common.TechDiff.TechDiffElement
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

#nullable disable
namespace Intermech.ImpExp.TechCard.TechProcPump.Common.TechDiff;

internal class TechDiffElement
{
  private readonly int _key;
  private readonly int _docTcKey;
  private readonly int _artTcKey;
  private readonly string _entity;
  private readonly int _row;
  private readonly string _strValue;
  private readonly double _numValue;
  private readonly int _entType;

  public TechDiffElement(
    int key,
    int docTcKey,
    int artTcKey,
    string entity,
    int row,
    string strValue,
    double numValue,
    int entType)
  {
    this._key = key;
    this._docTcKey = docTcKey;
    this._artTcKey = artTcKey;
    this._entity = entity;
    this._row = row;
    this._strValue = strValue;
    this._numValue = numValue;
    this._entType = entType;
  }

  public int Key => this._key;

  public int DocTcKEy => this._docTcKey;

  public int ArtTcKey => this._artTcKey;

  public string Entity => this._entity;

  public int Row => this._row;

  public string StrValue => this._strValue;

  public double NumValue => this._numValue;

  public int EntType => this._entType;
}
