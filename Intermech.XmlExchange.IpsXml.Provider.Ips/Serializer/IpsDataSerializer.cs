// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Provider.Ips.Serializer.IpsDataSerializer
// Assembly: Intermech.XmlExchange.IpsXml.Provider.Ips, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: B2EE3099-B947-440E-865D-611E406056AB
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Provider.Ips.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Briefcase;
using Intermech.Interfaces.XmlExchange;
using Intermech.IpsXmlViewer.Interfaces;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration;
using Intermech.XmlExchange.IpsXml.Interfaces;
using Intermech.XmlExchange.IpsXml.Interfaces.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using XmlReaderAPI.Data;
using XmlReaderAPI.MetaData;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Provider.Ips.Serializer;

public sealed class IpsDataSerializer : IpsXmlDataProvider, IXmlDataProvider
{
  private string _iconFolderName = string.Empty;
  private string _workerDataFolder = string.Empty;
  private readonly string _datetimeFormat = string.Empty;
  private IServiceProvider _services;

  public IpsDataSerializer(IServiceProvider services)
  {
    this._services = services;
    this.InitializeData();
  }

  public void AddObject(ImObject obj) => this.InternalAddObject(obj);

  public void AddRelation(ImRelation relation) => this.InternalAddRelation(relation, true);

  public void AddAttribute(IImDataElement target, ImAttribute attr)
  {
    this.InternalAddAttribute(target, attr);
  }

  public string[] SaveData()
  {
    return new string[3]
    {
      this.SaveMetadata(),
      this.SaveObjects(),
      this.SaveRelations()
    };
  }

  private string SaveMetadata()
  {
    string fileName = Path.Combine(this._workerDataFolder, XmlExchangeConsts.Common.XmlMetaBriedFileName);
    FileStream fileStream;
    XmlTextWriter xmlWriter;
    BriefcaseProcs.OpenXML(fileName, out fileStream, out xmlWriter, XmlExchangeConsts.XML.XmlMetaBriefDataName);
    try
    {
      this.SaveAttrTypes(xmlWriter);
      this.SaveObjectTypes(xmlWriter);
      this.SaveRelationTypes(xmlWriter);
    }
    finally
    {
      BriefcaseProcs.CloseXML(ref fileStream, ref xmlWriter);
    }
    return fileName;
  }

  private void SaveAttrTypes(XmlTextWriter metaWriter)
  {
    List<ImAttributeType> list = this.GetAllAttrTypes().Cast<ImAttributeType>().ToList<ImAttributeType>();
    GenericListHelper.MakeUnique<ImAttributeType>(list);
    metaWriter.WriteStartElement(XmlExchangeConsts.XML.XmlAttrTypesName);
    list.ForEach((Action<ImAttributeType>) (attrType =>
    {
      IMSAttributeType attributeType = MetaDataHelper.GetAttributeType(attrType.F_ATTRIBUTE_ID);
      if (attributeType != null)
      {
        metaWriter.WriteStartElement(XmlExchangeConsts.XML.XmlAttrTypeName);
        this.WriteToXML(metaWriter, "F_ATTRIBUTE_ID", attributeType.AttributeID.ToString());
        this.WriteToXML(metaWriter, "F_NAME", attributeType.Name);
        this.WriteToXML(metaWriter, "F_ALIAS", attributeType.Alias);
        this.WriteToXML(metaWriter, "F_GUID", attributeType.AttributeGuid.ToString());
        this.WriteToXML(metaWriter, "F_ATTRIBUTE_TYPE", ((int) attributeType.FieldType).ToString());
        string asString1 = attrType.GetAsString(XmlExchangeConsts.XML.F_USER_ID, string.Empty);
        if (!string.IsNullOrEmpty(asString1))
          this.WriteToXML(metaWriter, XmlExchangeConsts.XML.F_USER_ID, asString1);
        string asString2 = attrType.GetAsString(XmlExchangeConsts.XML.F_USER_NAME, string.Empty);
        if (!string.IsNullOrEmpty(asString2))
          this.WriteToXML(metaWriter, XmlExchangeConsts.XML.F_USER_NAME, asString2);
        string asString3 = attrType.GetAsString(XmlExchangeConsts.XML.F_USER_ALIAS, string.Empty);
        if (!string.IsNullOrEmpty(asString3))
          this.WriteToXML(metaWriter, XmlExchangeConsts.XML.F_USER_ALIAS, asString3);
        metaWriter.WriteEndElement();
      }
      else
      {
        metaWriter.WriteStartElement(XmlExchangeConsts.XML.XmlAttrTypeName);
        this.WriteToXML(metaWriter, "F_ATTRIBUTE_ID", attrType.F_ATTRIBUTE_ID.ToString());
        this.WriteToXML(metaWriter, "F_NAME", attrType.F_NAME);
        this.WriteToXML(metaWriter, "F_ATTRIBUTE_TYPE", attrType.F_ATTRIBUTE_TYPE.ToString());
        metaWriter.WriteEndElement();
      }
      metaWriter.Flush();
    }));
    metaWriter.WriteEndElement();
    metaWriter.Flush();
  }

