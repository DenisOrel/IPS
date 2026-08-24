// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.ToSAP.IPSToSAPService
// Assembly: Intermech.XmlExchange.IpsXml.Converter.ToSAP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 946972C6-4ABC-4C4A-94A5-3ADC51FD9A58
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.ToSAP.dll

using Intermech.Interfaces;
using Intermech.IpsXmlViewer.Interfaces;
using Intermech.XmlExchange.IpsXml.Converter.ToSAP.IMXML;
using Intermech.XmlExchange.IpsXml.Interfaces;
using Intermech.XmlExchange.IpsXml.Provider.Ips;
using Intermech.XmlExchange.IpsXml.Provider.Ips.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.ToSAP;

public class IPSToSAPService
{
  private IUserSession _session;
  private IpsXmlDataProvider _parser;

  public IPSToSAPService(IUserSession session) => this._session = session;

  public IEnumerable<string> Convert(string[] fileNames)
  {
    this._parser = new IpsXmlDataFactory().GetDataProvider(fileNames) as IpsXmlDataProvider;
    IEnumerable<IImObject> imObjects = this._parser.Load(fileNames).Where<IXmlObject>((Func<IXmlObject, bool>) (headObj => headObj is IImObject targetObj1 && targetObj1.IsArticle())).Select<IXmlObject, IImObject>((Func<IXmlObject, IImObject>) (headObj => headObj as IImObject));
    List<string> stringList = new List<string>();
    foreach (IImObject startFromObj in imObjects)
    {
      string filename = Path.Combine(Path.GetDirectoryName(fileNames[0]), startFromObj.GetAsString("F_OBJECT_ID", "NULL_OBJ_ID")) + ".xml";
      stringList.Add(filename);
      using (XmlTextWriter writer = new XmlTextWriter(filename, Encoding.GetEncoding(1251)))
      {
        IMXMLFormatter docFormatter = new IMXMLFormatter(this._session.UserName);
        XElement elArt = this.SerializeObjectToIMXML(startFromObj, docFormatter);
        this._parser.Traverse(startFromObj as IXmlObject, (OnFilterObject) ((IXmlObject parentObj, IXmlObject childObj, IXmlRelation relation, ref bool traverse) => childObj is IImObject targetObj2 && targetObj2.IsProcessRoute()), (OnVisitObject) ((IXmlObject parentObj, IXmlObject childObj, IXmlRelation relation, ref bool traverse) => this.SerializeArtProcessRoute(childObj as IImObject, elArt, docFormatter)), false);
        docFormatter.ResultDocument.Save((XmlWriter) writer);
      }
    }
    return stringList.Count == 0 ? (IEnumerable<string>) null : (IEnumerable<string>) stringList.ToArray();
  }

  private void SerializeArtProcessRoute(
    IImObject processRoute,
    XElement elArt,
    IMXMLFormatter docFormatter)
  {
    XElement objectElement1 = docFormatter.CreateObjectElement(IMXMLFormat.NodeType.ntMBOM);
    XElement objectElement2 = docFormatter.CreateObjectElement(IMXMLFormat.NodeType.ntPROC);
    docFormatter.CreateOccurrenceElement(elArt, objectElement1);
    docFormatter.CreateOccurrenceElement(elArt, objectElement2);
    IImObject route = (IImObject) null;
    IImObject tp = (IImObject) null;
    IList<IImObject> workpieces = (IList<IImObject>) new List<IImObject>();
    IList<IImObject> workshopEnters = (IList<IImObject>) new List<IImObject>();
    this._parser.Traverse(processRoute as IXmlObject, (OnFilterObject) ((IXmlObject parentObj, IXmlObject childObject, IXmlRelation relation, ref bool traverse) =>
    {
      if (!(childObject is IImObject targetObj2))
        return false;
      if (targetObj2.IsRoute() && route == null)
        route = targetObj2;
      else if (targetObj2.IsTechProccess() && tp == null)
        tp = targetObj2;
      else if (targetObj2.IsWorkpiece())
        workpieces.Add(targetObj2);
      return false;
    }), (OnVisitObject) null, false);
    if (tp != null)
      this._parser.Traverse(tp as IXmlObject, (OnFilterObject) ((IXmlObject parentObj, IXmlObject childObject, IXmlRelation relation, ref bool traverse) =>
      {
        if (childObject is IImObject targetObj4 && targetObj4.IsWorkShopEnter())
          workshopEnters.Add(targetObj4);
        return false;
      }), (OnVisitObject) null, false);
    XElement xelement = (XElement) null;
    if (route != null)
      xelement = this.SerializeRoute(route, docFormatter);
    else if (tp != null)
      xelement = docFormatter.CreateObjectElement(IMXMLFormat.NodeType.ntRoute);
    if (xelement != null)
      docFormatter.CreateOccurrenceElement(objectElement1, xelement);
    if (tp != null)
    {
      XElement childElem = this.SerializeTP(tp, docFormatter);
      docFormatter.CreateOccurrenceElement(xelement, childElem);
    }
    if (workshopEnters.Count <= 0)
      return;
    XElement parentElem1 = xelement == null ? objectElement1 : xelement;
    XElement parentElem2 = (XElement) null;
    for (int index = workshopEnters.Count - 1; index >= 0; --index)
    {
      XElement childElem = this.SerializeWorkshopEnter(workshopEnters[index], docFormatter);
      docFormatter.CreateOccurrenceElement(parentElem1, childElem);
      parentElem1 = childElem;
      docFormatter.CreateOccurrenceElement(objectElement2, childElem);
      parentElem2 = childElem;
    }
    foreach (IImObject workpiece in (IEnumerable<IImObject>) workpieces)
    {
      XElement childElem = this.SerializeWorkpiece(workpiece, docFormatter);
      docFormatter.CreateOccurrenceElement(parentElem2, childElem);
    }
  }

