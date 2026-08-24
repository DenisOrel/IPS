// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.MaterialTag
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System.IO;
using System.Text;

#nullable disable
namespace Intermech.ImpExp.Interface;

public class MaterialTag : ITagImportObject
{
  public string Name = "";

  public MaterialTag()
  {
  }

  public MaterialTag(string name) => this.Name = name;

  public byte[] Save()
  {
    using (MemoryStream output = new MemoryStream())
    {
      BinaryWriter binaryWriter = new BinaryWriter((Stream) output, Encoding.UTF8);
      try
      {
        binaryWriter.Write(this.Name.Length);
        if (this.Name.Length > 0)
          binaryWriter.Write(this.Name);
      }
      finally
      {
        binaryWriter.Flush();
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
        int length = br.ReadInt32();
        if (length <= 0)
          return;
        this.Name = TagImportObjectHelper.GetString(length, br);
      }
      finally
      {
        br.Close();
      }
    }
  }

  public short ClassID => 18;
}
