// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.CompoitionSettings
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Kernel.Search;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

[Serializable]
public class CompoitionSettings : ICompoitionSettings
{
  public List<Tuple<int, int, List<int>>> ChildTypes { get; private set; }

  public virtual List<int> GetChildTypes(int parentTypeID, int relationTypeID)
  {
    if (this.ChildTypes == null)
      return new List<int>(0);
    Tuple<int, int, List<int>> tuple = this.ChildTypes.Find((Predicate<Tuple<int, int, List<int>>>) (x => x.Item1 == parentTypeID && x.Item2 == relationTypeID));
    return tuple != null ? MetaDataHelper.GetObjectTypeChildrenIDRecursive((IEnumerable<int>) tuple.Item3) : new List<int>(0);
  }

  public List<Tuple<int, List<int>>> ObjectCompareAttributes { get; private set; }

  public virtual List<int> GetObjectCompareAttributes(int objectTypeID)
  {
    return this.ObjectCompareAttributes == null ? new List<int>(0) : this.GetValuesFromCollection<int>(this.ObjectCompareAttributes, objectTypeID);
  }

  public List<Tuple<int, List<int>>> RelationCompareAttributes { get; private set; }

  public virtual List<int> GetRelationCompareAttributes(int relationTypeID)
  {
    return this.RelationCompareAttributes == null ? new List<int>(0) : this.GetValuesFromCollection<int>(this.RelationCompareAttributes, relationTypeID, false);
  }

  public List<Tuple<int, List<int>>> ObjectIDAttributes { get; private set; }

  public virtual List<int> GetIDObjectAttributes(int objectTypeID)
  {
    return this.ObjectIDAttributes == null ? new List<int>(0) : this.GetValuesFromCollection<int>(this.ObjectIDAttributes, objectTypeID);
  }

  public List<Tuple<int, int, List<int>>> RelationIDAttributes { get; private set; }

  public virtual List<int> GetIDRelationAttributes(int parentTypeID, int relationTypeID)
  {
    if (this.RelationIDAttributes == null)
      return new List<int>(0);
    Tuple<int, int, List<int>> tuple = this.RelationIDAttributes.Find((Predicate<Tuple<int, int, List<int>>>) (x => x.Item1 == parentTypeID && x.Item2 == relationTypeID));
    return tuple != null ? tuple.Item3 : new List<int>(0);
  }

  public List<Tuple<int, List<Tuple<int, AttributeSourceTypes>>>> SortedAttributes { get; private set; }

  public virtual List<Tuple<int, AttributeSourceTypes>> GetSortedAttributes(int parentTypeID)
  {
    return this.SortedAttributes == null ? new List<Tuple<int, AttributeSourceTypes>>(0) : this.GetValuesFromCollection<Tuple<int, AttributeSourceTypes>>(this.SortedAttributes, parentTypeID);
  }

  public virtual List<int> GetRelationTypes(int objectTypeID)
  {
    if (this.ChildTypes == null)
      return new List<int>(0);
    List<int> relationTypes = new List<int>();
    foreach (Tuple<int, int, List<int>> tuple in this.ChildTypes.FindAll((Predicate<Tuple<int, int, List<int>>>) (x => x.Item1 == objectTypeID)))
    {
      if (tuple.Item3.Count > 0)
        relationTypes.Add(tuple.Item2);
    }
    return relationTypes;
  }

  public bool CheckExistsAttributes { get; set; }

  public void AddApplicability(Tuple<int, int, List<int>> item)
  {
    this.ChildTypes.Add(item);
    if (!this.SortedAttributes.Exists((Predicate<Tuple<int, List<Tuple<int, AttributeSourceTypes>>>>) (x => x.Item1 == item.Item1)))
      this.SortedAttributes.Add(new Tuple<int, List<Tuple<int, AttributeSourceTypes>>>(item.Item1, new List<Tuple<int, AttributeSourceTypes>>()));
    this.AddNewObjectType(item.Item1);
    this.AddNewApplicability(item.Item1, item.Item2);
  }

