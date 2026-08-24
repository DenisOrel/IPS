// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechExpPump.FormulaPump.TokenRecord
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.TechExpPump.Common;
using System;
using System.IO;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechExpPump.FormulaPump;

[Serializable]
public class TokenRecord : ICloneable
{
  public byte Kind;
  public short Index;
  public string SValue;

  public void Load(BinaryReader reader, int version)
  {
    if (reader == null)
      return;
    this.Kind = reader.ReadByte();
    switch (this.Kind)
    {
      case 1:
      case 2:
      case 3:
      case 4:
        this.SValue = TechExpert.Utils.TechReadString(reader, version);
        break;
      case 15:
        this.Index = reader.ReadInt16();
        break;
    }
  }

  public void Load(BinaryReader reader) => this.Load(reader, TechExpert.Const.cnt_FileVersion);

  public void Save(BinaryReader reader) => throw new Exception("Not implemented yet!");

  public object Clone()
  {
    return (object) new TokenRecord()
    {
      Kind = this.Kind,
      Index = this.Index,
      SValue = this.SValue
    };
  }
}
