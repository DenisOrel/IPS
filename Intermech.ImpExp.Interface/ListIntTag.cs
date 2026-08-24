// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.ListIntTag
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System.Collections.Generic;
using System.IO;
using System.Text;

#nullable disable
namespace Intermech.ImpExp.Interface;

public class ListIntTag : ITagImportObject
{
  public List<int> Items;

  public ListIntTag()
  {
  }

  public ListIntTag(List<int> items) => this.Items = items;

  public byte[] Save()
  {
    using (MemoryStream output = new MemoryStream())
    {
      BinaryWriter binaryWriter1 = new BinaryWriter((Stream) output, Encoding.UTF8);
      try
      {
        BinaryWriter binaryWriter2 = binaryWriter1;
        List<int> items = this.Items;
        // ISSUE: explicit non-virtual call
        int count = items != null ? __nonvirtual (items.Count) : 0;
        binaryWriter2.Write(count);
        if (this.Items != null)
        {
          foreach (int num in this.Items)
            binaryWriter1.Write(num);
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
      BinaryReader binaryReader = new BinaryReader((Stream) input, Encoding.UTF8);
      try
      {
        int capacity = binaryReader.ReadInt32();
        this.Items = new List<int>(capacity);
        for (int index = 0; index < capacity; ++index)
          this.Items.Add(binaryReader.ReadInt32());
      }
      finally
      {
        binaryReader.Close();
      }
    }
  }

  public virtual short ClassID => 24;
}
