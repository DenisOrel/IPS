// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechExpPump.FormulaPump.CompiledTokenRecord
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.TechExpPump.Common;
using System;
using System.IO;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechExpPump.FormulaPump;

[Serializable]
public class CompiledTokenRecord : ICloneable
{
  public byte Kind;
  public short Index;
  public short Offset;
  public object Value;

  public void Load(BinaryReader reader, int version)
  {
    if (reader == null)
      return;
    this.Kind = reader.ReadByte();
    switch (this.Kind)
    {
      case 1:
        this.Value = (object) reader.ReadInt32();
        break;
      case 2:
        this.Value = (object) (version >= TechExpert.Const.cnt_FileVersion ? reader.ReadDouble() : Real48.Real48ToDouble(reader));
        break;
      case 3:
        this.Value = (object) TechExpert.Utils.TechReadString(reader, version);
        break;
      case 4:
        this.Value = (object) reader.ReadBoolean();
        break;
      case 15:
        this.Index = reader.ReadInt16();
        break;
      case 181:
      case 182:
        this.Offset = reader.ReadInt16();
        break;
    }
  }

  public void Load(BinaryReader reader) => this.Load(reader, TechExpert.Const.cnt_FileVersion);

  public void Save(BinaryReader reader)
  {
    if (reader != null)
      throw new Exception("Not implemented yet!");
  }

  public object Clone()
  {
    return (object) new CompiledTokenRecord()
    {
      Kind = this.Kind,
      Index = this.Index,
      Offset = this.Offset,
      Value = this.Value
    };
  }
}
