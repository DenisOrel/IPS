// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechTypes.TechTypeInfo
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechTypes;

[Serializable]
public class TechTypeInfo
{
  protected TechTypeSett _typeSett;
  public string Type = string.Empty;
  public string Name = string.Empty;
  public string DopTypes = string.Empty;
  public bool Saving;
  public int PredefID;
  public int RecordID;

  public TechTypeSett TypeSett
  {
    get => this._typeSett;
    set => this._typeSett = value;
  }
}
