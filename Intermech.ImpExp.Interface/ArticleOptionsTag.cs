// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.ArticleOptionsTag
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

#nullable disable
namespace Intermech.ImpExp.Interface;

public class ArticleOptionsTag : ITagImportObject
{
  public List<int> OptionValues;
  public Guid Guid;

  public ArticleOptionsTag()
  {
  }

  public ArticleOptionsTag(Guid guid, List<int> optionValues)
  {
    this.Guid = guid;
    this.OptionValues = optionValues;
  }

  public byte[] Save()
  {
    using (MemoryStream output = new MemoryStream())
    {
      BinaryWriter binaryWriter1 = new BinaryWriter((Stream) output, Encoding.UTF8);
      try
      {
        binaryWriter1.Write(this.Guid.ToString().ToCharArray());
        BinaryWriter binaryWriter2 = binaryWriter1;
        List<int> optionValues = this.OptionValues;
        // ISSUE: explicit non-virtual call
        int count = optionValues != null ? __nonvirtual (optionValues.Count) : 0;
        binaryWriter2.Write(count);
        if (this.OptionValues != null)
        {
          foreach (int optionValue in this.OptionValues)
            binaryWriter1.Write(optionValue);
        }
      }
      finally
      {
        binaryWriter1.Flush();
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
        this.Guid = new Guid(TagImportObjectHelper.GetString(36, br));
        int capacity = br.ReadInt32();
        this.OptionValues = new List<int>(capacity);
        for (int index = 0; index < capacity; ++index)
          this.OptionValues.Add(br.ReadInt32());
      }
      finally
      {
        br.Close();
      }
    }
  }

  public virtual short ClassID => 23;
}
