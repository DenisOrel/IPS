// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.DocsPump.ImTextTypeRecord
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;
using System.Xml;

#nullable disable
namespace Intermech.ImpExp.TechCard.DocsPump;

internal class ImTextTypeRecord : ImDocTypeRecord
{
  public string Template = "";
  public int Digits;

  public ImTextTypeRecord(string id, string pId, string recCode)
    : base(id, pId, recCode)
  {
  }

  public ImTextTypeRecord(XmlNode node)
    : base(node)
  {
  }

  public override void WriteDataToXml(ref XmlTextWriter writer)
  {
    base.WriteDataToXml(ref writer);
    writer.WriteElementString("Class", "DT");
    writer.WriteElementString("Template", this.Template);
    writer.WriteElementString("Digits", Convert.ToString(this.Digits));
  }

  public override void LoadDataFromXml(XmlNode node)
  {
    base.LoadDataFromXml(node);
    if (!node.HasChildNodes)
      return;
    foreach (XmlNode childNode in node.ChildNodes)
    {
      if (childNode.NodeType == XmlNodeType.Element && childNode.Name == "Template")
        this.Template = childNode.InnerText;
      else if (childNode.NodeType == XmlNodeType.Element && childNode.Name == "Digits")
        this.attrId = Convert.ToInt32(childNode.InnerText);
    }
  }
}