  public void RemoveRootType(int typeID)
  {
    this.ChildTypes.RemoveAll((Predicate<Tuple<int, int, List<int>>>) (x => x.Item1 == typeID));
    this.RelationIDAttributes.RemoveAll((Predicate<Tuple<int, int, List<int>>>) (x => x.Item1 == typeID));
    if (this.ChildTypes.Find((Predicate<Tuple<int, int, List<int>>>) (x => x.Item3.Contains(typeID))) != null)
      return;
    this.ObjectIDAttributes.RemoveAll((Predicate<Tuple<int, List<int>>>) (x => x.Item1 == typeID));
    this.ObjectCompareAttributes.RemoveAll((Predicate<Tuple<int, List<int>>>) (x => x.Item1 == typeID));
    this.SortedAttributes.RemoveAll((Predicate<Tuple<int, List<Tuple<int, AttributeSourceTypes>>>>) (x => x.Item1 == typeID));
  }

  public static CompoitionSettings CreateNew()
  {
    return new CompoitionSettings()
    {
      ChildTypes = new List<Tuple<int, int, List<int>>>(),
      ObjectCompareAttributes = new List<Tuple<int, List<int>>>(),
      ObjectIDAttributes = new List<Tuple<int, List<int>>>(),
      RelationCompareAttributes = new List<Tuple<int, List<int>>>(),
      RelationIDAttributes = new List<Tuple<int, int, List<int>>>(),
      SortedAttributes = new List<Tuple<int, List<Tuple<int, AttributeSourceTypes>>>>(),
      CheckExistsAttributes = false
    };
  }

  public void AddChildType(int parentTypeID, int relationTypeID, int childTypeID)
  {
    Tuple<int, int, List<int>> tuple = this.ChildTypes.Find((Predicate<Tuple<int, int, List<int>>>) (x => x.Item1 == parentTypeID && x.Item2 == relationTypeID));
    if (tuple == null)
    {
      tuple = new Tuple<int, int, List<int>>(parentTypeID, relationTypeID, new List<int>());
      this.AddApplicability(tuple);
    }
    if (!tuple.Item3.Contains(childTypeID))
      tuple.Item3.Add(childTypeID);
    this.AddNewObjectType(childTypeID);
  }

  public void RemoveChildType(int parentTypeID, int relationTypeID, int childTypeID)
  {
    Tuple<int, int, List<int>> tuple = this.ChildTypes.Find((Predicate<Tuple<int, int, List<int>>>) (x => x.Item1 == parentTypeID && x.Item2 == relationTypeID));
    if (tuple == null)
      return;
    tuple.Item3.Remove(childTypeID);
    if (this.ChildTypes.Find((Predicate<Tuple<int, int, List<int>>>) (x => x.Item1 == childTypeID || x.Item3.Contains(childTypeID))) != null)
      return;
    this.ObjectIDAttributes.RemoveAll((Predicate<Tuple<int, List<int>>>) (x => x.Item1 == childTypeID));
    this.ObjectCompareAttributes.RemoveAll((Predicate<Tuple<int, List<int>>>) (x => x.Item1 == childTypeID));
  }

  public ICompoitionSettings Clone()
  {
    return (ICompoitionSettings) new CompoitionSettings()
    {
      ChildTypes = this.CloneListType2(this.ChildTypes),
      ObjectIDAttributes = this.CloneListType1(this.ObjectIDAttributes),
      RelationIDAttributes = this.CloneListType2(this.RelationIDAttributes),
      ObjectCompareAttributes = this.CloneListType1(this.ObjectCompareAttributes),
      RelationCompareAttributes = this.CloneListType1(this.RelationCompareAttributes),
      SortedAttributes = this.CloneListType4(this.SortedAttributes),
      CheckExistsAttributes = this.CheckExistsAttributes
    };
  }

  public bool AddSortedAttribute(
    int parentTypeID,
    int attributeID,
    AttributeSourceTypes sourceType)
  {
    Tuple<int, List<Tuple<int, AttributeSourceTypes>>> tuple = this.SortedAttributes.Find((Predicate<Tuple<int, List<Tuple<int, AttributeSourceTypes>>>>) (x => x.Item1 == parentTypeID));
    if (tuple.Item2.Exists((Predicate<Tuple<int, AttributeSourceTypes>>) (x => x.Item1 == attributeID && x.Item2 == sourceType)))
      return false;
    tuple.Item2.Add(new Tuple<int, AttributeSourceTypes>(attributeID, sourceType));
    return true;
  }

