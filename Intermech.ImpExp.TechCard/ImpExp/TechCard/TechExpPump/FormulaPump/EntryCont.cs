// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechExpPump.FormulaPump.EntryCont
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;
using System.IO;
using System.Runtime.InteropServices;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechExpPump.FormulaPump;

internal class EntryCont
{
  public short FormWidth;
  public short CondWidth;
  public short ResType;
  public short FormAm;

  public int SizeOfEntryCont()
  {
    return 0 + Marshal.SizeOf<short>(this.FormWidth) + Marshal.SizeOf<short>(this.CondWidth) + Marshal.SizeOf<short>(this.ResType) + Marshal.SizeOf<short>(this.FormAm);
  }

  public bool Load(BinaryReader reader)
  {
    if (reader == null)
      return false;
    long position = reader.BaseStream.Position;
    this.FormWidth = reader.ReadInt16();
    this.CondWidth = reader.ReadInt16();
    this.ResType = reader.ReadInt16();
    this.FormAm = reader.ReadInt16();
    return reader.BaseStream.Position == position + (long) this.SizeOfEntryCont();
  }

  public bool Save(BinaryReader reader) => throw new Exception("Not implemented");
}
