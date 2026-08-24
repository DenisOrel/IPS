// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.AssemblyNode
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Intermech.Interfaces;
using System.Xml;

#nullable disable
namespace Intermech.MRP2;

internal class AssemblyNode : ComplectNode
{
  private string _id_PKDSE;

  public string id_PKDSE => this._id_PKDSE;

  protected override void init(long objectID, IUserSession session)
  {
    this._objectID = objectID;
    if (!Consts.IsUndefinedObjectId(objectID))
    {
      AttributeValues[] attributesValues = session.GetObjectAttributesValues(objectID, new int[4]
      {
        -3,
        -50,
        -12,
        MRP2Consts.attrIdPKDSE_Id
      }, GetAttributeValuesModes.IncludeObligatoryAttributes, true);
      this._id = attributesValues[0].AsInteger;
      this._caption = attributesValues[1].AsString;
      this._fguid = attributesValues[2].AsString;
      this._id_PKDSE = attributesValues[3].AsString;
    }
    else
    {
      this._id = 0L;
      this._caption = "";
      this._fguid = "";
      this._id_PKDSE = "";
    }
  }

  public AssemblyNode(ComplectNode parent, long objectID, IUserSession session)
    : base(objectID, session)
  {
    this._parent = parent;
  }

  public AssemblyNode(ComplectNode parent, XmlNode node, IUserSession session)
    : base(node, session)
  {
    this._parent = parent;
    this._id = long.Parse(node.Attributes["id"].Value);
    this._objectID = long.Parse(node.Attributes["objectId"].Value);
    this._fguid = node.Attributes["f_guid"].Value;
    this._caption = node.Attributes["caption"].Value;
    this._id_PKDSE = node.Attributes["id_pkdse"].Value;
    if (!(node.LocalName == "ea"))
      return;
    foreach (XmlNode childNode in node.ChildNodes)
      this._childNodes.Add((object) new NumbersNode(this, childNode));
  }

  public override void SaveToXml(XmlNode parentNode)
  {
    XmlElement element = parentNode.OwnerDocument.CreateElement("ea");
    XmlAttribute attribute1 = parentNode.OwnerDocument.CreateAttribute("id");
    attribute1.Value = this._id.ToString();
    element.Attributes.Append(attribute1);
    XmlAttribute attribute2 = parentNode.OwnerDocument.CreateAttribute("objectId");
    attribute2.Value = this._objectID.ToString();
    element.Attributes.Append(attribute2);
    XmlAttribute attribute3 = parentNode.OwnerDocument.CreateAttribute("f_guid");
    attribute3.Value = this._fguid;
    element.Attributes.Append(attribute3);
    XmlAttribute attribute4 = parentNode.OwnerDocument.CreateAttribute("caption");
    attribute4.Value = this._caption;
    element.Attributes.Append(attribute4);
    XmlAttribute attribute5 = parentNode.OwnerDocument.CreateAttribute("id_pkdse");
    attribute5.Value = this._id_PKDSE;
    element.Attributes.Append(attribute5);
    parentNode.AppendChild((XmlNode) element);
    foreach (object childNode in this._childNodes)
      (childNode as ComplectNode).SaveToXml((XmlNode) element);
  }
}