  public void RemoveSortedAttribute(
    int parentTypeID,
    int attributeID,
    AttributeSourceTypes sourceType)
  {
    Tuple<int, List<Tuple<int, AttributeSourceTypes>>> tuple = this.SortedAttributes.Find((Predicate<Tuple<int, List<Tuple<int, AttributeSourceTypes>>>>) (x => x.Item1 == parentTypeID));
    tuple.Item2.Remove(tuple.Item2.Find((Predicate<Tuple<int, AttributeSourceTypes>>) (x => x.Item1 == attributeID && x.Item2 == sourceType)));
  }

  public bool AddRelationIDAttribute(int parentTypeID, int relationTypeID, int attributeID)
  {
    Tuple<int, int, List<int>> tuple = this.RelationIDAttributes.Find((Predicate<Tuple<int, int, List<int>>>) (x => x.Item1 == parentTypeID && x.Item2 == relationTypeID));
    if (tuple.Item3.Contains(attributeID))
      return false;
    tuple.Item3.Add(attributeID);
    return true;
  }

  public void RemoveRelationIDAttribute(int parentTypeID, int relationTypeID, int attributeID)
  {
    Tuple<int, int, List<int>> tuple = this.RelationIDAttributes.Find((Predicate<Tuple<int, int, List<int>>>) (x => x.Item1 == parentTypeID && x.Item2 == relationTypeID));
    if (!tuple.Item3.Contains(attributeID))
      return;
    tuple.Item3.Remove(attributeID);
  }

  public bool AddObjectIDAttribute(int objectTypeID, int attributeID)
  {
    Tuple<int, List<int>> tuple = this.ObjectIDAttributes.Find((Predicate<Tuple<int, List<int>>>) (x => x.Item1 == objectTypeID));
    if (tuple.Item2.Contains(attributeID))
      return false;
    tuple.Item2.Add(attributeID);
    return true;
  }

  public void RemoveObjectIDAttribute(int objectTypeID, int attributeID)
  {
    Tuple<int, List<int>> tuple = this.ObjectIDAttributes.Find((Predicate<Tuple<int, List<int>>>) (x => x.Item1 == objectTypeID));
    if (!tuple.Item2.Contains(attributeID))
      return;
    tuple.Item2.Remove(attributeID);
  }

  public void AddRelationCompareAttribute(int relationTypeID, int attributeID)
  {
    this.AddCompareAttribute(this.RelationCompareAttributes, relationTypeID, attributeID);
  }

  public void AddObjectCompareAttribute(int objectTypeID, int attributeID)
  {
    this.AddCompareAttribute(this.ObjectCompareAttributes, objectTypeID, attributeID);
  }

  public void RemoveRelationCompareAttribute(int relationTypeID, int attributeID)
  {
    this.RemoveCompareAttribute(this.RelationCompareAttributes, relationTypeID, attributeID);
  }

  public void RemoveObjectCompareAttribute(int objectTypeID, int attributeID)
  {
    this.RemoveCompareAttribute(this.ObjectCompareAttributes, objectTypeID, attributeID);
  }

  protected void RemoveCompareAttribute(
    List<Tuple<int, List<int>>> collection,
    int id,
    int attributeID)
  {
    Tuple<int, List<int>> tuple = collection.Find((Predicate<Tuple<int, List<int>>>) (x => x.Item1 == id));
    if (tuple == null || !tuple.Item2.Contains(attributeID))
      return;
    tuple.Item2.Remove(attributeID);
  }

  protected void AddCompareAttribute(
    List<Tuple<int, List<int>>> collection,
    int id,
    int attributeID)
  {
    Tuple<int, List<int>> tuple = collection.Find((Predicate<Tuple<int, List<int>>>) (x => x.Item1 == id)) ?? new Tuple<int, List<int>>(id, new List<int>());
    if (tuple.Item2.Contains(attributeID))
      return;
    tuple.Item2.Add(attributeID);
  }

  protected void AddNewApplicability(int parentTypeID, int relationTypeID)
  {
    if (!this.RelationIDAttributes.Exists((Predicate<Tuple<int, int, List<int>>>) (x => x.Item1 == parentTypeID && x.Item2 == relationTypeID)))
      this.RelationIDAttributes.Add(new Tuple<int, int, List<int>>(parentTypeID, relationTypeID, new List<int>()));
    if (this.RelationCompareAttributes.Exists((Predicate<Tuple<int, List<int>>>) (x => x.Item1 == relationTypeID)))
      return;
    this.RelationCompareAttributes.Add(new Tuple<int, List<int>>(relationTypeID, new List<int>()));
  }

