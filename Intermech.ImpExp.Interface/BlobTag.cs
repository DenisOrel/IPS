// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.BlobTag
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System;
using System.IO;
using System.Text;

#nullable disable
namespace Intermech.ImpExp.Interface;

public class BlobTag : ITagImportObject
{
  public int AttrID;
  public long ObjectID;
  public long BlobID;
  public DateTime ModifyDate;

  public BlobTag()
  {
  }

  public BlobTag(int attrID, long blobID, long objectID, DateTime modifyDate)
  {
    this.AttrID = attrID;
    this.BlobID = blobID;
    this.ObjectID = objectID;
    this.ModifyDate = modifyDate;
  }

  public byte[] Save()
  {
    using (MemoryStream output = new MemoryStream())
    {
      BinaryWriter binaryWriter = new BinaryWriter((Stream) output, Encoding.UTF8);
      try
      {
        binaryWriter.Write(this.AttrID);
        binaryWriter.Write(this.ObjectID);
        binaryWriter.Write(this.BlobID);
        binaryWriter.Write(this.ModifyDate.Ticks);
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
        this.AttrID = binaryReader.ReadInt32();
        this.ObjectID = binaryReader.ReadInt64();
        this.BlobID = binaryReader.ReadInt64();
        this.ModifyDate = new DateTime(binaryReader.ReadInt64());
      }
      finally
      {
        binaryReader.Close();
      }
    }
  }

  public virtual short ClassID => 25;
}
