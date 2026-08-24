// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.XmlHelper
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Kernel.Search;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

internal static class XmlHelper
{
  public static List<Tuple<int, int, List<int>>> DecodeCollection(
    XmlDocument xmlDoc,
    string path,
    XmlNodeAttribute attribute1,
    XmlNodeAttribute attribute2,
    XmlMetadataTypes innerMetadataType)
  {
    List<Tuple<int, int, List<int>>> tupleList = new List<Tuple<int, int, List<int>>>();
    foreach (XmlNode selectNode in xmlDoc.DocumentElement.SelectNodes(path))
    {
      int num1 = XmlHelper.DecodeValue(attribute1.MetadataType, selectNode.Attributes[attribute1.Name].Value);
      if (num1 != -1)
      {
        int num2 = XmlHelper.DecodeValue(attribute2.MetadataType, selectNode.Attributes[attribute2.Name].Value);
        if (num2 != -1)
        {
          List<int> intList = XmlHelper.DecodeInnerText(selectNode.InnerText, innerMetadataType) ?? new List<int>();
          tupleList.Add(new Tuple<int, int, List<int>>(num1, num2, intList));
        }
      }
    }
    return tupleList;
  }

  public static List<Tuple<int, List<int>>> DecodeCollection(
    XmlDocument xmlDoc,
    string path,
    XmlNodeAttribute attribute1,
    XmlMetadataTypes innerMetadataType)
  {
    List<Tuple<int, List<int>>> tupleList = new List<Tuple<int, List<int>>>();
    foreach (XmlNode selectNode in xmlDoc.DocumentElement.SelectNodes(path))
    {
      int num = XmlHelper.DecodeValue(attribute1.MetadataType, selectNode.Attributes[attribute1.Name].Value);
      if (num != -1)
      {
        List<int> intList = XmlHelper.DecodeInnerText(selectNode.InnerText, innerMetadataType) ?? new List<int>();
        tupleList.Add(new Tuple<int, List<int>>(num, intList));
      }
    }
    return tupleList;
  }

  public static List<Tuple<int, List<Tuple<int, AttributeSourceTypes>>>> DecodeCollection(
    XmlDocument xmlDoc,
    string path,
    XmlNodeAttribute attribute1)
  {
    List<Tuple<int, List<Tuple<int, AttributeSourceTypes>>>> tupleList1 = new List<Tuple<int, List<Tuple<int, AttributeSourceTypes>>>>();
    foreach (XmlNode selectNode in xmlDoc.DocumentElement.SelectNodes(path))
    {
      int num = XmlHelper.DecodeValue(attribute1.MetadataType, selectNode.Attributes[attribute1.Name].Value);
      if (num != -1)
      {
        List<Tuple<int, AttributeSourceTypes>> tupleList2 = XmlHelper.DecodeInnerText(selectNode.InnerText) ?? new List<Tuple<int, AttributeSourceTypes>>();
        tupleList1.Add(new Tuple<int, List<Tuple<int, AttributeSourceTypes>>>(num, tupleList2));
      }
    }
    return tupleList1;
  }

  public static XmlNode EncodeCollection(
    XmlDocument xmlDoc,
    string rootNodeName,
    string nodeName,
    XmlNodeAttribute attribute1,
    XmlMetadataTypes innerMetadataType,
    List<Tuple<int, List<int>>> collection)
  {
    XmlNode element1 = (XmlNode) xmlDoc.CreateElement(rootNodeName);
    foreach (Tuple<int, List<int>> tuple in collection)
    {
      XmlNode element2 = (XmlNode) xmlDoc.CreateElement(nodeName);
      XmlHelper.CreateAttribute(xmlDoc, element2, attribute1.Name, XmlHelper.EncodeValue(attribute1.MetadataType, tuple.Item1));
      element2.InnerText = XmlHelper.EncodeInnerText(tuple.Item2, innerMetadataType);
      element1.AppendChild(element2);
    }
    return element1;
  }

  public static XmlNode EncodeCollection(
    XmlDocument xmlDoc,
    string rootNodeName,
    string nodeName,
    XmlNodeAttribute attribute1,
    XmlNodeAttribute attribute2,
    XmlMetadataTypes innerMetadataType,
    List<Tuple<int, int, List<int>>> collection)
  {
    XmlNode element1 = (XmlNode) xmlDoc.CreateElement(rootNodeName);
    foreach (Tuple<int, int, List<int>> tuple in collection)
    {
      XmlNode element2 = (XmlNode) xmlDoc.CreateElement(nodeName);
      XmlHelper.CreateAttribute(xmlDoc, element2, attribute1.Name, XmlHelper.EncodeValue(attribute1.MetadataType, tuple.Item1));
      XmlHelper.CreateAttribute(xmlDoc, element2, attribute2.Name, XmlHelper.EncodeValue(attribute2.MetadataType, tuple.Item2));
      element2.InnerText = XmlHelper.EncodeInnerText(tuple.Item3, innerMetadataType);
      element1.AppendChild(element2);
    }
    return element1;
  }

