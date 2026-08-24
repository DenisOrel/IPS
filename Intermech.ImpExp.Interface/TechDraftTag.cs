// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.TechDraftTag
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System.Collections.Generic;
using System.IO;
using System.Text;

#nullable disable
namespace Intermech.ImpExp.Interface;

public class TechDraftTag : ITagImportObject
{
  public Dictionary<string, long> Drafts = new Dictionary<string, long>();
  public Dictionary<int, long> Versions = new Dictionary<int, long>();

  public TechDraftTag(Dictionary<string, long> drafts) => this.Drafts = drafts;

  public TechDraftTag()
  {
  }

  protected virtual void InternalSave(BinaryWriter bw)
  {
    bw.Write(this.Drafts.Count);
    foreach (KeyValuePair<string, long> draft in this.Drafts)
    {
      bw.Write(draft.Key.Length);
      if (draft.Key.Length > 0)
        bw.Write(draft.Key.ToCharArray());
      bw.Write(draft.Value);
    }
  }

  protected virtual void InternalLoad(BinaryReader br)
  {
    int num1 = br.ReadInt32();
    for (int index = 0; index < num1; ++index)
    {
      int length = br.ReadInt32();
      string key = "";
      if (length > 0)
        key = TagImportObjectHelper.GetString(length, br);
      long num2 = br.ReadInt64();
      this.Drafts.Add(key, num2);
    }
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

  public virtual short ClassID => 19;
}