  protected void AddNewObjectType(int typeID)
  {
    if (!this.ObjectIDAttributes.Exists((Predicate<Tuple<int, List<int>>>) (x => x.Item1 == typeID)))
      this.ObjectIDAttributes.Add(new Tuple<int, List<int>>(typeID, new List<int>()));
    if (this.ObjectCompareAttributes.Exists((Predicate<Tuple<int, List<int>>>) (x => x.Item1 == typeID)))
      return;
    this.ObjectCompareAttributes.Add(new Tuple<int, List<int>>(typeID, new List<int>()));
  }

  protected List<Tuple<int, List<Tuple<int, AttributeSourceTypes>>>> CloneListType4(
    List<Tuple<int, List<Tuple<int, AttributeSourceTypes>>>> inList)
  {
    if (inList == null)
      return (List<Tuple<int, List<Tuple<int, AttributeSourceTypes>>>>) null;
    List<Tuple<int, List<Tuple<int, AttributeSourceTypes>>>> tupleList = new List<Tuple<int, List<Tuple<int, AttributeSourceTypes>>>>();
    foreach (Tuple<int, List<Tuple<int, AttributeSourceTypes>>> tuple in inList)
      tupleList.Add(new Tuple<int, List<Tuple<int, AttributeSourceTypes>>>(tuple.Item1, this.CloneListType3(tuple.Item2)));
    return tupleList;
  }

  protected List<Tuple<int, int, List<int>>> CloneListType2(List<Tuple<int, int, List<int>>> inList)
  {
    if (inList == null)
      return (List<Tuple<int, int, List<int>>>) null;
    List<Tuple<int, int, List<int>>> tupleList = new List<Tuple<int, int, List<int>>>();
    foreach (Tuple<int, int, List<int>> tuple in inList)
      tupleList.Add(new Tuple<int, int, List<int>>(tuple.Item1, tuple.Item2, this.CloneList(tuple.Item3)));
    return tupleList;
  }

  protected List<Tuple<int, List<int>>> CloneListType1(List<Tuple<int, List<int>>> inList)
  {
    if (inList == null)
      return (List<Tuple<int, List<int>>>) null;
    List<Tuple<int, List<int>>> tupleList = new List<Tuple<int, List<int>>>();
    foreach (Tuple<int, List<int>> tuple in inList)
      tupleList.Add(new Tuple<int, List<int>>(tuple.Item1, this.CloneList(tuple.Item2)));
    return tupleList;
  }

  protected List<Tuple<int, AttributeSourceTypes>> CloneListType3(
    List<Tuple<int, AttributeSourceTypes>> inList)
  {
    if (inList == null)
      return (List<Tuple<int, AttributeSourceTypes>>) null;
    List<Tuple<int, AttributeSourceTypes>> tupleList = new List<Tuple<int, AttributeSourceTypes>>();
    foreach (Tuple<int, AttributeSourceTypes> tuple in inList)
      tupleList.Add(new Tuple<int, AttributeSourceTypes>(tuple.Item1, tuple.Item2));
    return tupleList;
  }

  protected List<int> CloneList(List<int> inList)
  {
    if (inList == null)
      return (List<int>) null;
    List<int> intList = new List<int>();
    intList.AddRange((IEnumerable<int>) inList.ToArray());
    return intList;
  }

  public void Load(Stream stream)
  {
    XmlDocument xmlDoc = new XmlDocument();
    xmlDoc.Load(stream);
    this.ChildTypes = XmlHelper.DecodeCollection(xmlDoc, "childTypes/childType", new XmlNodeAttribute("parentType", XmlMetadataTypes.ObjectType), new XmlNodeAttribute("relationType", XmlMetadataTypes.RelationType), XmlMetadataTypes.ObjectType);
    this.ObjectIDAttributes = XmlHelper.DecodeCollection(xmlDoc, "objectIDAttributes/objectIDAttribute", new XmlNodeAttribute("objectType", XmlMetadataTypes.ObjectType), XmlMetadataTypes.Attribute);
    this.RelationIDAttributes = XmlHelper.DecodeCollection(xmlDoc, "relationIDAttributes/relationIDAttribute", new XmlNodeAttribute("parentType", XmlMetadataTypes.ObjectType), new XmlNodeAttribute("relationType", XmlMetadataTypes.RelationType), XmlMetadataTypes.Attribute);
    this.ObjectCompareAttributes = XmlHelper.DecodeCollection(xmlDoc, "objectCompareAttributes/objectCompareAttribute", new XmlNodeAttribute("objectType", XmlMetadataTypes.ObjectType), XmlMetadataTypes.Attribute);
    this.RelationCompareAttributes = XmlHelper.DecodeCollection(xmlDoc, "relationCompareAttributes/relationCompareAttribute", new XmlNodeAttribute("relationType", XmlMetadataTypes.RelationType), XmlMetadataTypes.Attribute);
    this.SortedAttributes = XmlHelper.DecodeCollection(xmlDoc, "sortedAttributes/sortedAttribute", new XmlNodeAttribute("parentType", XmlMetadataTypes.ObjectType));
    XmlNode xmlNode = xmlDoc.DocumentElement.SelectSingleNode("checkExistsAttributes");
    if (xmlNode == null)
      return;
    this.CheckExistsAttributes = Convert.ToBoolean(xmlNode.InnerText);
  }

