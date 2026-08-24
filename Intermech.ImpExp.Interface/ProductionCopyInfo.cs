// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.ProductionCopyInfo
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System.IO;
using System.Text;

#nullable disable
namespace Intermech.ImpExp.Interface;

public class ProductionCopyInfo : ITagImportObject
{
  public int ObjectType;
  public long ID;
  public string Hash;

  public ProductionCopyInfo()
  {
    this.ObjectType = -1;
    this.ID = 0L;
    this.Hash = string.Empty;
  }

  public ProductionCopyInfo(int objectType, string hash, long id)
  {
    this.ObjectType = objectType;
    this.Hash = hash;
    this.ID = id;
  }

  public byte[] Save()
  {
    using (MemoryStream output = new MemoryStream())
    {
      BinaryWriter binaryWriter = new BinaryWriter((Stream) output, Encoding.UTF8);
      try
      {
        binaryWriter.Write(this.ID);
        binaryWriter.Write(this.ObjectType);
        binaryWriter.Write(this.Hash.Length);
        if (this.Hash.Length > 0)
          binaryWriter.Write(this.Hash);
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
        this.ID = br.ReadInt64();
        this.ObjectType = br.ReadInt32();
        int length = br.ReadInt32();
        if (length <= 0)
          return;
        this.Hash = TagImportObjectHelper.GetString(length, br);
      }
      finally
      {
        br.Close();
      }
    }
  }

  public short ClassID => 33;
}
