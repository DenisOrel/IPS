// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.InvalidObject
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

#nullable disable
namespace Intermech.ImpExp.SearchData;

internal struct InvalidObject(string type, int ID, int VerID, int invalidObjectType)
{
  public string Type = type;
  public int ID = ID;
  public int VerID = VerID;
  public int InvalidObjectType = invalidObjectType;
}
