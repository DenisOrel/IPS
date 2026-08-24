// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.ArticleTag
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System.Collections.Generic;
using System.IO;
using System.Text;

#nullable disable
namespace Intermech.ImpExp.Interface;

public class ArticleTag : ITagImportObject
{
  public int ID;
  public int VersionID;
  public ArticleFlag Flags;
  public Dictionary<int, long> Versions = new Dictionary<int, long>();

  public ArticleTag(int articleID, Dictionary<int, long> versions)
  {
    this.ID = articleID;
    this.Versions = versions;
  }

  public ArticleTag() => this.ID = 0;

  protected virtual void InternalSave(BinaryWriter bw)
  {
    bw.Write(this.ID);
    bw.Write(this.VersionID);
    bw.Write(this.Versions.Count);
    foreach (KeyValuePair<int, long> version in this.Versions)
    {
      bw.Write(version.Key);
      bw.Write(version.Value);
    }
    bw.Write((ushort) this.Flags);
  }

  protected virtual void InternalLoad(BinaryReader br)
  {
    this.ID = br.ReadInt32();
    this.VersionID = br.ReadInt32();
    int num = br.ReadInt32();
    for (int index = 0; index < num; ++index)
      this.Versions.Add(br.ReadInt32(), br.ReadInt64());
    this.Flags = (ArticleFlag) br.ReadUInt16();
  }

  public byte[] Save()
  {
    using (MemoryStream output = new MemoryStream())
    {
      BinaryWriter bw = new BinaryWriter((Stream) output, Encoding.UTF8);
      try
      {
        this.InternalSave(bw);
      }
      finally
      {
        bw.Flush();
      }
      return output.ToArray();
    }
  }

  public void Load(byte[] s)
  {
    using (MemoryStream input = new MemoryStream(s))
    {
      BinaryReader br = new BinaryReader((Stream) input, Encoding.UTF8);
      try
      {
        this.InternalLoad(br);
      }
      finally
      {
        br.Close();
      }
    }
  }

  public virtual short ClassID => 8;
}
