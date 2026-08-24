// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.ImbaseTableAttributes
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
public class ImbaseTableAttributes : ITagImportObject
{
  public List<TableAttribute> Attributes;

  public ImbaseTableAttributes() => this.Attributes = new List<TableAttribute>(1);

  public ImbaseTableAttributes(List<TableAttribute> attributes) => this.Attributes = attributes;

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
          foreach (TableAttribute attribute in this.Attributes)
          {
            string str1 = attribute.AttributeGuid.ToString();
            binaryWriter.Write(str1.Length);
            if (str1.Length > 0)
              binaryWriter.Write(str1.ToCharArray());
            binaryWriter.Write((int) attribute.AddMode);
            binaryWriter.Write((int) attribute.ComputeMode);
            binaryWriter.Write((int) attribute.InViewMode);
            binaryWriter.Write((int) attribute.MaskFlag);
            binaryWriter.Write((int) attribute.EnterMode);
            int num1 = attribute.IsTableRecRef ? 1 : 0;
            binaryWriter.Write(num1);
            int num2 = attribute.IsGuid ? 1 : 0;
            binaryWriter.Write(num2);
            binaryWriter.Write(attribute.ImFormula.Length);
            if (attribute.ImFormula.Length > 0)
              binaryWriter.Write(attribute.ImFormula.ToCharArray());
            binaryWriter.Write(attribute.DefVal.Length);
            if (attribute.DefVal.Length > 0)
              binaryWriter.Write(attribute.DefVal.ToCharArray());
            if (attribute.Measure != Guid.Empty)
            {
              string str2 = attribute.Measure.ToString();
              binaryWriter.Write(str2.Length);
              if (str2.Length > 0)
                binaryWriter.Write(str2.ToCharArray());
            }
            else
              binaryWriter.Write(0);
            binaryWriter.Write(attribute.Display.Length);
            if (attribute.Display.Length > 0)
              binaryWriter.Write(attribute.Display.ToCharArray());
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
          this.Attributes = (List<TableAttribute>) null;
        }
        else
        {
          this.Attributes = new List<TableAttribute>(capacity);
          for (int index = 0; index < capacity; ++index)
          {
            TableAttribute tableAttribute = new TableAttribute();
            int length1 = br.ReadInt32();
            if (length1 > 0)
              tableAttribute.AttributeGuid = new Guid(TagImportObjectHelper.GetString(length1, br));
            tableAttribute.AddMode = (RequiredModes) br.ReadInt32();
            tableAttribute.ComputeMode = (ComputeValueModes) br.ReadInt32();
            tableAttribute.InViewMode = (OptimizationModes) br.ReadInt32();
            tableAttribute.MaskFlag = (AttributeOptions) br.ReadInt32();
            tableAttribute.EnterMode = (ImEnterMode) br.ReadInt32();
            tableAttribute.IsTableRecRef = br.ReadInt32() == 1;
            tableAttribute.IsGuid = br.ReadInt32() == 1;
            int length2 = br.ReadInt32();
            if (length2 > 0)
              tableAttribute.ImFormula = TagImportObjectHelper.GetString(length2, br);
            int length3 = br.ReadInt32();
            if (length3 > 0)
              tableAttribute.DefVal = TagImportObjectHelper.GetString(length3, br);
            int length4 = br.ReadInt32();
            tableAttribute.Measure = length4 <= 0 ? Guid.Empty : new Guid(TagImportObjectHelper.GetString(length4, br));
            int length5 = br.ReadInt32();
            if (length5 > 0)
              tableAttribute.Display = TagImportObjectHelper.GetString(length5, br);
            this.Attributes.Add(tableAttribute);
          }
        }
      }
      finally
      {
        br.Close();
      }
    }
  }

  public short ClassID => 3;
}
