// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechExpPump.FormulaPump.FormulaHeader
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;
using System.IO;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechExpPump.FormulaPump;

internal struct FormulaHeader
{
  public string Key;
  public int Version;
  public short HdSize;
  public int FlSize;
  public int OlSize;
  public int OlItems;
  public int EntryAm;
  public byte DgtAfter;
  public bool TrZFlag;
  public short Res1;
  public byte Res2;
  public byte Res3;

  public bool Load(BinaryReader reader)
  {
    if (reader == null)
      return false;
    if (reader.BaseStream.Position != 0L)
      reader.BaseStream.Position = 0L;
    if (reader.ReadByte() != (byte) 4)
      return false;
    this.Key = new string(reader.ReadChars(4));
    bool flag = true;
    try
    {
      this.Version = reader.ReadInt32();
      this.HdSize = reader.ReadInt16();
      this.FlSize = reader.ReadInt32();
      this.OlSize = reader.ReadInt32();
      this.OlItems = reader.ReadInt32();
      this.EntryAm = reader.ReadInt32();
      this.DgtAfter = reader.ReadByte();
      this.TrZFlag = reader.ReadBoolean();
      this.Res1 = reader.ReadInt16();
      this.Res2 = reader.ReadByte();
      this.Res3 = reader.ReadByte();
    }
    catch (Exception ex)
    {
      flag = false;
      if (ex is OutOfMemoryException)
        throw;
    }
    return flag;
  }

  public bool Save(BinaryWriter writer) => throw new Exception("Not implemented yet!");
}