  public static XmlNode EncodeCollection(
    XmlDocument xmlDoc,
    string rootNodeName,
    string nodeName,
    XmlNodeAttribute attribute1,
    List<Tuple<int, List<Tuple<int, AttributeSourceTypes>>>> collection)
  {
    XmlNode element1 = (XmlNode) xmlDoc.CreateElement(rootNodeName);
    foreach (Tuple<int, List<Tuple<int, AttributeSourceTypes>>> tuple in collection)
    {
      XmlNode element2 = (XmlNode) xmlDoc.CreateElement(nodeName);
      XmlHelper.CreateAttribute(xmlDoc, element2, attribute1.Name, XmlHelper.EncodeValue(attribute1.MetadataType, tuple.Item1));
      element2.InnerText = XmlHelper.EncodeInnerText(tuple.Item2);
      element1.AppendChild(element2);
    }
    return element1;
  }

  private static List<Tuple<int, AttributeSourceTypes>> DecodeInnerText(string innerText)
  {
    string[] strArray1 = innerText.Split(';');
    if (strArray1.Length == 0)
      return (List<Tuple<int, AttributeSourceTypes>>) null;
    List<Tuple<int, AttributeSourceTypes>> tupleList = new List<Tuple<int, AttributeSourceTypes>>();
    foreach (string str in strArray1)
    {
      char[] chArray = new char[1]{ ':' };
      string[] strArray2 = str.Split(chArray);
      int num = XmlHelper.DecodeValue(XmlMetadataTypes.Attribute, strArray2[0]);
      if (num == -1)
        return (List<Tuple<int, AttributeSourceTypes>>) null;
      tupleList.Add(new Tuple<int, AttributeSourceTypes>(num, (AttributeSourceTypes) Convert.ToInt32(strArray2[1])));
    }
    return tupleList.Count <= 0 ? (List<Tuple<int, AttributeSourceTypes>>) null : tupleList;
  }

  private static string EncodeInnerText(List<Tuple<int, AttributeSourceTypes>> ids)
  {
    StringBuilder stringBuilder = new StringBuilder();
    foreach (Tuple<int, AttributeSourceTypes> id in ids)
    {
      if (stringBuilder.Length > 0)
        stringBuilder.Append(';');
      stringBuilder.Append(XmlHelper.EncodeValue(XmlMetadataTypes.Attribute, id.Item1));
      stringBuilder.Append(':');
      stringBuilder.Append((int) id.Item2);
    }
    return stringBuilder.ToString();
  }

  private static List<int> DecodeInnerText(string innerText, XmlMetadataTypes innerMetadataType)
  {
    string[] strArray = innerText.Split(';');
    if (strArray.Length == 0)
      return (List<int>) null;
    List<int> intList = new List<int>();
    foreach (string guid in strArray)
    {
      int num = XmlHelper.DecodeValue(innerMetadataType, guid);
      if (num == -1)
        return (List<int>) null;
      intList.Add(num);
    }
    return intList.Count <= 0 ? (List<int>) null : intList;
  }

  private static string EncodeInnerText(List<int> ids, XmlMetadataTypes innerMetadataType)
  {
    StringBuilder stringBuilder = new StringBuilder();
    foreach (int id in ids)
    {
      if (stringBuilder.Length > 0)
        stringBuilder.Append(';');
      stringBuilder.Append(XmlHelper.EncodeValue(innerMetadataType, id));
    }
    return stringBuilder.ToString();
  }

  public static int DecodeValue(XmlMetadataTypes metadataType, string guid)
  {
    if (GuidHelper.IsGuid(guid))
    {
      Guid guid1 = new Guid(guid);
      switch (metadataType)
      {
        case XmlMetadataTypes.Attribute:
          return MetaDataHelper.GetAttributeTypeID(guid1);
        case XmlMetadataTypes.ObjectType:
          return MetaDataHelper.GetObjectTypeID(guid1);
        case XmlMetadataTypes.RelationType:
          return MetaDataHelper.GetRelationTypeID(guid1);
      }
    }
    return -1;
  }

  private static string EncodeValue(XmlMetadataTypes metadataType, int id)
  {
    switch (metadataType)
    {
      case XmlMetadataTypes.Attribute:
        return MetaDataHelper.GetAttributeTypeGuid(id).ToString();
      case XmlMetadataTypes.ObjectType:
        return MetaDataHelper.GetObjectTypeGuid(id).ToString();
      case XmlMetadataTypes.RelationType:
        return MetaDataHelper.GetRelationTypeGuid(id).ToString();
      default:
        return string.Empty;
    }
  }

  public static void CreateAttribute(XmlDocument xmlDoc, XmlNode node, string name, string value)
  {
    XmlAttribute attribute = xmlDoc.CreateAttribute(name);
    attribute.Value = value;
    node.Attributes.Append(attribute);
  }
}