  public void Save(Stream stream)
  {
    XmlDocument xmlDoc = new XmlDocument();
    XmlNode element1 = (XmlNode) xmlDoc.CreateElement("loadCompoitionSettings");
    if (this.ChildTypes != null)
      element1.AppendChild(XmlHelper.EncodeCollection(xmlDoc, "childTypes", "childType", new XmlNodeAttribute("parentType", XmlMetadataTypes.ObjectType), new XmlNodeAttribute("relationType", XmlMetadataTypes.RelationType), XmlMetadataTypes.ObjectType, this.ChildTypes));
    if (this.ObjectIDAttributes != null)
      element1.AppendChild(XmlHelper.EncodeCollection(xmlDoc, "objectIDAttributes", "objectIDAttribute", new XmlNodeAttribute("objectType", XmlMetadataTypes.ObjectType), XmlMetadataTypes.Attribute, this.ObjectIDAttributes));
    if (this.RelationIDAttributes != null)
      element1.AppendChild(XmlHelper.EncodeCollection(xmlDoc, "relationIDAttributes", "relationIDAttribute", new XmlNodeAttribute("parentType", XmlMetadataTypes.ObjectType), new XmlNodeAttribute("relationType", XmlMetadataTypes.RelationType), XmlMetadataTypes.Attribute, this.RelationIDAttributes));
    if (this.ObjectCompareAttributes != null)
      element1.AppendChild(XmlHelper.EncodeCollection(xmlDoc, "objectCompareAttributes", "objectCompareAttribute", new XmlNodeAttribute("objectType", XmlMetadataTypes.ObjectType), XmlMetadataTypes.Attribute, this.ObjectCompareAttributes));
    if (this.RelationCompareAttributes != null)
      element1.AppendChild(XmlHelper.EncodeCollection(xmlDoc, "relationCompareAttributes", "relationCompareAttribute", new XmlNodeAttribute("relationType", XmlMetadataTypes.RelationType), XmlMetadataTypes.Attribute, this.RelationCompareAttributes));
    if (this.SortedAttributes != null)
      element1.AppendChild(XmlHelper.EncodeCollection(xmlDoc, "sortedAttributes", "sortedAttribute", new XmlNodeAttribute("parentType", XmlMetadataTypes.ObjectType), this.SortedAttributes));
    XmlNode element2 = (XmlNode) xmlDoc.CreateElement("checkExistsAttributes");
    element2.InnerText = this.CheckExistsAttributes.ToString();
    element1.AppendChild(element2);
    xmlDoc.AppendChild(element1);
    xmlDoc.Save(stream);
  }

  protected List<T> GetValuesFromCollection<T>(List<Tuple<int, List<T>>> collection, int item1)
  {
    return this.GetValuesFromCollection<T>(collection, item1, true);
  }

  protected List<T> GetValuesFromCollection<T>(
    List<Tuple<int, List<T>>> collection,
    int item1,
    bool inheritance)
  {
    Tuple<int, List<T>> tuple = collection.Find((Predicate<Tuple<int, List<T>>>) (x => x.Item1 == item1));
    if (tuple != null)
      return tuple.Item2;
    if (inheritance)
    {
      int objectTypeParentId = MetaDataHelper.GetObjectTypeParentID(item1);
      if (objectTypeParentId != -1)
        return this.GetValuesFromCollection<T>(collection, objectTypeParentId, true);
    }
    return new List<T>(0);
  }
}
