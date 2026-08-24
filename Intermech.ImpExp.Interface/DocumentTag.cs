// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.DocumentTag
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Intermech.ImpExp.Interface;

public class DocumentTag : ArticleTag
{
  public Dictionary<int, Intermech.ImpExp.Interface.AddVersionInfo> AddVersionInfo = new Dictionary<int, Intermech.ImpExp.Interface.AddVersionInfo>();
  public int LCStep;
  public DocumentFlag Flags;

  public DocumentTag()
  {
  }

  public DocumentTag(int docID, Dictionary<int, long> versions)
    : base(docID, versions)
  {
  }

  public override short ClassID => 9;

  protected override void InternalSave(BinaryWriter bw)
  {
    base.InternalSave(bw);
    foreach (KeyValuePair<int, Intermech.ImpExp.Interface.AddVersionInfo> keyValuePair in this.AddVersionInfo)
    {
      bw.Write(keyValuePair.Key);
      bw.Write(keyValuePair.Value.AdvanFilesDate);
      bw.Write(keyValuePair.Value.FileDate.Ticks);
      bw.Write(keyValuePair.Value.FileSize);
      bw.Write(keyValuePair.Value.ContentModifiedDate.Ticks);
      bw.Write(keyValuePair.Value.FileCount);
    }
    bw.Write(this.LCStep);
    if (this.Flags <= DocumentFlag.None)
      return;
    bw.Write((ushort) this.Flags);
  }

  protected override void InternalLoad(BinaryReader br)
  {
    base.InternalLoad(br);
    int count = this.Versions.Count;
    for (int index = 0; index < count; ++index)
      this.AddVersionInfo.Add(br.ReadInt32(), new Intermech.ImpExp.Interface.AddVersionInfo()
      {
        AdvanFilesDate = br.ReadInt64(),
        FileDate = new DateTime(br.ReadInt64()),
        FileSize = br.ReadInt32(),
        ContentModifiedDate = new DateTime(br.ReadInt64()),
        FileCount = br.ReadInt16()
      });
    this.LCStep = br.ReadInt32();
    if (br.BaseStream.Position >= br.BaseStream.Length)
      return;
    this.Flags = (DocumentFlag) br.ReadUInt16();
  }

  public bool HasFlag(DocumentFlag flag) => (this.Flags & flag) == flag;
}
