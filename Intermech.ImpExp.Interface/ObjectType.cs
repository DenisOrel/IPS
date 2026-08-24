// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.ObjectType
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System;
using System.IO;
using System.Text;

#nullable disable
namespace Intermech.ImpExp.Interface;

public class ObjectType : ITagImportObject
{
  public string Name = string.Empty;
  public string ShortName = string.Empty;
  public string InstanceName = string.Empty;
  public Guid Guid = Guid.Empty;
  public long SysID;
  public byte[] Icon;
  public ObjectVersionModes ObjectVersionMode = ObjectVersionModes.SingleVersion;
  public bool AnyAttribute = true;
  public Guid LcShema = Guid.Empty;
  public Guid DefaultRelation = Guid.Empty;
  public Guid ParentType = Guid.Empty;

  public ObjectType()
  {
  }

  public ObjectType(
    string name,
    string shortName,
    string instanceName,
    Guid guid,
    long sysID,
    byte[] icon,
    ObjectVersionModes versionable,
    bool anyAttribute,
    Guid LcShemaId,
    Guid defaultRelationID,
    Guid parentTypeId)
  {
    this.Name = name;
    this.ShortName = shortName;
    this.InstanceName = instanceName;
    this.Guid = guid;
    this.SysID = sysID;
    this.Icon = icon;
    this.ObjectVersionMode = versionable;
    this.AnyAttribute = anyAttribute;
    this.LcShema = LcShemaId;
    this.DefaultRelation = defaultRelationID;
    this.ParentType = parentTypeId;
  }

  public byte[] Save()
  {
    using (MemoryStream output = new MemoryStream())
    {
      BinaryWriter binaryWriter = new BinaryWriter((Stream) output, Encoding.UTF8);
      try
      {
        binaryWriter.Write(this.Name.Length);
        if (this.Name.Length > 0)
          binaryWriter.Write(this.Name.ToCharArray());
        binaryWriter.Write(this.ShortName.Length);
        if (this.ShortName.Length > 0)
          binaryWriter.Write(this.ShortName.ToCharArray());
        binaryWriter.Write(this.InstanceName.Length);
        if (this.InstanceName.Length > 0)
          binaryWriter.Write(this.InstanceName.ToCharArray());
        string str1 = this.Guid.ToString();
        binaryWriter.Write(str1.Length);
        if (str1.Length > 0)
          binaryWriter.Write(str1.ToCharArray());
        binaryWriter.Write(this.SysID);
        if (this.Icon != null)
        {
          binaryWriter.Write(this.Icon.Length);
          if (this.Icon.Length != 0)
            binaryWriter.Write(this.Icon);
        }
        else
          binaryWriter.Write(0);
        binaryWriter.Write((int) this.ObjectVersionMode);
        binaryWriter.Write(this.AnyAttribute);
        string str2 = this.LcShema.ToString();
        binaryWriter.Write(str2.Length);
        if (str2.Length > 0)
          binaryWriter.Write(str2.ToCharArray());
        string str3 = this.DefaultRelation.ToString();
        binaryWriter.Write(str3.Length);
        if (str3.Length > 0)
          binaryWriter.Write(str3.ToCharArray());
        string str4 = this.ParentType.ToString();
        binaryWriter.Write(str4.Length);
        if (str4.Length > 0)
          binaryWriter.Write(str4.ToCharArray());
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
        int length1 = br.ReadInt32();
        if (length1 > 0)
          this.Name = TagImportObjectHelper.GetString(length1, br);
        int length2 = br.ReadInt32();
        if (length2 > 0)
          this.ShortName = TagImportObjectHelper.GetString(length2, br);
        int length3 = br.ReadInt32();
        if (length3 > 0)
          this.InstanceName = TagImportObjectHelper.GetString(length3, br);
        int length4 = br.ReadInt32();
        if (length4 > 0)
          this.Guid = new Guid(TagImportObjectHelper.GetString(length4, br));
        this.SysID = br.ReadInt64();
        int count = br.ReadInt32();
        if (count > 0)
          this.Icon = br.ReadBytes(count);
        this.ObjectVersionMode = (ObjectVersionModes) br.ReadInt32();
        this.AnyAttribute = br.ReadBoolean();
        int length5 = br.ReadInt32();
        if (length5 > 0)
          this.LcShema = new Guid(TagImportObjectHelper.GetString(length5, br));
        int length6 = br.ReadInt32();
        if (length6 > 0)
          this.DefaultRelation = new Guid(TagImportObjectHelper.GetString(length6, br));
        int length7 = br.ReadInt32();
        if (length7 <= 0)
          return;
        this.ParentType = new Guid(TagImportObjectHelper.GetString(length7, br));
      }
      finally
      {
        br.Close();
      }
    }
  }

  public short ClassID => 7;
}
