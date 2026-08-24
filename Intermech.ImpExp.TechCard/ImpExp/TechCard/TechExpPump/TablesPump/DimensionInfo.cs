// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechExpPump.TablesPump.DimensionInfo
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechExpPump.TablesPump;

[Serializable]
public class DimensionInfo
{
  public readonly List<string> ArgList = new List<string>();
  public DimensionType dType;
  public ushort dSize;
  public List<bool> IsCond = new List<bool>();

  public void LoadFormFile(BinaryReader br, ushort version)
  {
    this.dType = (DimensionType) br.ReadByte();
    this.dSize = br.ReadUInt16();
    short num = br.ReadInt16();
    for (short index = 0; (int) index < (int) num; ++index)
    {
      byte length = br.ReadByte();
      this.ArgList.Add(new string(br.ReadChars(4)).Substring(0, (int) length));
    }
    if (!this.dType.Equals((object) DimensionType.CondResult))
      return;
    this.IsCond = new List<bool>();
    for (short index = 0; (int) index < (int) num; ++index)
      this.IsCond.Add(br.ReadBoolean());
  }
}
