// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.VisDialogs.PreviewNodeData
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using System;
using System.Xml;

#nullable disable
namespace Intermech.Pdm.VisDialogs;

public class PreviewNodeData : StyleNodeData
{
  public int PreviewScale { get; set; }

  public string UpperHint { get; set; }

  public string LowerHint { get; set; }

  public PreviewNodeData()
  {
    this.PreviewScale = 100;
    this.UpperHint = "";
    this.LowerHint = "";
  }

  public PreviewNodeData(XmlNode node, IUserSession ius)
    : base(node, ius)
  {
    if (!node.HasChildNodes)
      return;
    foreach (XmlNode childNode in node.ChildNodes)
    {
      if (childNode.NodeType == XmlNodeType.Element && childNode.Name == "Scale")
      {
        this.PreviewScale = Convert.ToInt32(childNode.InnerText);
      }
      else
      {
        if (childNode.NodeType == XmlNodeType.Element && childNode.Name == nameof (UpperHint))
          this.UpperHint = childNode.InnerText;
        if (childNode.NodeType == XmlNodeType.Element && childNode.Name == nameof (LowerHint))
          this.LowerHint = childNode.InnerText;
      }
    }
  }

  public override void SaveToXml(XmlTextWriter writer)
  {
    base.SaveToXml(writer);
    writer.WriteElementString("Scale", this.PreviewScale.ToString());
    writer.WriteElementString("UpperHint", this.UpperHint);
    writer.WriteElementString("LowerHint", this.LowerHint);
  }
}