  private void SaveObjectTypes(XmlTextWriter metaWriter)
  {
    List<ImObjectType> list = this.GetAllObjTypes().Cast<ImObjectType>().ToList<ImObjectType>();
    GenericListHelper.MakeUnique<ImObjectType>(list);
    metaWriter.WriteStartElement(XmlExchangeConsts.XML.XmlObjTypesName);
    list.ForEach((Action<ImObjectType>) (objType =>
    {
      IMSObjectType objectType = MetaDataHelper.GetObjectType(objType.F_OBJ_TYPE);
      if (objectType != null)
      {
        metaWriter.WriteStartElement(XmlExchangeConsts.XML.XmlObjTypeName);
        this.WriteToXML(metaWriter, XmlExchangeConsts.XML.F_OBJ_TYPE, objectType.ObjectTypeID.ToString());
        this.WriteToXML(metaWriter, "F_OBJ_TYPE_NAME", objectType.ObjectTypeName);
        this.WriteToXML(metaWriter, "F_GUID", objectType.Guid.ToString());
        string asString1 = objType.GetAsString(XmlExchangeConsts.XML.F_USER_ID, string.Empty);
        if (!string.IsNullOrEmpty(asString1))
          this.WriteToXML(metaWriter, XmlExchangeConsts.XML.F_USER_ID, asString1);
        string asString2 = objType.GetAsString(XmlExchangeConsts.XML.F_USER_NAME, string.Empty);
        if (!string.IsNullOrEmpty(asString2))
          this.WriteToXML(metaWriter, XmlExchangeConsts.XML.F_USER_NAME, asString2);
        string asString3 = objType.GetAsString(XmlExchangeConsts.XML.F_USER_ALIAS, string.Empty);
        if (!string.IsNullOrEmpty(asString3))
          this.WriteToXML(metaWriter, XmlExchangeConsts.XML.F_USER_ALIAS, asString3);
        IDBObjectType dbObjectType = (IDBObjectType) null;
        ConvertSessionInfo service = this._services.GetService<ConvertSessionInfo>();
        if (service != null)
          dbObjectType = service.UserSession.GetObjectType(objectType.ObjectTypeID, false);
        if (dbObjectType != null && dbObjectType.Icon != null)
        {
          string path = $"{this._iconFolderName}{Path.DirectorySeparatorChar.ToString()}{(object) objectType.ObjectTypeID}.ico";
          using (FileStream fileStream = new FileStream(path, FileMode.Create))
          {
            try
            {
              fileStream.Write(dbObjectType.Icon, 0, dbObjectType.Icon.Length);
            }
            finally
            {
              fileStream.Flush();
              fileStream.Close();
            }
          }
          this.WriteToXML(metaWriter, "F_ICON", path.Replace(this._workerDataFolder + Path.DirectorySeparatorChar.ToString(), ""));
        }
        metaWriter.WriteEndElement();
      }
      else
      {
        metaWriter.WriteStartElement(XmlExchangeConsts.XML.XmlObjTypeName);
        this.WriteToXML(metaWriter, XmlExchangeConsts.XML.F_OBJ_TYPE, objType.F_OBJ_TYPE.ToString());
        this.WriteToXML(metaWriter, "F_OBJ_TYPE_NAME", objType.F_OBJ_TYPE_NAME);
        metaWriter.WriteEndElement();
      }
      metaWriter.Flush();
    }));
    metaWriter.WriteEndElement();
    metaWriter.Flush();
  }

