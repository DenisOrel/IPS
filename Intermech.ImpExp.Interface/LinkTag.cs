// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.LinkTag
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

public sealed class LinkTag : ITagImportObject
{
  public List<Tuple<int, string>> Items { get; private set; }

  public LinkTag()
  {
  }

  public LinkTag(List<Tuple<int, string>> items) => this.Items = items;

  public byte[] Save()
  {
    using (MemoryStream output = new MemoryStream())
    {
      BinaryWriter bw = new BinaryWriter((Stream) output, Encoding.UTF8);
      try
      {
        BinaryWriter binaryWriter = bw;
        List<Tuple<int, string>> items = this.Items;
        // ISSUE: explicit non-virtual call
        int count = items != null ? __nonvirtual (items.Count) : 0;
        binaryWriter.Write(count);
        if (this.Items != null)
        {
          foreach (Tuple<int, string> tuple in this.Items)
          {
            bw.Write(tuple.Item1);
            TagImportObjectHelper.SetString(tuple.Item2, bw);
          }
        }
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
        int capacity = br.ReadInt32();
        this.Items = new List<Tuple<int, string>>(capacity);
        for (int index = 0; index < capacity; ++index)
          this.Items.Add(new Tuple<int, string>(br.ReadInt32(), TagImportObjectHelper.GetString(br)));
      }
      finally
      {
        br.Close();
      }
    }
  }

  public short ClassID => 26;
}
