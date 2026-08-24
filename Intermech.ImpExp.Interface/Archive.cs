// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.Archive
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System.IO;
using System.Text;

#nullable disable
namespace Intermech.ImpExp.Interface;

public class Archive : ITagImportObject
{
  public int ArchiveID;
  public int StrongSign;
  public int PersonId;
  public int ParentID;
  public int ChkRights;
  public int StorageId;
  public string SignStamp;
  public string Descriptio;
  public string Alias;
  public string FileName;

  public Archive(
    int archiveID,
    int strongSign,
    int personId,
    int parentID,
    int chkRights,
    int storageId,
    string signStamp,
    string descriptio,
    string alias,
    string fileName)
  {
    this.ArchiveID = archiveID;
    this.StrongSign = strongSign;
    this.PersonId = personId;
    this.ParentID = parentID;
    this.ChkRights = chkRights;
    this.StorageId = storageId;
    this.SignStamp = signStamp;
    this.Descriptio = descriptio;
    this.Alias = alias;
    this.FileName = fileName;
  }

  public Archive()
  {
  }

  public byte[] Save()
  {
    using (MemoryStream output = new MemoryStream())
    {
      BinaryWriter binaryWriter = new BinaryWriter((Stream) output, Encoding.UTF8);
      try
      {
        binaryWriter.Write(this.ArchiveID);
        binaryWriter.Write(this.StrongSign);
        binaryWriter.Write(this.PersonId);
        binaryWriter.Write(this.ParentID);
        binaryWriter.Write(this.ChkRights);
        binaryWriter.Write(this.StorageId);
        binaryWriter.Write(this.SignStamp.Length);
        if (this.SignStamp.Length > 0)
          binaryWriter.Write(this.SignStamp.ToCharArray());
        binaryWriter.Write(this.Descriptio.Length);
        if (this.Descriptio.Length > 0)
          binaryWriter.Write(this.Descriptio.ToCharArray());
        binaryWriter.Write(this.Alias.Length);
        if (this.Alias.Length > 0)
          binaryWriter.Write(this.Alias.ToCharArray());
        binaryWriter.Write(this.FileName.Length);
        if (this.FileName.Length > 0)
          binaryWriter.Write(this.FileName.ToCharArray());
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
        this.ArchiveID = br.ReadInt32();
        this.StrongSign = br.ReadInt32();
        this.PersonId = br.ReadInt32();
        this.ParentID = br.ReadInt32();
        this.ChkRights = br.ReadInt32();
        this.StorageId = br.ReadInt32();
        int length1 = br.ReadInt32();
        if (length1 > 0)
          this.SignStamp = TagImportObjectHelper.GetString(length1, br);
        int length2 = br.ReadInt32();
        if (length2 > 0)
          this.Descriptio = TagImportObjectHelper.GetString(length2, br);
        int length3 = br.ReadInt32();
        if (length3 > 0)
          this.Alias = TagImportObjectHelper.GetString(length3, br);
        int length4 = br.ReadInt32();
        if (length4 <= 0)
          return;
        this.FileName = TagImportObjectHelper.GetString(length4, br);
      }
      finally
      {
        br.Close();
      }
    }
  }

  public short ClassID => 6;
}
