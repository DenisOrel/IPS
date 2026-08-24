// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.ImbaseGroup
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System;
using System.IO;
using System.Text;

#nullable disable
namespace Intermech.ImpExp.Interface;

/// <summary>таблица Imbase</summary>
public class ImbaseGroup : ITagImportObject
{
  public int Key;
  public int TableType;
  public int State;
  public int Openmode;
  public int Order;
  public int Nextkey;
  public int TextID;
  public int GraphID;
  public int Access;
  public long ObjectID;
  public string TableName = string.Empty;
  public string Description = string.Empty;
  public string User = string.Empty;
  public DateTime Created = DateTime.Now;
  public DateTime Modified = DateTime.Now;
  public Guid RecordsTypeGuid = Guid.NewGuid();

  public ImbaseGroup()
  {
  }

  public ImbaseGroup(
    int key,
    int tableType,
    int state,
    int openmode,
    int order,
    int nextkey,
    int textID,
    int graphID,
    int access,
    string tableName,
    string description,
    string user,
    DateTime created,
    DateTime modified,
    Guid recordsTypeGuid)
  {
    this.Key = key;
    this.TableType = tableType;
    this.State = state;
    this.Openmode = openmode;
    this.Order = order;
    this.Nextkey = nextkey;
    this.TextID = textID;
    this.GraphID = graphID;
    this.Access = access;
    this.TableName = tableName;
    this.Description = description;
    this.User = user;
    this.Created = created;
    this.Modified = modified;
    this.RecordsTypeGuid = recordsTypeGuid;
    this.ObjectID = 0L;
  }

  public byte[] Save()
  {
    using (MemoryStream output = new MemoryStream())
    {
      BinaryWriter binaryWriter = new BinaryWriter((Stream) output, Encoding.UTF8);
      try
      {
        binaryWriter.Write(this.Key);
        binaryWriter.Write(this.TableType);
        binaryWriter.Write(this.State);
        binaryWriter.Write(this.Openmode);
        binaryWriter.Write(this.Order);
        binaryWriter.Write(this.Nextkey);
        binaryWriter.Write(this.TextID);
        binaryWriter.Write(this.GraphID);
        binaryWriter.Write(this.Access);
        binaryWriter.Write(this.ObjectID);
        binaryWriter.Write(this.TableName.Length);
        if (this.TableName.Length > 0)
          binaryWriter.Write(this.TableName.ToCharArray());
        binaryWriter.Write(this.Description.Length);
        if (this.Description.Length > 0)
          binaryWriter.Write(this.Description.ToCharArray());
        binaryWriter.Write(this.User.Length);
        if (this.User.Length > 0)
          binaryWriter.Write(this.User.ToCharArray());
        binaryWriter.Write(this.Created.Ticks);
        binaryWriter.Write(this.Modified.Ticks);
        string str = this.RecordsTypeGuid.ToString();
        binaryWriter.Write(str.Length);
        if (str.Length > 0)
          binaryWriter.Write(str.ToCharArray());
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
        this.Key = br.ReadInt32();
        this.TableType = br.ReadInt32();
        this.State = br.ReadInt32();
        this.Openmode = br.ReadInt32();
        this.Order = br.ReadInt32();
        this.Nextkey = br.ReadInt32();
        this.TextID = br.ReadInt32();
        this.GraphID = br.ReadInt32();
        this.Access = br.ReadInt32();
        this.ObjectID = br.ReadInt64();
        int length1 = br.ReadInt32();
        if (length1 > 0)
          this.TableName = TagImportObjectHelper.GetString(length1, br);
        int length2 = br.ReadInt32();
        if (length2 > 0)
          this.Description = TagImportObjectHelper.GetString(length2, br);
        int length3 = br.ReadInt32();
        if (length3 > 0)
          this.User = TagImportObjectHelper.GetString(length3, br);
        this.Created = new DateTime(br.ReadInt64());
        this.Modified = new DateTime(br.ReadInt64());
        int length4 = br.ReadInt32();
        if (length4 <= 0)
          return;
        this.RecordsTypeGuid = new Guid(TagImportObjectHelper.GetString(length4, br));
      }
      finally
      {
        br.Close();
      }
    }
  }

  public short ClassID => 2;
}
