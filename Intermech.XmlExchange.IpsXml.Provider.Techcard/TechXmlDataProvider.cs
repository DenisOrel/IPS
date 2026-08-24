// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Provider.Techcard.TechXmlDataProvider
// Assembly: Intermech.XmlExchange.IpsXml.Provider.Techcard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 6433FBE8-382D-4C90-9782-A3F865DC9A28
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Provider.Techcard.dll

using Intermech.XmlExchange.IpsXml.Interfaces;
using Intermech.XmlExchange.IpsXml.Interfaces.Provider;
using Intermech.XmlExchange.IpsXml.Provider.Techcard.Format;
using Intermech.XmlExchange.IpsXml.Provider.Techcard.Objects;
using Intermech.XmlExchange.IpsXml.Provider.Techcard.Params;
using Intermech.XmlExchange.IpsXml.Provider.Techcard.Relations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Provider.Techcard;

public class TechXmlDataProvider : BaseXmlDataProvider, IXmlDataProvider
{
  private IDictionary<string, TechXmlObject> _allObjects = (IDictionary<string, TechXmlObject>) new Dictionary<string, TechXmlObject>();
  private IDictionary<string, TechXmlRelation> _AllRelations = (IDictionary<string, TechXmlRelation>) new Dictionary<string, TechXmlRelation>();
  private List<TechXmlObject> _headObjects = new List<TechXmlObject>();
  private IDictionary<string, ISet<string>> _objsChildRelations = (IDictionary<string, ISet<string>>) new Dictionary<string, ISet<string>>();
  private IDictionary<string, ISet<string>> _objsParentRelations = (IDictionary<string, ISet<string>>) new Dictionary<string, ISet<string>>();
  private IDictionary<string, string> _refsToOwners = (IDictionary<string, string>) new Dictionary<string, string>();

  public IEnumerable<IXmlObject> Load(string fileName)
  {
    this.ClearCaches();
    using (XmlReader xml = XmlReader.Create(fileName))
    {
      while (xml.Read())
      {
        if (xml.NodeType == XmlNodeType.Element)
        {
          NodeType nodeType = xml.Name.ParseNodeType();
          if (nodeType.IsTechObj())
            this.LoadTechObj(xml);
          else if (nodeType == NodeType.Form)
            this.LoadObjParams(xml);
        }
      }
    }
    this._refsToOwners.Clear();
    return (IEnumerable<IXmlObject>) this.RootObjects;
  }

  public override IReadOnlyCollection<IXmlObject> RootObjects
  {
    get => (IReadOnlyCollection<IXmlObject>) this._headObjects;
  }

  public override IReadOnlyCollection<IXmlObject> GetAllObjects()
  {
    return (IReadOnlyCollection<IXmlObject>) this._allObjects.Values.ToList<TechXmlObject>();
  }

  public override IXmlObject GetRelParentObj(IXmlRelation rel)
  {
    TechXmlObject techXmlObject;
    return this._allObjects.TryGetValue((rel as TechXmlRelation).ParentObjId, out techXmlObject) ? (IXmlObject) techXmlObject : (IXmlObject) null;
  }

  public override IXmlObject GetRelChildObj(IXmlRelation rel)
  {
    TechXmlObject techXmlObject;
    return this._allObjects.TryGetValue((rel as TechXmlRelation).ChildObjId, out techXmlObject) ? (IXmlObject) techXmlObject : (IXmlObject) null;
  }

  public override IReadOnlyCollection<IXmlRelation> GetObjChildRelations(IXmlObject obj)
  {
    ISet<string> source;
    return this._objsChildRelations.TryGetValue((obj as TechXmlObject).Id, out source) ? (IReadOnlyCollection<IXmlRelation>) source.Where<string>((Func<string, bool>) (relID => this._AllRelations.ContainsKey(relID))).Select<string, TechXmlRelation>((Func<string, TechXmlRelation>) (relID => this._AllRelations[relID])).ToList<TechXmlRelation>() : (IReadOnlyCollection<IXmlRelation>) null;
  }

  public override IReadOnlyCollection<IXmlRelation> GetObjParentRelations(IXmlObject obj)
  {
    ISet<string> source;
    return this._objsParentRelations.TryGetValue((obj as TechXmlObject).Id, out source) ? (IReadOnlyCollection<IXmlRelation>) source.Where<string>((Func<string, bool>) (relID => this._AllRelations.ContainsKey(relID))).Select<string, TechXmlRelation>((Func<string, TechXmlRelation>) (relID => this._AllRelations[relID])).ToList<TechXmlRelation>() : (IReadOnlyCollection<IXmlRelation>) null;
  }

  private void ClearCaches()
  {
    this._allObjects.Clear();
    this._headObjects.Clear();
    this._refsToOwners.Clear();
    this._objsChildRelations.Clear();
    this._objsParentRelations.Clear();
  }

