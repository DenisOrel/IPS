// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.NumbersNode
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Intermech.Interfaces;
using System.Xml;

#nullable disable
namespace Intermech.MRP2;

internal class NumbersNode : AssemblyNode
{
  private int _start;
  private int _end;

  protected override void init(long objectID, IUserSession session)
  {
    this._id = 0L;
    this._caption = "→";
    this._fguid = "";
  }

  public NumbersNode(AssemblyNode parent, int start, int end)
    : base((ComplectNode) parent, 0L, (IUserSession) null)
  {
    this._start = start;
    this._end = end;
  }

  public NumbersNode(AssemblyNode parent, XmlNode node)
    : base((ComplectNode) parent, 0L, (IUserSession) null)
  {
    this.Start = node.Attributes["from"].Value;
    this.End = node.Attributes["to"].Value;
  }

  public string Start
  {
    get => this._start <= 0 ? "" : this._start.ToString();
    set
    {
      if (int.TryParse(value, out this._start))
        return;
      this._start = -1;
    }
  }

  public string End
  {
    get => this._end <= 0 ? "" : this._end.ToString();
    set
    {
      if (int.TryParse(value, out this._end))
        return;
      this._end = -1;
    }
  }

  internal int s => this._start;

  internal int e => this._end;

  public override void SaveToXml(XmlNode parentNode)
  {
    XmlElement element = parentNode.OwnerDocument.CreateElement("num");
    XmlAttribute attribute1 = parentNode.OwnerDocument.CreateAttribute("from");
    attribute1.Value = this.Start;
    element.Attributes.Append(attribute1);
    XmlAttribute attribute2 = parentNode.OwnerDocument.CreateAttribute("to");
    attribute2.Value = this.End;
    element.Attributes.Append(attribute2);
    parentNode.AppendChild((XmlNode) element);
  }
}