  private XElement SerializeRoute(IImObject route, IMXMLFormatter docFormatter)
  {
    return this.SerializeObjectToIMXML(route, docFormatter);
  }

  private XElement SerializeTP(IImObject tp, IMXMLFormatter docFormatter)
  {
    return this.SerializeObjectToIMXML(tp, docFormatter);
  }

  private XElement SerializeWorkshopEnter(IImObject workshopEnter, IMXMLFormatter docFormatter)
  {
    XElement elWorkshopEnter = (XElement) null;
    int localOperNumber = 1;
    this._parser.Traverse<XElement>(workshopEnter as IXmlObject, (OnFilterObject<XElement>) ((IXmlObject parentObj, IXmlObject childObject, IXmlRelation relation, ref bool traverse, ref XElement parentElem) =>
    {
      if (!(childObject is IImObject targetObj2))
        return false;
      return targetObj2.IsWorkShopEnter() || targetObj2.IsMaterial() || targetObj2.IsOper();
    }), (OnVisitObject<XElement>) ((IXmlObject parentObj, IXmlObject childObject, IXmlRelation relation, ref bool traverse, ref XElement parentElem) =>
    {
      XElement childElem = (XElement) null;
      if (!(childObject is IImObject imObject2))
        return;
      if (imObject2.IsWorkShopEnter())
      {
        childElem = this.SerializeObjectToIMXML(imObject2, docFormatter);
        elWorkshopEnter = childElem;
      }
      else if (imObject2.IsMaterial())
        childElem = this.SerializeMat(imObject2, docFormatter);
      else if (imObject2.IsOper())
      {
        childElem = this.SerializeOper(imObject2, localOperNumber, docFormatter);
        ++localOperNumber;
      }
      if (parentElem != null && childElem != null)
        docFormatter.CreateOccurrenceElement(parentElem, childElem);
      if (childElem == null || !imObject2.IsWorkShopEnter() && !imObject2.IsOper())
        return;
      parentElem = childElem;
    }), true, (XElement) null);
    return elWorkshopEnter;
  }

  private XElement SerializeOper(IImObject oper, int localOperNumber, IMXMLFormatter docFormatter)
  {
    XElement elObj;
    XElement elObjParamsForm;
    this.SerializeObjectToIMXML(oper, docFormatter, out elObj, out elObjParamsForm);
    if (elObjParamsForm != null)
      docFormatter.AddParamToParamsForm(elObjParamsForm, IMXMLFormat.FixedParam.fpOperNumberInWorkshop.ToIMXMLTag(), localOperNumber.ToString().PadLeft(3, '0'));
    return elObj;
  }

  private XElement SerializeMat(IImObject mat, IMXMLFormatter docFormatter)
  {
    return this.SerializeObjectToIMXML(mat, docFormatter);
  }

  private XElement SerializeWorkpiece(IImObject workpiece, IMXMLFormatter docFormatter)
  {
    return this.SerializeObjectToIMXML(workpiece, docFormatter);
  }

  private XElement SerializeObjectToIMXML(IImObject obj, IMXMLFormatter docFormatter)
  {
    XElement elObj;
    this.SerializeObjectToIMXML(obj, docFormatter, out elObj, out XElement _);
    return elObj;
  }

  private void SerializeObjectToIMXML(
    IImObject obj,
    IMXMLFormatter docFormatter,
    out XElement elObj,
    out XElement elObjParamsForm)
  {
    elObj = docFormatter.CreateObjectElement(obj.ToIMXMLNodeType());
    if (obj.Attributes.Count > 0)
    {
      elObjParamsForm = docFormatter.CreateObjectParamsForm(elObj);
      foreach (KeyValuePair<string, object> attribute1 in (IEnumerable<KeyValuePair<string, object>>) obj.Attributes)
      {
        string paramName;
        string asString;
        if (attribute1.Value is IImAttribute attribute2)
        {
          IImAttributeType attrType = this._parser.GetAttrType(attribute2);
          paramName = attrType != null ? attrType.F_NAME : attribute2.F_ATTRIBUTE_ID;
          asString = attribute2.GetAsString("F_VALUE", "");
        }
        else
        {
          paramName = attribute1.Key;
          asString = obj.GetAsString(attribute1.Key, "");
        }
        docFormatter.AddParamToParamsForm(elObjParamsForm, paramName, asString);
      }
      docFormatter.CreateRelationElement(elObj, elObjParamsForm);
    }
    else
      elObjParamsForm = (XElement) null;
  }
}
