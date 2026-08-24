// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.VisDialogs.ObjNodeData
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using System.Xml;

#nullable disable
namespace Intermech.Pdm.VisDialogs;

public class ObjNodeData : StyleNodeData
{
  public string UpperStr { get; set; }

  public string UpperHint { get; set; }

  public string MainHint { get; set; }

  public string LowerStr { get; set; }

  public string LowerHint { get; set; }

  public ObjNodeData()
  {
    this.UpperStr = "{Заголовок объекта}";
    this.UpperHint = "{Заголовок объекта}\r\n{Идентификатор версии объекта}";
    this.MainHint = "{Заголовок объекта}\r\n{Идентификатор версии объекта}";
    this.LowerStr = "{Идентификатор версии объекта}";
    this.LowerHint = "{Заголовок объекта}\r\n{Идентификатор версии объекта}";
  }

  public ObjNodeData(XmlNode node, IUserSession ius)
    : base(node, ius)
  {
    if (!node.HasChildNodes)
      return;
    foreach (XmlNode childNode in node.ChildNodes)
    {
      if (childNode.NodeType == XmlNodeType.Element && childNode.Name == nameof (UpperStr))
        this.UpperStr = childNode.InnerText;
      else if (childNode.NodeType == XmlNodeType.Element && childNode.Name == nameof (UpperHint))
        this.UpperHint = childNode.InnerText;
      else if (childNode.NodeType == XmlNodeType.Element && childNode.Name == nameof (LowerStr))
        this.LowerStr = childNode.InnerText;
      else if (childNode.NodeType == XmlNodeType.Element && childNode.Name == nameof (LowerHint))
        this.LowerHint = childNode.InnerText;
      else if (childNode.NodeType == XmlNodeType.Element && childNode.Name == nameof (MainHint))
        this.MainHint = childNode.InnerText;
    }
  }

  public override void SaveToXml(XmlTextWriter writer)
  {
    base.SaveToXml(writer);
    writer.WriteElementString("UpperStr", this.UpperStr);
    writer.WriteElementString("UpperHint", this.UpperHint);
    writer.WriteElementString("MainHint", this.MainHint);
    writer.WriteElementString("LowerStr", this.LowerStr);
    writer.WriteElementString("LowerHint", this.LowerHint);
  }
}
