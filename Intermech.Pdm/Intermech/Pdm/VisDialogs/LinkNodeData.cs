// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.VisDialogs.LinkNodeData
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml;

#nullable disable
namespace Intermech.Pdm.VisDialogs;

public class LinkNodeData : StyleNodeData
{
  public DashStyle DStyle { get; set; }

  public Color LineColor { get; set; }

  public Color HighlightColor { get; set; }

  public string AttrName { get; set; }

  public LinkNodeData()
  {
    this.DStyle = DashStyle.Solid;
    this.LineColor = Color.Gray;
    this.HighlightColor = Color.Gold;
    this.AttrName = "Количество";
  }

  public LinkNodeData(XmlNode node, IUserSession ius)
    : base(node, ius)
  {
    if (!node.HasChildNodes)
      return;
    foreach (XmlNode childNode in node.ChildNodes)
    {
      if (childNode.NodeType == XmlNodeType.Element && childNode.Name == "Dash-Style")
        this.DStyle = (DashStyle) Convert.ToInt32(childNode.InnerText);
      else if (childNode.NodeType == XmlNodeType.Element && childNode.Name == "Line-Color")
        this.LineColor = Color.FromArgb(Convert.ToInt32(childNode.InnerText));
      else if (childNode.NodeType == XmlNodeType.Element && childNode.Name == "Highlight-Color")
        this.HighlightColor = Color.FromArgb(Convert.ToInt32(childNode.InnerText));
      else if (childNode.NodeType == XmlNodeType.Element && childNode.Name == "Line-Attr")
        this.AttrName = childNode.InnerText;
    }
  }

  public override void SaveToXml(XmlTextWriter writer)
  {
    base.SaveToXml(writer);
    writer.WriteElementString("Dash-Style", ((int) this.DStyle).ToString());
    writer.WriteElementString("Line-Color", this.LineColor.ToArgb().ToString());
    writer.WriteElementString("Highlight-Color", this.HighlightColor.ToArgb().ToString());
    writer.WriteElementString("Line-Attr", this.AttrName);
  }
}
