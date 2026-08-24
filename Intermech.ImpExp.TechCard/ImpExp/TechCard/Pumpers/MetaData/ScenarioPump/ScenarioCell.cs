// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.ScenarioPump.ScenarioCell
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.ScenarioPump;

[Serializable]
internal class ScenarioCell
{
  private const int DefaultWidth = 150;
  private readonly CellValueType _type;
  private readonly string _value;
  private readonly int _width;

  public ScenarioCell(string value, CellValueType type, int width = 150, int height = 21)
  {
    this._value = value;
    this._type = type;
    this._width = width;
    this.Height = height;
    this.DefaultValue = string.Empty;
  }

  public CellValueType Type => this._type;

  public bool IsReCountButton { get; set; }

  public string DefaultValue { get; set; }

  public string Value => this._value;

  public int Width => this._width;

  public int Height { get; set; }

  public string Anchor { get; set; }

  public override string ToString() => $"{this._value} ({this._width})";
}
