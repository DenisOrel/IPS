// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.SpecificationSection
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

#nullable disable
namespace Intermech.ImpExp.SearchData;

internal class SpecificationSection
{
  public readonly long ObjectID;
  public readonly string Caption;
  public object Tag;

  public SpecificationSection(long id, string caption)
  {
    this.ObjectID = id;
    this.Caption = caption;
  }
}