  private void SaveRelationTypes(XmlTextWriter metaWriter)
  {
    List<ImRelationType> list = this.GetAllRelTypes().Cast<ImRelationType>().ToList<ImRelationType>();
    GenericListHelper.MakeUnique<ImRelationType>(list);
    metaWriter.WriteStartElement(XmlExchangeConsts.XML.XmlRelTypesName);
    list.ForEach((Action<ImRelationType>) (relType =>
    {
      IMSRelationType relationType = MetaDataHelper.GetRelationType(relType.F_RELATION_TYPE);
      if (relationType != null)
      {
        metaWriter.WriteStartElement(XmlExchangeConsts.XML.XmlRelTypeName);
        this.WriteToXML(metaWriter, "F_RELATION_TYPE", relationType.RelationTypeID.ToString());
        this.WriteToXML(metaWriter, "F_TYPE_NAME", relationType.Description);
        this.WriteToXML(metaWriter, "F_GUID", relationType.Guid.ToString());
        string asString1 = relType.GetAsString(XmlExchangeConsts.XML.F_USER_ID, string.Empty);
        if (!string.IsNullOrEmpty(asString1))
          this.WriteToXML(metaWriter, XmlExchangeConsts.XML.F_USER_ID, asString1);
        string asString2 = relType.GetAsString(XmlExchangeConsts.XML.F_USER_NAME, string.Empty);
        if (!string.IsNullOrEmpty(asString2))
          this.WriteToXML(metaWriter, XmlExchangeConsts.XML.F_USER_NAME, asString2);
        string asString3 = relType.GetAsString(XmlExchangeConsts.XML.F_USER_ALIAS, string.Empty);
        if (!string.IsNullOrEmpty(asString3))
          this.WriteToXML(metaWriter, XmlExchangeConsts.XML.F_USER_ALIAS, asString3);
        metaWriter.WriteEndElement();
      }
      else
      {
        metaWriter.WriteStartElement(XmlExchangeConsts.XML.XmlRelTypeName);
        this.WriteToXML(metaWriter, "F_RELATION_TYPE", relType.F_RELATION_TYPE.ToString());
        this.WriteToXML(metaWriter, "F_TYPE_NAME", relType.F_TYPE_NAME);
        metaWriter.WriteEndElement();
      }
      metaWriter.Flush();
    }));
    metaWriter.WriteEndElement();
    metaWriter.Flush();
  }

  private string SaveObjects()
  {
    string fileName = Path.Combine(this._workerDataFolder, "Objects.xml");
    FileStream fileStream;
    XmlTextWriter xmlWriter;
    BriefcaseProcs.OpenXML(fileName, out fileStream, out xmlWriter, BriefcaseConsts.XmlObjectsDatasetName);
    try
    {
      this.GetAllObjects().Cast<IImObject>().ToList<IImObject>().ForEach((Action<IImObject>) (sourceObj => this.SaveObject(sourceObj, xmlWriter)));
    }
    finally
    {
      BriefcaseProcs.CloseXML(ref fileStream, ref xmlWriter);
    }
    return fileName;
  }

