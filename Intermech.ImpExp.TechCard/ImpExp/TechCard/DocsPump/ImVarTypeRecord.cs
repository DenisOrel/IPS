// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.DocsPump.ImVarTypeRecord
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;
using System.Xml;

#nullable disable
namespace Intermech.ImpExp.TechCard.DocsPump;

internal class ImVarTypeRecord : ImDocTypeRecord
{
  public bool GroupVariant;
  public int SortNumber;

  public ImVarTypeRecord(string id, string pId, string recCode)
    : base(id, pId, recCode)
  {
  }

  public ImVarTypeRecord(XmlNode node)
    : base(node)
  {
  }

  public override void WriteDataToXml(ref XmlTextWriter writer)
  {
    base.WriteDataToXml(ref writer);
    writer.WriteElementString("Class", "VT");
    writer.WriteElementString("GroupVariant", this.GroupVariant ? "Y" : "N");
    writer.WriteElementString("SortNumber", Convert.ToString(this.SortNumber));
  }

  public override void LoadDataFromXml(XmlNode node)
  {
    base.LoadDataFromXml(node);
    if (!node.HasChildNodes)
      return;
    foreach (XmlNode childNode in node.ChildNodes)
    {
      if (childNode.NodeType == XmlNodeType.Element && childNode.Name == "GroupVariant")
        this.GroupVariant = childNode.InnerText == "Y";
      else if (childNode.NodeType == XmlNodeType.Element && childNode.Name == "SortNumber")
        this.SortNumber = Convert.ToInt32(childNode.InnerText);
    }
  }
}
