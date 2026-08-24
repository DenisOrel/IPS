// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.AttributeType
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

public class AttributeType : ITagImportObject
{
  public string Name = string.Empty;
  public string ShortName = string.Empty;
  public string Alias = string.Empty;
  public string DefaultValue = string.Empty;
  public Guid AttrGuid = Guid.Empty;
  public long Size;
  public long SystemID;
  public FieldTypes FieldType;
  public List<int> ValuesListIds = new List<int>();
  public Dictionary<int, string> ValuesListMeasureIDs = new Dictionary<int, string>();
  public MultiValueModes MultiValueMode;

  public AttributeType()
  {
  }

  public AttributeType(
    string name,
    string shortName,
    string alias,
    string defaultValue,
    Guid guid,
    long size,
    long systemID,
    FieldTypes fieldType,
    List<int> valuesListIDs,
    Dictionary<int, string> valuesListMeasureIDs,
    MultiValueModes multiValueMode)
  {
    this.AttrGuid = guid;
    this.Name = name;
    this.ShortName = shortName;
    this.Alias = alias;
    this.DefaultValue = defaultValue;
    this.Size = size;
    this.SystemID = systemID;
    this.FieldType = fieldType;
    this.ValuesListIds = valuesListIDs;
    this.MultiValueMode = multiValueMode;
    this.ValuesListMeasureIDs = valuesListMeasureIDs;
  }

  public byte[] Save()
  {
    using (MemoryStream output = new MemoryStream())
    {
      BinaryWriter binaryWriter = new BinaryWriter((Stream) output, Encoding.UTF8);
      try
      {
        binaryWriter.Write(this.Alias.Length);
        if (this.Alias.Length > 0)
          binaryWriter.Write(this.Alias.ToCharArray());
        string str = this.AttrGuid.ToString();
        binaryWriter.Write(str.Length);
        if (str.Length > 0)
          binaryWriter.Write(str.ToCharArray());
        binaryWriter.Write(this.DefaultValue.Length);
        if (this.DefaultValue.Length > 0)
          binaryWriter.Write(this.DefaultValue.ToCharArray());
        binaryWriter.Write((int) this.FieldType);
        binaryWriter.Write((int) this.MultiValueMode);
        binaryWriter.Write(this.Name.Length);
        if (this.Name.Length > 0)
          binaryWriter.Write(this.Name.ToCharArray());
        binaryWriter.Write(this.ShortName.Length);
        if (this.ShortName.Length > 0)
          binaryWriter.Write(this.ShortName.ToCharArray());
        binaryWriter.Write(this.Size);
        binaryWriter.Write(this.SystemID);
        binaryWriter.Write(this.ValuesListIds.Count);
        for (int index = 0; index < this.ValuesListIds.Count; ++index)
          binaryWriter.Write(this.ValuesListIds[index]);
        binaryWriter.Write(this.ValuesListMeasureIDs.Count);
        foreach (KeyValuePair<int, string> valuesListMeasureId in this.ValuesListMeasureIDs)
        {
          binaryWriter.Write(valuesListMeasureId.Key);
          binaryWriter.Write(valuesListMeasureId.Value.Length);
          if (valuesListMeasureId.Value.Length > 0)
            binaryWriter.Write(valuesListMeasureId.Value.ToCharArray());
        }
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
          this.Alias = TagImportObjectHelper.GetString(length1, br);
        int length2 = br.ReadInt32();
        if (length2 > 0)
          this.AttrGuid = new Guid(TagImportObjectHelper.GetString(length2, br));
        int length3 = br.ReadInt32();
        if (length3 > 0)
          this.DefaultValue = TagImportObjectHelper.GetString(length3, br);
        this.FieldType = (FieldTypes) br.ReadInt32();
        this.MultiValueMode = (MultiValueModes) br.ReadInt32();
        int length4 = br.ReadInt32();
        if (length4 > 0)
          this.Name = TagImportObjectHelper.GetString(length4, br);
        int length5 = br.ReadInt32();
        if (length5 > 0)
          this.ShortName = TagImportObjectHelper.GetString(length5, br);
        this.Size = br.ReadInt64();
        this.SystemID = br.ReadInt64();
        int capacity1 = br.ReadInt32();
        this.ValuesListIds = new List<int>(capacity1);
        for (int index = 0; index < capacity1; ++index)
          this.ValuesListIds.Add(br.ReadInt32());
        int capacity2 = br.ReadInt32();
        this.ValuesListMeasureIDs = new Dictionary<int, string>(capacity2);
        for (int index = 0; index < capacity2; ++index)
        {
          int key = br.ReadInt32();
          int length6 = br.ReadInt32();
          if (length6 > 0)
            this.ValuesListMeasureIDs.Add(key, TagImportObjectHelper.GetString(length6, br));
        }
      }
      finally
      {
        br.Close();
      }
    }
  }

  public short ClassID => 5;
}