  private void SaveObject(IImObject sourceObj, XmlTextWriter xmlWriter)
  {
    xmlWriter.WriteStartElement(BriefcaseConsts.XmlObjectRecordTag);
    this.WriteToXML(xmlWriter, "F_OBJECT_ID", sourceObj.F_OBJECT_ID.ToString());
    this.WriteToXML(xmlWriter, "F_OBJECTGUID", sourceObj.GetAsString("F_OBJECTGUID", string.Empty));
    this.WriteToXML(xmlWriter, "F_ID", sourceObj.GetAsString("F_ID", string.Empty));
    this.WriteToXML(xmlWriter, "F_IDGUID", sourceObj.GetAsString("F_IDGUID", string.Empty));
    this.WriteToXML(xmlWriter, "F_LC_STEP", sourceObj.GetAsString("F_LC_STEP", string.Empty));
    this.WriteToXML(xmlWriter, "F_VERSION_ID", sourceObj.GetAsString("F_VERSION_ID", string.Empty));
    string asString1 = sourceObj.GetAsString("F_PARENT_ID", string.Empty);
    if (!string.IsNullOrEmpty(asString1))
      this.WriteToXML(xmlWriter, "F_PARENT_ID", asString1);
    string asString2 = sourceObj.GetAsString("F_CHKOUT_BY", string.Empty);
    if (!string.IsNullOrEmpty(asString2))
      this.WriteToXML(xmlWriter, "F_CHKOUT_BY", asString2);
    string asString3 = sourceObj.GetAsString("F_CHKOUTGUID", string.Empty);
    if (!string.IsNullOrEmpty(asString3))
      this.WriteToXML(xmlWriter, "F_CHKOUTGUID", asString3);
    this.WriteToXML(xmlWriter, "F_OBJECT_VER_TYPE", sourceObj.GetAsString("F_OBJECT_VER_TYPE", string.Empty));
    this.WriteToXML(xmlWriter, "F_OBJECT_TYPE", sourceObj.GetAsString("F_OBJECT_TYPE", string.Empty));
    this.WriteToXML(xmlWriter, "F_OWNER_ID", sourceObj.GetAsString("F_OWNER_ID", string.Empty));
    string asString4 = sourceObj.GetAsString("F_OWNERGUID", string.Empty);
    if (!string.IsNullOrEmpty(asString4))
      this.WriteToXML(xmlWriter, "F_OWNERGUID", asString4);
    object obj;
    if (sourceObj.Attributes.TryGetValue("F_MODIFY_DATE", out obj))
    {
      switch (obj)
      {
        case DateTime _:
          this.WriteToXML(xmlWriter, "F_MODIFY_DATE", string.IsNullOrEmpty(this._datetimeFormat) ? XmlConvert.ToString((DateTime) obj, XmlDateTimeSerializationMode.Unspecified) : XmlConvert.ToString((DateTime) obj, this._datetimeFormat));
          break;
        case string _:
          this.WriteToXML(xmlWriter, "F_MODIFY_DATE", Convert.ToString(obj));
          break;
      }
    }
    this.WriteToXML(xmlWriter, "F_LEVEL_ID", sourceObj.GetAsString("F_LEVEL_ID", string.Empty));
    this.WriteToXML(xmlWriter, "F_OBJ_CREATE", sourceObj.GetAsString("F_OBJ_CREATE", string.Empty));
    this.WriteToXML(xmlWriter, "CAPTION", sourceObj.GetAsString("CAPTION", string.Empty));
    string asString5 = sourceObj.GetAsString("F_PROJECT_ID", string.Empty);
    if (!string.IsNullOrEmpty(asString5))
      this.WriteToXML(xmlWriter, "F_PROJECT_ID", asString5);
    string asString6 = sourceObj.GetAsString("F_PROJECTGUID", string.Empty);
    if (!string.IsNullOrEmpty(asString6))
      this.WriteToXML(xmlWriter, "F_PROJECTGUID", asString6);
    this.WriteToXML(xmlWriter, "F_CREATOR_ID", sourceObj.GetAsString("F_CREATOR_ID", string.Empty));
    this.SaveAttributes(sourceObj.Attributes, xmlWriter);
    xmlWriter.WriteEndElement();
    xmlWriter.Flush();
  }

  private void SaveAttributes(IDictionary<string, object> attributes, XmlTextWriter xmlWriter)
  {
    xmlWriter.WriteStartElement(XmlExchangeConsts.XML.XmlAttrDataName);
    foreach (string key in (IEnumerable<string>) attributes.Keys)
    {
      object attribute = attributes[key];
      if (attribute is IImAttribute)
        this.SaveAttribute(attribute as IImAttribute, xmlWriter);
    }
    xmlWriter.WriteEndElement();
  }

