// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.ImbaseGroupAttributes
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

/// <summary>таблица Imbase</summary>
public class ImbaseGroupAttributes : ITagImportObject
{
  public List<GroupAttribute> Attributes;

  public ImbaseGroupAttributes() => this.Attributes = new List<GroupAttribute>(1);

  public ImbaseGroupAttributes(List<GroupAttribute> attributes) => this.Attributes = attributes;

  public byte[] Save()
  {
    using (MemoryStream output = new MemoryStream())
    {
      BinaryWriter binaryWriter = new BinaryWriter((Stream) output, Encoding.UTF8);
      try
      {
        if (this.Attributes == null)
        {
          binaryWriter.Write(0);
        }
        else
        {
          binaryWriter.Write(this.Attributes.Count);
          foreach (GroupAttribute attribute in this.Attributes)
          {
            binaryWriter.Write(attribute.ExistInBase);
            binaryWriter.Write(attribute.AttrFieldType);
            binaryWriter.Write(attribute.DataMode);
            binaryWriter.Write(attribute.DataType);
            binaryWriter.Write(attribute.EnterMode);
            binaryWriter.Write(attribute.Flags);
            binaryWriter.Write(attribute.Key);
            binaryWriter.Write(attribute.PumpPosible);
            binaryWriter.Write(attribute.Required);
            binaryWriter.Write(attribute.Sort);
            binaryWriter.Write(attribute.Width);
            binaryWriter.Write(attribute.Data.Length);
            if (attribute.Data.Length > 0)
              binaryWriter.Write(attribute.Data.ToCharArray());
            binaryWriter.Write(attribute.Field.Length);
            if (attribute.Field.Length > 0)
              binaryWriter.Write(attribute.Field.ToCharArray());
            binaryWriter.Write(attribute.Units.Length);
            if (attribute.Units.Length > 0)
              binaryWriter.Write(attribute.Units.ToCharArray());
            binaryWriter.Write(attribute.LongName.Length);
            if (attribute.LongName.Length > 0)
              binaryWriter.Write(attribute.LongName.ToCharArray());
            string str = attribute.AttrGuid.ToString();
            binaryWriter.Write(str.Length);
            if (str.Length > 0)
              binaryWriter.Write(str.ToCharArray());
          }
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
        int capacity = br.ReadInt32();
        if (capacity == 0)
        {
          this.Attributes = (List<GroupAttribute>) null;
        }
        else
        {
          this.Attributes = new List<GroupAttribute>(capacity);
          for (int index = 0; index < capacity; ++index)
          {
            GroupAttribute groupAttribute = new GroupAttribute();
            groupAttribute.ExistInBase = br.ReadBoolean();
            groupAttribute.AttrFieldType = br.ReadInt32();
            groupAttribute.DataMode = br.ReadInt32();
            groupAttribute.DataType = br.ReadInt32();
            groupAttribute.EnterMode = br.ReadInt32();
            groupAttribute.Flags = br.ReadInt32();
            groupAttribute.Key = br.ReadInt32();
            groupAttribute.PumpPosible = br.ReadInt32();
            groupAttribute.Required = br.ReadInt32();
            groupAttribute.Sort = br.ReadInt32();
            groupAttribute.Width = br.ReadInt64();
            int length1 = br.ReadInt32();
            if (length1 > 0)
              groupAttribute.Data = TagImportObjectHelper.GetString(length1, br);
            int length2 = br.ReadInt32();
            if (length2 > 0)
              groupAttribute.Field = TagImportObjectHelper.GetString(length2, br);
            int length3 = br.ReadInt32();
            if (length3 > 0)
              groupAttribute.Units = TagImportObjectHelper.GetString(length3, br);
            int length4 = br.ReadInt32();
            if (length4 > 0)
              groupAttribute.LongName = TagImportObjectHelper.GetString(length4, br);
            int length5 = br.ReadInt32();
            if (length5 > 0)
              groupAttribute.AttrGuid = new Guid(TagImportObjectHelper.GetString(length5, br));
            this.Attributes.Add(groupAttribute);
          }
        }
      }
      finally
      {
        br.Close();
      }
    }
  }

  public short ClassID => 4;
}
