// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.ToSAP.IMXML.IMXMLFormatter
// Assembly: Intermech.XmlExchange.IpsXml.Converter.ToSAP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 946972C6-4ABC-4C4A-94A5-3ADC51FD9A58
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.ToSAP.dll

using System.Xml.Linq;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.ToSAP.IMXML;

internal class IMXMLFormatter
{
  private long _IDCounter;
  private XDocument _resultDoc;
  private XElement _mainElement;

  public IMXMLFormatter(string exportUserName) => this.initializeDocument(exportUserName);

  public XDocument ResultDocument => this._resultDoc;

  public XElement CreateObjectElement(IMXMLFormat.NodeType objType)
  {
    return this.CreateElement(this._mainElement, objType, new IMXMLFormat.Attr[1]
    {
      IMXMLFormat.Attr.atId
    }, new string[1]{ this.GetNextID().ToString() });
  }

  public XElement CreateObjectParamsForm(XElement obj)
  {
    return this.CreateObjectElement(IMXMLFormat.NodeType.ntForm);
  }

  public XElement AddParamToParamsForm(
    XElement form,
    string paramName,
    string paramValue,
    IMXMLFormat.ParmType paramType = IMXMLFormat.ParmType.ptTechcard)
  {
    return this.CreateElement(form, IMXMLFormat.NodeType.ntFormAttribute, new IMXMLFormat.Attr[4]
    {
      IMXMLFormat.Attr.atId,
      IMXMLFormat.Attr.atName,
      IMXMLFormat.Attr.atValue,
      IMXMLFormat.Attr.atParmType
    }, new string[4]
    {
      this.GetNextID().ToString(),
      paramName,
      paramValue,
      paramType.ToIMXMLTag()
    });
  }

  public XElement CreateOccurrenceElement(XElement parentElem, XElement childElem)
  {
    return this.CreateLinkElement(parentElem, childElem, false);
  }

  public XElement CreateRelationElement(XElement parentElem, XElement childElem)
  {
    return this.CreateLinkElement(parentElem, childElem, true);
  }

  private XElement CreateLinkElement(XElement parentElem, XElement childElem, bool isRelation)
  {
    IMXMLFormat.NodeType elemType = isRelation ? IMXMLFormat.NodeType.ntRelation : IMXMLFormat.NodeType.ntOccurrence;
    return this.CreateElement(parentElem, elemType, new IMXMLFormat.Attr[3]
    {
      IMXMLFormat.Attr.atId,
      IMXMLFormat.Attr.atReference,
      IMXMLFormat.Attr.atElementType
    }, new string[3]
    {
      this.GetNextID().ToString(),
      childElem.Attribute((XName) IMXMLFormat.Attr.atId.ToIMXMLTag()).Value,
      childElem.Name.ToString()
    });
  }

  private void initializeDocument(string userName)
  {
    this._mainElement = this.CreateElement((XElement) null, IMXMLFormat.NodeType.ntIntermech, new IMXMLFormat.Attr[1]
    {
      IMXMLFormat.Attr.atExportUser
    }, new string[1]{ userName });
    this._resultDoc = new XDocument(new object[1]
    {
      (object) this._mainElement
    });
  }

  private XElement CreateElement(
    XElement parentElem,
    IMXMLFormat.NodeType elemType,
    IMXMLFormat.Attr[] attrs = null,
    string[] attrsValues = null)
  {
    XElement content = new XElement((XName) elemType.ToIMXMLTag());
    parentElem?.Add((object) content);
    if (attrs != null)
    {
      for (int index = 0; index < attrs.Length; ++index)
        content.Add((object) new XAttribute((XName) attrs[index].ToIMXMLTag(), (object) attrsValues[index]));
    }
    return content;
  }

  private long GetNextID()
  {
    ++this._IDCounter;
    return this._IDCounter;
  }
}