  private void SaveAttribute(IImAttribute attr, XmlTextWriter xmlWriter)
  {
    if (attr.MultiValuesCount <= 1)
    {
      xmlWriter.WriteStartElement(BriefcaseConsts.XmlAttributeRecordTag);
      this.WriteToXML(xmlWriter, "F_ATTRIBUTE_ID", attr.F_ATTRIBUTE_ID.ToString());
      this.WriteToXML(xmlWriter, "F_INLIST_ID", attr.F_INLIST_ID.ToString());
      string asString1 = attr.GetAsString("F_INTEGER_VALUE", string.Empty);
      if (!string.IsNullOrEmpty(asString1))
      {
        this.WriteToXML(xmlWriter, "F_INTEGER_VALUE", asString1);
        string asString2 = attr.GetAsString("F_INTEGERGUID", string.Empty);
        if (!string.IsNullOrEmpty(asString2))
          this.WriteToXML(xmlWriter, "F_INTEGERGUID", asString2);
      }
      object obj;
      if (attr.Attributes.TryGetValue("F_DOUBLE_VALUE", out obj))
      {
        switch (obj)
        {
          case double num2:
            Decimal num1 = Convert.ToDecimal(num2);
            this.WriteToXML(xmlWriter, "F_DOUBLE_VALUE", num1.ToString((IFormatProvider) CultureInfo.InvariantCulture));
            string asString3 = attr.GetAsString("F_DOUBLEGUID", string.Empty);
            if (!string.IsNullOrEmpty(asString3))
            {
              this.WriteToXML(xmlWriter, "F_DOUBLEGUID", asString3);
              break;
            }
            break;
          case string _:
            this.WriteToXML(xmlWriter, "F_DOUBLE_VALUE", Convert.ToString(obj));
            break;
        }
      }
      string asString4 = attr.GetAsString("F_STRING_VALUE", string.Empty);
      if (!string.IsNullOrEmpty(asString4))
        this.WriteToXML(xmlWriter, "F_STRING_VALUE", asString4);
      if (attr.Attributes.TryGetValue("F_DATE_VALUE", out obj))
      {
        switch (obj)
        {
          case DateTime _:
            this.WriteToXML(xmlWriter, "F_DATE_VALUE", string.IsNullOrEmpty(this._datetimeFormat) ? XmlConvert.ToString((DateTime) obj, XmlDateTimeSerializationMode.Unspecified) : XmlConvert.ToString((DateTime) obj, this._datetimeFormat));
            break;
          case string _:
            this.WriteToXML(xmlWriter, "F_DATE_VALUE", Convert.ToString(obj));
            break;
        }
      }
      string asString5 = attr.GetAsString("F_FILESIZE", string.Empty);
      if (!string.IsNullOrEmpty(asString5))
        this.WriteToXML(xmlWriter, "F_FILESIZE", asString5);
      string asString6 = attr.GetAsString("F_ARC_METHOD", string.Empty);
      if (!string.IsNullOrEmpty(asString6))
        this.WriteToXML(xmlWriter, "F_ARC_METHOD", asString6);
      string asString7 = attr.GetAsString("F_NOTE", string.Empty);
      if (!string.IsNullOrEmpty(asString7))
        this.WriteToXML(xmlWriter, "F_NOTE", asString7);
      string asString8 = attr.GetAsString("F_PATH2FILE", string.Empty);
      if (!string.IsNullOrEmpty(asString8))
        this.WriteToXML(xmlWriter, "F_PATH2FILE", asString8);
      string asString9 = attr.GetAsString(XmlExchangeConsts.XML.F_EI, string.Empty);
      if (!string.IsNullOrEmpty(asString9))
        this.WriteToXML(xmlWriter, XmlExchangeConsts.XML.F_EI, asString9);
      string asString10 = attr.GetAsString(XmlExchangeConsts.XML.F_EI_OKEI, string.Empty);
      if (!string.IsNullOrEmpty(asString10))
        this.WriteToXML(xmlWriter, XmlExchangeConsts.XML.F_EI_OKEI, asString10);
      this.WriteToXML(xmlWriter, XmlExchangeConsts.XML.F_VALUE, attr.GetAsString(XmlExchangeConsts.XML.F_VALUE, string.Empty));
      if (attr.Attributes.TryGetValue(XmlExchangeConsts.XML.F_BASE_VALUE, out obj))
      {
        switch (obj)
        {
          case double num4:
            Decimal num3 = Convert.ToDecimal(num4);
            this.WriteToXML(xmlWriter, XmlExchangeConsts.XML.F_BASE_VALUE, num3.ToString((IFormatProvider) CultureInfo.InvariantCulture));
            break;
          case string _:
            this.WriteToXML(xmlWriter, XmlExchangeConsts.XML.F_BASE_VALUE, Convert.ToString(obj));
            break;
        }
        this.WriteToXML(xmlWriter, XmlExchangeConsts.XML.F_BASE_ID, attr.GetAsString(XmlExchangeConsts.XML.F_BASE_ID, string.Empty));
        this.WriteToXML(xmlWriter, XmlExchangeConsts.XML.F_BASE_CODE, attr.GetAsString(XmlExchangeConsts.XML.F_BASE_CODE, string.Empty));
      }
      string asString11 = attr.GetAsString("F_FILENAME", string.Empty);
      if (!string.IsNullOrEmpty(asString11))
        this.WriteToXML(xmlWriter, "F_FILENAME", asString11);
      string asString12 = attr.GetAsString("F_LINKTYPE", string.Empty);
      if (!string.IsNullOrEmpty(asString12))
        this.WriteToXML(xmlWriter, "F_LINKTYPE", asString12);
      string asString13 = attr.GetAsString("F_LINKTYPE", string.Empty);
      if (!string.IsNullOrEmpty(asString13))
        this.WriteToXML(xmlWriter, "F_CRC", asString13);
      xmlWriter.WriteEndElement();
    }
    else
    {
      IDictionary<int, IDictionary<string, object>> dictionary = attr.DeNormalize();
      foreach (int key in (IEnumerable<int>) dictionary.Keys)
        this.SaveAttributeValuePart(attr, key, dictionary[key], xmlWriter);
    }
  }

