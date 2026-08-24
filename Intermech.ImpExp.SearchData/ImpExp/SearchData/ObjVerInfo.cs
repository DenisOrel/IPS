// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.ObjVerInfo
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

#nullable disable
namespace Intermech.ImpExp.SearchData;

internal class ObjVerInfo
{
  public int ID;
  public readonly int VerID;
  public readonly int ActualVerID;

  public ObjVerInfo(int id, int verID, int actualVerID = -1)
  {
    this.ID = id;
    this.VerID = verID;
    this.ActualVerID = actualVerID;
  }
}
