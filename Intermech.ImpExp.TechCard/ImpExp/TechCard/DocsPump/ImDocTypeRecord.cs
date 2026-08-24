// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.DocsPump.ImDocTypeRecord
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.Expert;
using System;
using System.Xml;

#nullable disable
namespace Intermech.ImpExp.TechCard.DocsPump;

internal class ImDocTypeRecord
{
  public string Id = "";
  public string parentId = "";
  public string recordCode = "";
  public int attrId;
  public bool DontRepeat;
  public TempFormula cond;

  public ImDocTypeRecord(string id, string pId, string recCode)
  {
    this.Id = id;
    this.parentId = pId;
    this.recordCode = recCode;
  }

  public ImDocTypeRecord(XmlNode node) => this.LoadDataFromXml(node);

  public void WriteToXml(ref XmlTextWriter writer)
  {
    writer.WriteStartElement("imDocRecord");
    this.WriteDataToXml(ref writer);
    writer.WriteEndElement();
  }

  public static ImDocTypeRecord LoadFromXml(XmlNode node)
  {
    string str = "";
    if (node.HasChildNodes)
    {
      foreach (XmlNode childNode in node.ChildNodes)
      {
        if (childNode.NodeType == XmlNodeType.Element && childNode.Name == "Class")
          str = childNode.InnerText;
      }
    }
    ImDocTypeRecord imDocTypeRecord;
    switch (str)
    {
      case "DT":
        imDocTypeRecord = (ImDocTypeRecord) new ImTextTypeRecord(node);
        break;
      case "VT":
        imDocTypeRecord = (ImDocTypeRecord) new ImVarTypeRecord(node);
        break;
      default:
        imDocTypeRecord = new ImDocTypeRecord(node);
        break;
    }
    return imDocTypeRecord;
  }

  public virtual void WriteDataToXml(ref XmlTextWriter writer)
  {
    writer.WriteElementString("Id", this.Id);
    writer.WriteElementString("parentId", this.parentId);
    writer.WriteElementString("recordCode", this.recordCode);
    writer.WriteElementString("attrId", Convert.ToString(this.attrId));
    writer.WriteElementString("DontRepeat", this.DontRepeat ? "Y" : "N");
    if (this.cond == null)
      return;
    this.cond.WriteToXML(ref writer);
  }

  public virtual void LoadDataFromXml(XmlNode node)
  {
    if (!node.HasChildNodes)
      return;
    foreach (XmlNode childNode in node.ChildNodes)
    {
      if (childNode.NodeType == XmlNodeType.Element && childNode.Name == "Formula")
        this.cond = new TempFormula(childNode);
      else if (childNode.NodeType == XmlNodeType.Element && childNode.Name == "Id")
        this.Id = childNode.InnerText;
      else if (childNode.NodeType == XmlNodeType.Element && childNode.Name == "parentId")
        this.parentId = childNode.InnerText;
      else if (childNode.NodeType == XmlNodeType.Element && childNode.Name == "recordCode")
        this.recordCode = childNode.InnerText;
      else if (childNode.NodeType == XmlNodeType.Element && childNode.Name == "attrId")
        this.attrId = Convert.ToInt32(childNode.InnerText);
      else if (childNode.NodeType == XmlNodeType.Element && childNode.Name == "DontRepeat")
        this.DontRepeat = childNode.InnerText == "Y";
    }
  }
}