  private void SaveAttributeValuePart(
    IImAttribute attr,
    int valueIdx,
    IDictionary<string, object> valueInfo,
    XmlTextWriter xmlWriter)
  {
    xmlWriter.WriteStartElement(BriefcaseConsts.XmlAttributeRecordTag);
    this.WriteToXML(xmlWriter, "F_ATTRIBUTE_ID", attr.F_ATTRIBUTE_ID.ToString());
    this.WriteToXML(xmlWriter, "F_INLIST_ID", valueIdx.ToString());
    object obj;
    if (valueInfo.TryGetValue("F_INTEGER_VALUE", out obj))
    {
      this.WriteToXML(xmlWriter, "F_INTEGER_VALUE", Convert.ToString(obj));
      if (valueInfo.TryGetValue("F_INTEGERGUID", out obj))
        this.WriteToXML(xmlWriter, "F_INTEGERGUID", Convert.ToString(obj));
    }
    if (valueInfo.TryGetValue("F_DOUBLE_VALUE", out obj))
    {
      if (obj is double)
      {
        Decimal num = Convert.ToDecimal((double) obj);
        this.WriteToXML(xmlWriter, "F_DOUBLE_VALUE", num.ToString((IFormatProvider) CultureInfo.InvariantCulture));
        if (valueInfo.TryGetValue("F_DOUBLEGUID", out obj))
          this.WriteToXML(xmlWriter, "F_DOUBLEGUID", Convert.ToString(obj));
      }
      else if (obj is string)
        this.WriteToXML(xmlWriter, "F_DOUBLE_VALUE", Convert.ToString(obj));
    }
    if (valueInfo.TryGetValue("F_STRING_VALUE", out obj))
      this.WriteToXML(xmlWriter, "F_STRING_VALUE", Convert.ToString(obj));
    if (valueInfo.TryGetValue("F_DATE_VALUE", out obj))
    {
      if (obj is DateTime)
        this.WriteToXML(xmlWriter, "F_DATE_VALUE", string.IsNullOrEmpty(this._datetimeFormat) ? XmlConvert.ToString((DateTime) obj, XmlDateTimeSerializationMode.Unspecified) : XmlConvert.ToString((DateTime) obj, this._datetimeFormat));
      else if (obj is string)
        this.WriteToXML(xmlWriter, "F_DATE_VALUE", Convert.ToString(obj));
    }
    if (valueInfo.TryGetValue("F_FILESIZE", out obj))
      this.WriteToXML(xmlWriter, "F_FILESIZE", Convert.ToString(obj));
    if (valueInfo.TryGetValue("F_ARC_METHOD", out obj))
      this.WriteToXML(xmlWriter, "F_ARC_METHOD", Convert.ToString(obj));
    if (valueInfo.TryGetValue("F_NOTE", out obj))
      this.WriteToXML(xmlWriter, "F_NOTE", Convert.ToString(obj));
    if (valueInfo.TryGetValue("F_PATH2FILE", out obj))
      this.WriteToXML(xmlWriter, "F_PATH2FILE", Convert.ToString(obj));
    if (valueInfo.TryGetValue(XmlExchangeConsts.XML.F_EI, out obj))
      this.WriteToXML(xmlWriter, XmlExchangeConsts.XML.F_EI, Convert.ToString(obj));
    if (valueInfo.TryGetValue(XmlExchangeConsts.XML.F_EI_OKEI, out obj))
      this.WriteToXML(xmlWriter, XmlExchangeConsts.XML.F_EI_OKEI, Convert.ToString(obj));
    if (valueInfo.TryGetValue(XmlExchangeConsts.XML.F_VALUE, out obj))
      this.WriteToXML(xmlWriter, XmlExchangeConsts.XML.F_VALUE, Convert.ToString(obj));
    if (valueInfo.TryGetValue(XmlExchangeConsts.XML.F_BASE_VALUE, out obj))
    {
      if (obj is double num1)
      {
        Decimal num = Convert.ToDecimal(num1);
        this.WriteToXML(xmlWriter, XmlExchangeConsts.XML.F_BASE_VALUE, num.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      }
      else if (obj is string)
        this.WriteToXML(xmlWriter, XmlExchangeConsts.XML.F_BASE_VALUE, Convert.ToString(obj));
      if (valueInfo.TryGetValue(XmlExchangeConsts.XML.F_BASE_ID, out obj))
        this.WriteToXML(xmlWriter, XmlExchangeConsts.XML.F_BASE_ID, Convert.ToString(obj));
      if (valueInfo.TryGetValue(XmlExchangeConsts.XML.F_BASE_CODE, out obj))
        this.WriteToXML(xmlWriter, XmlExchangeConsts.XML.F_BASE_CODE, Convert.ToString(obj));
    }
    if (valueInfo.TryGetValue("F_FILENAME", out obj))
      this.WriteToXML(xmlWriter, "F_FILENAME", Convert.ToString(obj));
    if (valueInfo.TryGetValue("F_LINKTYPE", out obj))
      this.WriteToXML(xmlWriter, "F_LINKTYPE", Convert.ToString(obj));
    if (valueInfo.TryGetValue("F_CRC", out obj))
      this.WriteToXML(xmlWriter, "F_CRC", Convert.ToString(obj));
    xmlWriter.WriteEndElement();
  }

  private string SaveRelations()
  {
    string fileName = Path.Combine(this._workerDataFolder, "Relations.xml");
    FileStream fileStream;
    XmlTextWriter xmlWriter;
    BriefcaseProcs.OpenXML(fileName, out fileStream, out xmlWriter, BriefcaseConsts.XmlRelationsDatasetName);
    try
    {
      this.GetAllRelations().ToList<IImRelation>().ForEach((Action<IImRelation>) (sourceRelation => this.SaveRelation(sourceRelation, xmlWriter)));
    }
    finally
    {
      BriefcaseProcs.CloseXML(ref fileStream, ref xmlWriter);
    }
    return fileName;
  }

  private void SaveRelation(IImRelation sourceRelation, XmlTextWriter xmlWriter)
  {
    xmlWriter.WriteStartElement(BriefcaseConsts.XmlRelationRecordTag);
    this.WriteToXML(xmlWriter, "F_PRJLINK_ID", sourceRelation.GetAsString("F_PRJLINK_ID", string.Empty));
    this.WriteToXML(xmlWriter, "F_PRJ_GUID", sourceRelation.GetAsString("F_PRJ_GUID", string.Empty));
    string asString1 = sourceRelation.GetAsString(XmlExchangeConsts.XML.F_PROJ_OBJ, string.Empty);
    if (!string.IsNullOrEmpty(asString1))
      this.WriteToXML(xmlWriter, XmlExchangeConsts.XML.F_PROJ_OBJ, asString1);
    string asString2 = sourceRelation.GetAsString(XmlExchangeConsts.XML.F_PART_OBJ, string.Empty);
    if (!string.IsNullOrEmpty(asString2))
      this.WriteToXML(xmlWriter, XmlExchangeConsts.XML.F_PART_OBJ, asString2);
    string asString3 = sourceRelation.GetAsString("F_PROJ_ID", string.Empty);
    if (!string.IsNullOrEmpty(asString3))
      this.WriteToXML(xmlWriter, "F_PROJ_ID", asString3);
    string asString4 = sourceRelation.GetAsString("F_PART_ID", string.Empty);
    if (!string.IsNullOrEmpty(asString4))
      this.WriteToXML(xmlWriter, "F_PART_ID", asString4);
    this.WriteToXML(xmlWriter, "F_RELATION_TYPE", sourceRelation.GetAsString("F_RELATION_TYPE", string.Empty));
    object obj;
    if (sourceRelation.Attributes.TryGetValue("F_CREATE_DATE", out obj))
    {
      if (obj is DateTime)
        this.WriteToXML(xmlWriter, "F_CREATE_DATE", string.IsNullOrEmpty(this._datetimeFormat) ? XmlConvert.ToString((DateTime) obj, XmlDateTimeSerializationMode.Unspecified) : XmlConvert.ToString((DateTime) obj, this._datetimeFormat));
      else if (obj is string)
        this.WriteToXML(xmlWriter, "F_CREATE_DATE", Convert.ToString(obj));
    }
    if (sourceRelation.Attributes.TryGetValue("F_DELETE_DATE", out obj))
    {
      switch (obj)
      {
        case DateTime _:
          this.WriteToXML(xmlWriter, "F_DELETE_DATE", string.IsNullOrEmpty(this._datetimeFormat) ? XmlConvert.ToString((DateTime) obj, XmlDateTimeSerializationMode.Unspecified) : XmlConvert.ToString((DateTime) obj, this._datetimeFormat));
          break;
        case string _:
          this.WriteToXML(xmlWriter, "F_DELETE_DATE", Convert.ToString(obj));
          break;
      }
    }
    string asString5 = sourceRelation.GetAsString("F_REL_CREATOR", string.Empty);
    if (!string.IsNullOrEmpty(asString5))
      this.WriteToXML(xmlWriter, "F_REL_CREATOR", asString5);
    this.SaveAttributes(sourceRelation.Attributes, xmlWriter);
    xmlWriter.WriteEndElement();
    xmlWriter.Flush();
  }

  private void WriteToXML(XmlTextWriter xmlWriter, string tag, string tagData)
  {
    xmlWriter.WriteStartElement(tag);
    xmlWriter.WriteString(tagData);
    if (!string.Empty.Equals(tagData))
    {
      xmlWriter.WriteEndElement();
    }
    else
    {
      Formatting formatting = xmlWriter.Formatting;
      try
      {
        xmlWriter.Formatting = Formatting.None;
        xmlWriter.WriteFullEndElement();
      }
      finally
      {
        xmlWriter.Formatting = formatting;
      }
    }
  }

  private void InitializeData()
  {
    this._workerDataFolder = this._services.GetService<ConvertSessionInfo>().WorkDir;
    this._iconFolderName = Path.Combine(this._workerDataFolder, XmlExchangeConsts.Common.IconFolderName);
    XmlUtils.RecreateDirectory(this._iconFolderName);
  }
}
