// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechExpPump.TablesPump.TableHeader
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;
using System.IO;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechExpPump.TablesPump;

[Serializable]
public class TableHeader
{
  public char[] Signature;
  public ushort HeaderLen;
  public ushort VersionNum;
  public ushort TableCount;
  public int IndexPos;

  public void Load(BinaryReader br)
  {
    this.Signature = br.ReadChars(4);
    this.HeaderLen = br.ReadUInt16();
    this.VersionNum = br.ReadUInt16();
    this.TableCount = br.ReadUInt16();
    this.IndexPos = br.ReadInt32();
  }
}
