// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.SignTag
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System.IO;
using System.Text;

#nullable disable
namespace Intermech.ImpExp.Interface;

public class SignTag : ITagImportObject
{
  public long SignedObjectID;

  public SignTag()
  {
  }

  public SignTag(long signedObjectID) => this.SignedObjectID = signedObjectID;

  public byte[] Save()
  {
    using (MemoryStream output = new MemoryStream())
    {
      BinaryWriter binaryWriter = new BinaryWriter((Stream) output, Encoding.UTF8);
      try
      {
        binaryWriter.Write(this.SignedObjectID);
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
      BinaryReader binaryReader = new BinaryReader((Stream) input, Encoding.UTF8);
      try
      {
        this.SignedObjectID = (long) binaryReader.ReadInt32();
      }
      finally
      {
        binaryReader.Close();
      }
    }
  }

  public virtual short ClassID => 17;
}
