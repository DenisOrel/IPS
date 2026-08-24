// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Data.Obj2Link.Obj2LinkInfoObject
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Data.Obj2Link;

[Serializable]
internal class Obj2LinkInfoObject
{
  public Obj2LinkInfoObject(int key, int objKey, byte objType, int artTcKey)
  {
    this.Key = key;
    this.ObjKey = objKey;
    this.ObjType = objType;
    this.ArtTcKey = artTcKey;
  }

  public int Key { get; private set; }

  public int ObjKey { get; private set; }

  public byte ObjType { get; private set; }

  public int ArtTcKey { get; private set; }
}
