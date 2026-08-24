// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.ComplectNode
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Intermech.Interfaces;
using System.Collections;
using System.Xml;

#nullable disable
namespace Intermech.MRP2;

internal class ComplectNode
{
  protected ArrayList _childNodes = new ArrayList();
  protected string _caption;
  protected ComplectNode _parent;
  protected long _objectID;
  protected long _id;
  protected string _fguid;

  protected virtual void init(long objectID, IUserSession session)
  {
    this._objectID = objectID;
    if (!Consts.IsUndefinedObjectId(objectID))
    {
      AttributeValues[] attributesValues = session.GetObjectAttributesValues(objectID, new int[3]
      {
        -3,
        -50,
        -12
      }, GetAttributeValuesModes.IncludeObligatoryAttributes, true);
      this._id = attributesValues[0].AsInteger;
      this._caption = attributesValues[1].AsString;
      this._fguid = attributesValues[2].AsString;
    }
    else
    {
      this._id = 0L;
      this._caption = "";
      this._fguid = "";
    }
  }

  public virtual void SaveToXml(XmlNode parentNode)
  {
    XmlElement element = parentNode.OwnerDocument.CreateElement("pv");
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
    parentNode.AppendChild((XmlNode) element);
    foreach (object childNode in this._childNodes)
      (childNode as ComplectNode).SaveToXml((XmlNode) element);
  }

  public ComplectNode(long objectID, IUserSession session) => this.init(objectID, session);

  public ComplectNode(XmlNode node, IUserSession session)
  {
    this._parent = (ComplectNode) null;
    this._id = long.Parse(node.Attributes["id"].Value);
    this._objectID = long.Parse(node.Attributes["objectId"].Value);
    this._fguid = node.Attributes["f_guid"].Value;
    this._caption = node.Attributes["caption"].Value;
    if (!(node.LocalName == "pv"))
      return;
    foreach (XmlNode childNode in node.ChildNodes)
      this._childNodes.Add((object) new AssemblyNode(this, childNode, session));
  }

  public ComplectNode(long objectID, long asmObjectId, int start, int end, IUserSession session)
  {
    this.init(objectID, session);
    AssemblyNode parent = new AssemblyNode(this, asmObjectId, session);
    this.ChildNodes.Add((object) parent);
    NumbersNode numbersNode = new NumbersNode(parent, start, end);
    parent.ChildNodes.Add((object) numbersNode);
  }

  public IList ChildNodes => (IList) this._childNodes;

  public string Caption => this._caption;

  public ComplectNode Parent => this._parent;

  public long ObjectID => this._objectID;

  public long ID => this._id;

  public ComplectNode Root
  {
    get
    {
      ComplectNode root = this;
      while (root.Parent != null)
        root = root.Parent;
      return root;
    }
  }
}