  private void LoadTechObj(XmlReader xml)
  {
    string key = xml[AttrType.Id.ToTechXMLTag()];
    if (string.IsNullOrEmpty(key))
      return;
    TechXmlObject techXmlObject1 = new TechXmlObject();
    techXmlObject1.Id = key;
    techXmlObject1.NodeType = xml.Name.ParseNodeType();
    techXmlObject1.IsHead = xml[AttrType.IsHead.ToTechXMLTag()] == "1";
    TechXmlObject techXmlObject2 = techXmlObject1;
    if (xml.IsEmptyElement)
      return;
    Stack<string> stringStack = new Stack<string>();
    stringStack.Push(key);
    HashSet<string> stringSet1 = new HashSet<string>();
    while (xml.Read())
    {
      if (xml.NodeType == XmlNodeType.Element)
      {
        string str = xml[AttrType.Id.ToTechXMLTag()];
        if (!string.IsNullOrEmpty(str))
        {
          NodeType nodeType = xml.Name.ParseNodeType();
          if (nodeType == NodeType.Relation && xml[AttrType.ElementType.ToTechXMLTag()] == NodeType.Form.ToTechXMLTag())
          {
            string refId = this.GetRefID(xml[AttrType.Reference.ToTechXMLTag()], xml[AttrType.ElementType.ToTechXMLTag()]);
            if (!string.IsNullOrEmpty(refId))
              this._refsToOwners.Add(refId, stringStack.Peek());
          }
          else if (nodeType == NodeType.Relation || nodeType == NodeType.Occurrence)
          {
            TechXmlRelation techXmlRelation1 = new TechXmlRelation();
            techXmlRelation1.Id = str;
            techXmlRelation1.NodeType = nodeType;
            techXmlRelation1.ParentObjId = stringStack.Peek();
            techXmlRelation1.ChildObjId = xml[AttrType.Reference.ToTechXMLTag()];
            TechXmlRelation techXmlRelation2 = techXmlRelation1;
            this._AllRelations.Add(techXmlRelation2.Id, techXmlRelation2);
            stringSet1.Add(str);
            ISet<string> stringSet2;
            if (!this._objsParentRelations.TryGetValue(techXmlRelation2.ChildObjId, out stringSet2))
            {
              stringSet2 = (ISet<string>) new HashSet<string>();
              this._objsParentRelations.Add(techXmlRelation2.ChildObjId, stringSet2);
            }
            stringSet2.Add(techXmlRelation2.Id);
          }
          if (!xml.IsEmptyElement)
            stringStack.Push(str);
        }
      }
      else if (xml.NodeType == XmlNodeType.EndElement)
      {
        if (!(stringStack.Peek() == key))
          stringStack.Pop();
        else
          break;
      }
    }
    this._allObjects.Add(techXmlObject2.Id, techXmlObject2);
    if (techXmlObject2.IsHead)
      this._headObjects.Add(techXmlObject2);
    if (stringSet1.Count <= 0)
      return;
    this._objsChildRelations.Add(key, (ISet<string>) stringSet1);
  }

  private string GetRefID(string objId, string objType) => $"{objId}_{objType}";

  private void LoadObjParams(XmlReader xml)
  {
    string objId = xml[AttrType.Id.ToTechXMLTag()];
    if (string.IsNullOrEmpty(objId))
      return;
    int depth = xml.Depth;
    TechXmlParams techXmlParams = new TechXmlParams();
    while (xml.Read() && (xml.NodeType != XmlNodeType.EndElement || depth != xml.Depth))
    {
      if (xml.NodeType == XmlNodeType.Element && xml.Name.ParseNodeType() == NodeType.FormAttribute)
      {
        string name = xml[AttrType.Name.ToTechXMLTag()];
        string str1 = xml[AttrType.Value.ToTechXMLTag()];
        ParamType parmType = xml[AttrType.ParmType.ToTechXMLTag()].ParseParmType();
        string str2 = str1;
        int num = (int) parmType;
        TechXmlParam techXmlParam = new TechXmlParam(name, str2, (ParamType) num);
        techXmlParams.AddParam((IXmlParam) techXmlParam);
      }
    }
    string key;
    if (!this._refsToOwners.TryGetValue(this.GetRefID(objId, NodeType.Form.ToTechXMLTag()), out key))
      return;
    TechXmlObject techXmlObject;
    if (this._allObjects.TryGetValue(key, out techXmlObject))
    {
      techXmlObject.SetParams((IXmlParams) techXmlParams);
    }
    else
    {
      TechXmlRelation techXmlRelation;
      if (!this._AllRelations.TryGetValue(key, out techXmlRelation))
        return;
      techXmlRelation.SetParams((IXmlParams) techXmlParams);
    }
  }
}
