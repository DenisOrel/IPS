// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.VisDialogs.VisStyleNode
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Pdm;
using System;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace Intermech.Pdm.VisDialogs;

public class VisStyleNode
{
  public List<GlobalType> CatList;
  public StyleNodeData Data;

  public StyleKind Kind { get; set; }

  public string Name { get; set; }

  public VisStyleNode(StyleKind k, string name)
  {
    this.Kind = k;
    this.CatList = new List<GlobalType>();
    this.Name = name;
    switch (this.Kind)
    {
      case StyleKind.CommonObject:
        this.Data = (StyleNodeData) new ObjNodeData();
        break;
      case StyleKind.ObjPreview:
        this.Data = (StyleNodeData) new PreviewNodeData();
        break;
      case StyleKind.Relation:
        this.Data = (StyleNodeData) new LinkNodeData();
        break;
    }
  }

  public VisStyleNode(XmlNode node, IUserSession ius)
  {
    this.CatList = new List<GlobalType>();
    if (!node.HasChildNodes)
      return;
    int category = 4;
    foreach (XmlNode childNode in node.ChildNodes)
    {
      if (childNode.NodeType == XmlNodeType.Element && childNode.Name == nameof (Kind))
      {
        this.Kind = (StyleKind) Convert.ToInt32(childNode.InnerText);
        if (this.Kind == StyleKind.Relation)
          category = 6;
      }
      else
      {
        if (childNode.NodeType == XmlNodeType.Element && childNode.Name == nameof (Name))
          this.Name = childNode.InnerText;
        if (childNode.NodeType == XmlNodeType.Element && childNode.Name == "TypeId")
          this.CatList.Add(new GlobalType(Convert.ToInt32(childNode.InnerText), category, ius));
      }
    }
    switch (this.Kind)
    {
      case StyleKind.CommonObject:
        this.Data = (StyleNodeData) new ObjNodeData(node, ius);
        break;
      case StyleKind.ObjPreview:
        this.Data = (StyleNodeData) new PreviewNodeData(node, ius);
        break;
      case StyleKind.Relation:
        this.Data = (StyleNodeData) new LinkNodeData(node, ius);
        break;
    }
  }

  public void WriteToXml(XmlTextWriter writer)
  {
    writer.WriteStartElement("Style");
    this.SaveToXml(writer);
    this.Data.SaveToXml(writer);
    writer.WriteEndElement();
  }

  public virtual void SaveToXml(XmlTextWriter writer)
  {
    writer.WriteElementString("Kind", ((int) this.Kind).ToString());
    writer.WriteElementString("Name", this.Name);
    foreach (GlobalType cat in this.CatList)
      writer.WriteElementString("TypeId", cat.TypeID.ToString());
  }
}
