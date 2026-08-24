// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.ToLocman.IPSToLocmanService
// Assembly: Intermech.XmlExchange.IpsXml.Converter.ToLocman, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 76EBC069-92E6-4D74-866F-DCC1A2BB2547
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.ToLocman.dll

using Intermech.Interfaces;
using Intermech.IpsXmlViewer.Interfaces;
using Intermech.XmlExchange.IpsXml.Converter.ToLocman.Types;
using Intermech.XmlExchange.IpsXml.Interfaces;
using Intermech.XmlExchange.IpsXml.Interfaces.Converter;
using Intermech.XmlExchange.IpsXml.Interfaces.Converter.ConfigTypes;
using Intermech.XmlExchange.IpsXml.Provider.Ips;
using Intermech.XmlExchange.IpsXml.Provider.Ips.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using XmlReaderAPI.MetaData;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.ToLocman;

public class IPSToLocmanService
{
  private IPSToLocmanConfig _config;
  private IpsXmlDataProvider _parser;
  private IDictionary<string, IDictionary<string, string>> _allFilledValues = (IDictionary<string, IDictionary<string, string>>) new Dictionary<string, IDictionary<string, string>>();
  private List<List<List<AttrValueCalcEntity>>> _objsPreparedParams = new List<List<List<AttrValueCalcEntity>>>();
  private IMSAttributeType AT_MATERIAL = MetaDataHelper.GetAttributeType(new Guid("cad0038c-306c-11d8-b4e9-00304f19f545"));
  private IMSAttributeType AT_MATERIAL_SUB1 = MetaDataHelper.GetAttributeType(new Guid("cadd94c2-306c-11d8-b4e9-00304f19f545"));
  private IMSAttributeType AT_MATERIAL_SUB2 = MetaDataHelper.GetAttributeType(new Guid("cadd94c3-306c-11d8-b4e9-00304f19f545"));
  private IMSAttributeType AT_DESIGNATION = MetaDataHelper.GetAttributeType(new Guid("cad0001f-306c-11d8-b4e9-00304f19f545"));
  private IMSAttributeType AT_FILE = MetaDataHelper.GetAttributeType(new Guid("cad0004b-306c-11d8-b4e9-00304f19f545"));
  private string rootDir = "";

  public IPSToLocmanService(IPSToLocmanConfig config) => this._config = config;

  public string Convert(string[] fileNames)
  {
    if (fileNames.Length == 0)
      return string.Empty;
    this.rootDir = Path.GetDirectoryName(fileNames[0]);
    this._parser = new IpsXmlDataFactory().GetDataProvider(fileNames) as IpsXmlDataProvider;
    IReadOnlyCollection<IXmlObject> rootObjects = this._parser.RootObjects;
    XDocument doc = new XDocument();
    switch (this._config.ExportMode)
    {
      case ExportMode.emArtSostav:
        this.ConvertArtSostav(doc, (IEnumerable<IXmlObject>) rootObjects);
        break;
      case ExportMode.emECO_II:
        this.ConvertECOII(doc, (IEnumerable<IXmlObject>) rootObjects);
        break;
      default:
        return string.Empty;
    }
    this.ExportPreparedParams();
    string str = this._config.OutPutFileInfo.FileName;
    if (string.IsNullOrEmpty(Path.GetFileName(this._config.OutPutFileInfo.Name)))
      str = Path.Combine(this._config.OutPutFileInfo.Name, "XML_IM_TO_1C.xml");
    if (string.IsNullOrEmpty(Path.GetDirectoryName(this._config.OutPutFileInfo.Name)))
      str = Path.Combine(Path.GetDirectoryName(Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().CodeBase).Path)), str);
    Encoding encoding = Encoding.GetEncoding(this._config.OutPutFileInfo.Encoding) ?? Encoding.UTF8;
    doc.Declaration = new XDeclaration(this._config.OutPutFileInfo.Version, encoding.BodyName, (string) null);
    using (XmlTextWriter writer = new XmlTextWriter(str, encoding))
      doc.Save((XmlWriter) writer);
    return str;
  }

  private void ConvertArtSostav(XDocument doc, IEnumerable<IXmlObject> headObjects)
  {
    IEnumerable<IXmlObject> xmlObjects = headObjects.Where<IXmlObject>((Func<IXmlObject, bool>) (headObj => headObj is IImObject targetObj && targetObj.IsArticle()));
    XNamespace xnamespace = XNamespace.Get("http://www.w3.org/2001/XMLSchema-instance");
    XElement xelement = new XElement((XName) "PDMData", new object[2]
    {
      (object) new XAttribute(XNamespace.Xmlns + "xsi", (object) xnamespace),
      (object) new XAttribute(xnamespace + "noNamespaceSchemaLocation", (object) "LoodsmanData.xsd")
    });
    Dictionary<string, List<IImObject>> allIsps = new Dictionary<string, List<IImObject>>();
    this.collectIsps((IEnumerable<IXmlObject>) this._parser.GetAllObjects(), allIsps);
    foreach (IXmlObject xmlObject in xmlObjects)
      this.ExportObject(xelement, xmlObject as IImObject, (IImRelation) null, allIsps);
    doc.Add((object) xelement);
  }

  private void ConvertECOII(XDocument doc, IEnumerable<IXmlObject> headObjects)
  {
    IEnumerable<IXmlObject> xmlObjects = headObjects.Where<IXmlObject>((Func<IXmlObject, bool>) (headObj => headObj is IImObject targetObj && targetObj.IsEcoII()));
    XNamespace xnamespace = XNamespace.Get("http://www.w3.org/2001/XMLSchema-instance");
    XElement xelement = new XElement((XName) "PDMData", new object[2]
    {
      (object) new XAttribute(XNamespace.Xmlns + "xsi", (object) xnamespace),
      (object) new XAttribute(xnamespace + "noNamespaceSchemaLocation", (object) "LoodsmanData.xsd")
    });
    Dictionary<string, List<IImObject>> allIsps = new Dictionary<string, List<IImObject>>();
    this.collectIsps((IEnumerable<IXmlObject>) this._parser.GetAllObjects(), allIsps);
    foreach (IXmlObject ecoII in xmlObjects)
      this.ExportECOIIObject(xelement, ecoII as IImObject, allIsps);
    doc.Add((object) xelement);
  }

  private void collectIsps(
    IEnumerable<IXmlObject> objects,
    Dictionary<string, List<IImObject>> allIsps)
  {
    foreach (IXmlObject xmlObject in objects)
    {
      if (xmlObject is IImObject targetObj && targetObj.IsIsp())
      {
        string mainArtDesign = this.GetMainArtDesign(targetObj);
        if (!string.IsNullOrEmpty(mainArtDesign) && !this.GetObjDesign(targetObj).Equals(mainArtDesign, StringComparison.OrdinalIgnoreCase))
        {
          List<IImObject> imObjectList;
          if (!allIsps.TryGetValue(mainArtDesign, out imObjectList))
          {
            imObjectList = new List<IImObject>();
            allIsps.Add(mainArtDesign, imObjectList);
          }
          imObjectList.Add(targetObj);
        }
      }
    }
  }

  private void ExportObject(
    XElement nodePDMData,
    IImObject obj,
    IImRelation linkToObj,
    Dictionary<string, List<IImObject>> allIsps)
  {
    XElement xelement1 = new XElement((XName) "Product");
    this.ExportAttrs(xelement1, obj, linkToObj);
    nodePDMData.Add((object) xelement1);
    XElement xelement2 = new XElement((XName) "Version");
    this.ExportAttrs(xelement2, obj, linkToObj);
    xelement1.Add((object) xelement2);
    if (obj.IsDocument())
      this.ExportAuthenticFile(xelement2, obj, linkToObj);
    this.ExportArtSostavLinks(obj, xelement2, nodePDMData, allIsps);
    this.ExportMatLinks(obj, xelement2, nodePDMData, allIsps);
    this.ExportIspLinks(obj, xelement2, nodePDMData, allIsps);
  }

  private void ExportECOIIObject(
    XElement nodePDMData,
    IImObject ecoII,
    Dictionary<string, List<IImObject>> allIsps)
  {
    XElement xelement1 = new XElement((XName) "Product");
    this.ExportAttrs(xelement1, ecoII, customContext: "Изменение");
    nodePDMData.Add((object) xelement1);
    XElement xelement2 = new XElement((XName) "Version");
    this.ExportAttrs(xelement2, ecoII);
    xelement1.Add((object) xelement2);
    this.ExportECOIIDocument(nodePDMData, xelement1, ecoII);
    this.ExportECOIIContent(nodePDMData, xelement2, ecoII, allIsps);
  }

  private void ExportECOIIDocument(XElement nodePDMData, XElement nodeEcoII, IImObject ecoII)
  {
    XElement xelement1 = new XElement((XName) "Product");
    this.ExportAttrs(xelement1, ecoII, customContext: "Извещение об изменении (ИИ)");
    nodePDMData.Add((object) xelement1);
    XElement xelement2 = new XElement((XName) "Version");
    this.ExportAttrs(xelement2, ecoII);
    xelement1.Add((object) xelement2);
    if (ecoII.IsDocument())
      this.ExportAuthenticFile(xelement2, ecoII, (IImRelation) null);
    XElement content1 = new XElement((XName) "Links", (object) new XAttribute((XName) "LinkName", (object) "Документы"));
    nodeEcoII.Add((object) content1);
    XElement content2 = new XElement((XName) "LinkVersion");
    content1.Add((object) content2);
    string str = ecoII.Attributes["F_OBJECT_ID"].ToString();
    if (string.IsNullOrEmpty(str))
      return;
    XAttribute content3 = new XAttribute((XName) "uIDVer", (object) str);
    content2.Add((object) content3);
  }

  private void ExportECOIIContent(
    XElement nodePDMData,
    XElement nodeVersionEcoII,
    IImObject ecoII,
    Dictionary<string, List<IImObject>> allIsps)
  {
    XElement content1 = new XElement((XName) "Links", (object) new XAttribute((XName) "LinkName", (object) "Касается"));
    nodeVersionEcoII.Add((object) content1);
    XElement content2 = new XElement((XName) "Links", (object) new XAttribute((XName) "LinkName", (object) "Создает версию ..."));
    nodeVersionEcoII.Add((object) content2);
    IReadOnlyCollection<IXmlRelation> objChildRelations = this._parser.GetObjChildRelations(ecoII as IXmlObject);
    if (objChildRelations == null)
      return;
    foreach (IXmlRelation xmlRelation in (IEnumerable<IXmlRelation>) objChildRelations)
    {
      if (this._parser.GetRelType(xmlRelation as IImRelation).F_GUID.ToString().Equals("cad0036b-306c-11d8-b4e9-00304f19f545", StringComparison.OrdinalIgnoreCase))
      {
        IXmlObject relChildObj = this._parser.GetRelChildObj(xmlRelation);
        XElement xelement = new XElement((XName) "LinkVersion");
        content2.Add((object) xelement);
        this.ExportAttrs(xelement, relChildObj as IImObject, xmlRelation as IImRelation, "Создает версию ...");
        this.ExportObject(nodePDMData, relChildObj as IImObject, xmlRelation as IImRelation, allIsps);
      }
    }
  }

  private void ExportAuthenticFile(XElement nodeVersion, IImObject obj, IImRelation linkToObj)
  {
    XElement xelement1 = new XElement((XName) "File");
    this.ExportAttrs(xelement1, obj, linkToObj);
    nodeVersion.Add((object) xelement1);
    string dictAttrKey = ImAttributeType.GetDictAttrKey(this.AT_FILE.AttributeID.ToString());
    if (!obj.Attributes.ContainsKey(dictAttrKey))
      return;
    foreach (IDictionary<string, object> dictionary in (IEnumerable<IDictionary<string, object>>) (obj.Attributes[dictAttrKey] as IImAttribute).DeNormalize().Values)
    {
      object obj1;
      if (dictionary.TryGetValue("F_LINKTYPE", out obj1) && int.Parse((string) obj1) == 4 && dictionary.TryGetValue("F_PATH2FILE", out obj1))
      {
        string path = Path.Combine(this.rootDir, (string) obj1);
        if (!string.IsNullOrEmpty(path) && File.Exists(path))
        {
          XElement xelement2 = new XElement((XName) "AltView");
          this.ExportAttrs(xelement2, obj, linkToObj);
          nodeVersion.Add((object) xelement2);
          XElement xelement3 = new XElement((XName) "View");
          XAttribute content = new XAttribute((XName) "ext", (object) Path.GetExtension(path).Substring(1));
          xelement3.Add((object) content);
          this.ExportAttrs(xelement3, obj, linkToObj);
          xelement2.Add((object) xelement3);
          string base64String = System.Convert.ToBase64String(File.ReadAllBytes(path), Base64FormattingOptions.InsertLineBreaks);
          xelement3.Add((object) base64String);
        }
      }
    }
  }

  private void ExportIspLinks(
    IImObject obj,
    XElement nodeVersion,
    XElement nodePDMData,
    Dictionary<string, List<IImObject>> allIsps)
  {
    string objDesign = this.GetObjDesign(obj);
    List<IImObject> imObjectList;
    if (string.IsNullOrEmpty(objDesign) || !allIsps.TryGetValue(objDesign, out imObjectList) || imObjectList.Count == 0)
      return;
    XElement content = new XElement((XName) "Links", (object) new XAttribute((XName) "LinkName", (object) "Исполнения"));
    nodeVersion.Add((object) content);
    foreach (IImObject sourceIMObj in imObjectList)
    {
      XElement xelement = new XElement((XName) "LinkVersion");
      content.Add((object) xelement);
      this.ExportAttrs(xelement, sourceIMObj, customContext: "Исполнения");
    }
  }

  private void ExportArtSostavLinks(
    IImObject obj,
    XElement nodeVersion,
    XElement nodePDMData,
    Dictionary<string, List<IImObject>> allIsps)
  {
    Dictionary<int, List<IImRelation>> dictionary = new Dictionary<int, List<IImRelation>>();
    IReadOnlyCollection<IXmlRelation> objChildRelations = this._parser.GetObjChildRelations(obj as IXmlObject);
    if (objChildRelations == null)
      return;
    foreach (IXmlRelation relation in (IEnumerable<IXmlRelation>) objChildRelations)
    {
      List<IImRelation> imRelationList = (List<IImRelation>) null;
      IImRelationType relType = this._parser.GetRelType(relation as IImRelation);
      if (!dictionary.TryGetValue(relType.F_RELATION_TYPE, out imRelationList))
      {
        imRelationList = new List<IImRelation>();
        dictionary.Add(relType.F_RELATION_TYPE, imRelationList);
      }
      imRelationList.Add(relation as IImRelation);
    }
    foreach (int key in dictionary.Keys)
    {
      string linkContext = "Состоит из...";
      switch (key)
      {
        case 1:
          linkContext = "Состоит из...";
          break;
        case 1004:
          linkContext = "Документы";
          break;
      }
      XElement nodeLinks = new XElement((XName) "Links", (object) new XAttribute((XName) "LinkName", (object) linkContext));
      nodeVersion.Add((object) nodeLinks);
      dictionary[key].ForEach((Action<IImRelation>) (link =>
      {
        XElement xelement = new XElement((XName) "LinkVersion");
        nodeLinks.Add((object) xelement);
        IXmlObject relChildObj = this._parser.GetRelChildObj(link as IXmlRelation);
        this.ExportAttrs(xelement, relChildObj as IImObject, link, linkContext);
        this.ExportObject(nodePDMData, relChildObj as IImObject, link, allIsps);
      }));
    }
  }

  private void ExportMatLinks(
    IImObject obj,
    XElement nodeVersion,
    XElement nodePDMData,
    Dictionary<string, List<IImObject>> allIsps)
  {
    string dictAttrKey1 = ImAttributeType.GetDictAttrKey(this.AT_MATERIAL.AttributeID.ToString());
    if (obj.Attributes.ContainsKey(dictAttrKey1))
    {
      string asString = (obj.Attributes[dictAttrKey1] as IImAttribute).GetAsString("F_VALUE", string.Empty);
      if (!string.IsNullOrEmpty(asString))
      {
        XElement content1 = new XElement((XName) "Links", (object) new XAttribute((XName) "LinkName", (object) "Изготавливается из..."));
        nodeVersion.Add((object) content1);
        XElement content2 = new XElement((XName) "LinkVersion", (object) new XAttribute((XName) "uIDVer", (object) asString));
        content1.Add((object) content2);
        IImXmlObject objectById = this._parser.FindObjectById(asString);
        if (objectById != null)
          this.ExportObject(nodePDMData, (IImObject) objectById, (IImRelation) null, allIsps);
      }
    }
    string dictAttrKey2 = ImAttributeType.GetDictAttrKey(this.AT_MATERIAL_SUB1.AttributeID.ToString());
    string dictAttrKey3 = ImAttributeType.GetDictAttrKey(this.AT_MATERIAL_SUB2.AttributeID.ToString());
    if (!obj.Attributes.ContainsKey(dictAttrKey2) && !obj.Attributes.ContainsKey(dictAttrKey3))
      return;
    XElement content3 = new XElement((XName) "Links", (object) new XAttribute((XName) "LinkName", (object) "Заменитель"));
    nodeVersion.Add((object) content3);
    if (obj.Attributes.ContainsKey(dictAttrKey2))
    {
      string asString = (obj.Attributes[dictAttrKey2] as IImAttribute).GetAsString("F_VALUE", string.Empty);
      if (!string.IsNullOrEmpty(asString))
      {
        XElement content4 = new XElement((XName) "LinkVersion", (object) new XAttribute((XName) "uIDVer", (object) asString));
        content3.Add((object) content4);
        IImXmlObject objectById = this._parser.FindObjectById(asString);
        if (objectById != null)
          this.ExportObject(nodePDMData, (IImObject) objectById, (IImRelation) null, allIsps);
      }
    }
    if (!obj.Attributes.ContainsKey(dictAttrKey3))
      return;
    string asString1 = (obj.Attributes[dictAttrKey3] as IImAttribute).GetAsString("F_VALUE", string.Empty);
    if (string.IsNullOrEmpty(asString1))
      return;
    XElement content5 = new XElement((XName) "LinkVersion", (object) new XAttribute((XName) "uIDVer", (object) asString1));
    content3.Add((object) content5);
    IImXmlObject objectById1 = this._parser.FindObjectById(asString1);
    if (objectById1 == null)
      return;
    this.ExportObject(nodePDMData, (IImObject) objectById1, (IImRelation) null, allIsps);
  }

  private void ExportAttrs(
    XElement targetNode,
    IImObject sourceIMObj = null,
    IImRelation sourceIMRelation = null,
    string customContext = "")
  {
    NodeConfig nodeConfig = this._config.NodeConfigs[targetNode.Name.LocalName];
    if (nodeConfig == null)
      return;
    IDictionary<string, string> filledValues;
    if (sourceIMObj != null)
    {
      string uniqueId = this.GetUniqueID(sourceIMObj, sourceIMRelation);
      if (!this._allFilledValues.TryGetValue(uniqueId, out filledValues))
      {
        filledValues = (IDictionary<string, string>) new Dictionary<string, string>();
        this._allFilledValues.Add(uniqueId, filledValues);
      }
    }
    else
      filledValues = (IDictionary<string, string>) new Dictionary<string, string>();
    List<AttrValueCalcEntity> simpleEntities = new List<AttrValueCalcEntity>();
    List<AttrValueCalcEntity> fixedFuncEntities = new List<AttrValueCalcEntity>();
    List<AttrValueCalcEntity> IMParamsEntities = new List<AttrValueCalcEntity>();
    List<AttrValueCalcEntity> localEntities = new List<AttrValueCalcEntity>();
    List<AttrValueCalcEntity> grouppedLocalEntities = new List<AttrValueCalcEntity>();
    List<AttrValueCalcEntity> substituteEntities = new List<AttrValueCalcEntity>();
    List<AttrConfig> allAttrs = new List<AttrConfig>();
    Dictionary<string, List<AttrValueCalcEntity>> entitiesByAttr = new Dictionary<string, List<AttrValueCalcEntity>>();
    string context = !string.IsNullOrEmpty(customContext) ? customContext : (sourceIMObj != null ? this.FindCompatibleConfigContextForObject(sourceIMObj, nodeConfig.AttrConfigs.contexts) : string.Empty);
    nodeConfig.AttrConfigs.ForEach((Action<AttrConfig>) (attrConfig =>
    {
      allAttrs.Add(attrConfig);
      Dictionary<string, List<LocalValueConfig>> groups = new Dictionary<string, List<LocalValueConfig>>();
      attrConfig.ValueConfigs.ForEach((Action<ValueConfig>) (valueConfig =>
      {
        AttrValueCalcEntity attrValueCalcEntity = (AttrValueCalcEntity) null;
        switch (valueConfig.ValueType)
        {
          case ConfigFormat.AttrValueType.avtSimple:
            attrValueCalcEntity = new AttrValueCalcEntity(attrConfig, targetNode, sourceIMObj, sourceIMRelation);
            attrValueCalcEntity.ValueConfigs.Add(valueConfig);
            simpleEntities.Add(attrValueCalcEntity);
            break;
          case ConfigFormat.AttrValueType.avtIMObjectAttr:
          case ConfigFormat.AttrValueType.avtIMRelationAttr:
            attrValueCalcEntity = new AttrValueCalcEntity(attrConfig, targetNode, sourceIMObj, sourceIMRelation);
            attrValueCalcEntity.ValueConfigs.Add(valueConfig);
            IMParamsEntities.Add(attrValueCalcEntity);
            break;
          case ConfigFormat.AttrValueType.avtLocal:
            LocalValueConfig localValueConfig = valueConfig as LocalValueConfig;
            if (string.IsNullOrEmpty(localValueConfig.GroupID))
            {
              attrValueCalcEntity = new AttrValueCalcEntity(attrConfig, targetNode, sourceIMObj, sourceIMRelation);
              attrValueCalcEntity.ValueConfigs.Add(valueConfig);
              localEntities.Add(attrValueCalcEntity);
              break;
            }
            List<LocalValueConfig> localValueConfigList;
            if (!groups.TryGetValue(localValueConfig.GroupID, out localValueConfigList))
            {
              localValueConfigList = new List<LocalValueConfig>();
              groups.Add(localValueConfig.GroupID, localValueConfigList);
            }
            localValueConfigList.Add(localValueConfig);
            break;
          case ConfigFormat.AttrValueType.avtFixedFunc:
            attrValueCalcEntity = new AttrValueCalcEntity(attrConfig, targetNode, sourceIMObj, sourceIMRelation);
            attrValueCalcEntity.ValueConfigs.Add(valueConfig);
            fixedFuncEntities.Add(attrValueCalcEntity);
            break;
          case ConfigFormat.AttrValueType.avtSubstitute:
            attrValueCalcEntity = new AttrValueCalcEntity(attrConfig, targetNode, sourceIMObj, sourceIMRelation);
            attrValueCalcEntity.ValueConfigs.Add(valueConfig);
            substituteEntities.Add(attrValueCalcEntity);
            break;
        }
        if (attrValueCalcEntity == null)
          return;
        List<AttrValueCalcEntity> attrValueCalcEntityList;
        if (!entitiesByAttr.TryGetValue(attrConfig.Name, out attrValueCalcEntityList))
        {
          attrValueCalcEntityList = new List<AttrValueCalcEntity>();
          entitiesByAttr.Add(attrConfig.Name, attrValueCalcEntityList);
        }
        attrValueCalcEntityList.Add(attrValueCalcEntity);
      }));
      foreach (List<LocalValueConfig> localValueConfigList in groups.Values)
      {
        AttrValueCalcEntity calcEntity = new AttrValueCalcEntity(attrConfig, targetNode, sourceIMObj, sourceIMRelation);
        Action<LocalValueConfig> action = (Action<LocalValueConfig>) (localValueConfig => calcEntity.ValueConfigs.Add((ValueConfig) localValueConfig));
        localValueConfigList.ForEach(action);
        calcEntity.sortGrouppedValuesConfigs();
        grouppedLocalEntities.Add(calcEntity);
        List<AttrValueCalcEntity> attrValueCalcEntityList;
        if (!entitiesByAttr.TryGetValue(attrConfig.Name, out attrValueCalcEntityList))
        {
          attrValueCalcEntityList = new List<AttrValueCalcEntity>();
          entitiesByAttr.Add(attrConfig.Name, attrValueCalcEntityList);
        }
        attrValueCalcEntityList.Add(calcEntity);
      }
    }), context, (Predicate<AttrConfig>) null);
    this.CalcSimpleEntities(simpleEntities, filledValues);
    this.CalcFixedFuncEntities(fixedFuncEntities, filledValues);
    this.CalcIMParamsEntities(IMParamsEntities, filledValues);
    this.CalcLocalEntities(localEntities, filledValues);
    this.CalcGrouppedLocalEntities(grouppedLocalEntities, filledValues);
    List<List<AttrValueCalcEntity>> objParams = new List<List<AttrValueCalcEntity>>();
    allAttrs.Sort((Comparison<AttrConfig>) ((left, right) => left.Order - right.Order));
    allAttrs.ForEach((Action<AttrConfig>) (attrConfig =>
    {
      List<AttrValueCalcEntity> attrValueCalcEntityList;
      if (!entitiesByAttr.TryGetValue(attrConfig.Name, out attrValueCalcEntityList))
        return;
      objParams.Add(attrValueCalcEntityList);
    }));
    this._objsPreparedParams.Add(objParams);
  }

  private void ExportPreparedParams()
  {
    this._objsPreparedParams.ForEach((Action<List<List<AttrValueCalcEntity>>>) (objParams =>
    {
      Dictionary<string, AttrValueCalcEntity> dictionary = new Dictionary<string, AttrValueCalcEntity>();
      XElement attrOwnerNode = (XElement) null;
      objParams.ForEach((Action<List<AttrValueCalcEntity>>) (attrParams =>
      {
        attrParams.Sort((Comparison<AttrValueCalcEntity>) ((left, right) => left.ValueConfigs[0].Order - right.ValueConfigs[0].Order));
        attrParams.ForEach((Action<AttrValueCalcEntity>) (preparedParam =>
        {
          if (attrOwnerNode == null)
            attrOwnerNode = preparedParam.AttrOwnerNode;
          if (preparedParam.AttrValue.Count == 0 || string.IsNullOrEmpty(preparedParam.AttrValue[0]) || !preparedParam.TargetAttrConfig.Export)
            return;
          if (string.IsNullOrEmpty(preparedParam.ValueConfigs[0].AttrName) || !string.IsNullOrEmpty(preparedParam.ValueConfigs[0].AttrName) && !preparedParam.TargetAttrConfig.Name.Equals(preparedParam.ValueConfigs[0].AttrName, StringComparison.OrdinalIgnoreCase))
          {
            IEnumerable<XElement> xelements = attrOwnerNode.Elements((XName) "Attribute");
            XElement content = (XElement) null;
            foreach (XElement xelement in xelements)
            {
              if (xelement.Attribute((XName) "Name").Value == preparedParam.TargetAttrConfig.Name)
              {
                content = xelement;
                break;
              }
            }
            if (content == null)
            {
              content = new XElement((XName) "Attribute", (object) new XAttribute((XName) "Name", (object) preparedParam.TargetAttrConfig.Name));
              attrOwnerNode.Add((object) content);
            }
            string name = string.IsNullOrEmpty(preparedParam.ValueConfigs[0].AttrName) || preparedParam.TargetAttrConfig.Name.Equals(preparedParam.ValueConfigs[0].AttrName, StringComparison.OrdinalIgnoreCase) ? "Value" : preparedParam.ValueConfigs[0].AttrName;
            if (content.Attribute((XName) name) != null)
              return;
            content.Add((object) new XAttribute((XName) name, (object) preparedParam.AttrValue));
          }
          else
          {
            XAttribute xattribute = attrOwnerNode.Attribute((XName) preparedParam.ValueConfigs[0].AttrName);
            if (xattribute == null)
            {
              attrOwnerNode.Add((object) new XAttribute((XName) preparedParam.ValueConfigs[0].AttrName, (object) preparedParam.AttrValue));
            }
            else
            {
              if (preparedParam.AttrValue.Count <= 0)
                return;
              xattribute.Value = preparedParam.AttrValue[0];
            }
          }
        }));
      }));
    }));
  }

  private string FindCompatibleConfigContextForObject(
    IImObject obj,
    HashSet<string> availableContexts)
  {
    IImObjectType objType = this._parser.GetObjType(obj);
    if (objType == null)
      return string.Empty;
    IMSObjectType objectType1 = MetaDataHelper.GetObjectType(objType.F_GUID);
    if (objectType1 != null)
      return string.Empty;
    if (availableContexts.Contains(objectType1.ObjectTypeName))
      return objectType1.ObjectTypeName;
    string foundContext = string.Empty;
    MetaDataHelper.GetObjectTypeParentsID(objectType1.ObjectTypeID).ForEach((Action<int>) (parentObjTypeID =>
    {
      IMSObjectType objectType2 = MetaDataHelper.GetObjectType(parentObjTypeID);
      if (!availableContexts.Contains(objectType2.ObjectTypeName))
        return;
      foundContext = objectType2.ObjectTypeName;
    }));
    return foundContext;
  }

  private void CalcSimpleEntities(
    List<AttrValueCalcEntity> simpleEntities,
    IDictionary<string, string> filledValues)
  {
    simpleEntities.ForEach((Action<AttrValueCalcEntity>) (entity =>
    {
      ValueConfig valueConfig = entity.ValueConfigs[0];
      string str = this.FormatValue(this.GetConvertedValue(valueConfig.Value, valueConfig), valueConfig);
      if (string.IsNullOrEmpty(str))
        return;
      this.SetObjAttrValue(valueConfig.SurroundSymbol + str + valueConfig.SurroundSymbol, entity, valueConfig, filledValues);
    }));
  }

  private void CalcFixedFuncEntities(
    List<AttrValueCalcEntity> fixedFuncEntities,
    IDictionary<string, string> filledValues)
  {
    fixedFuncEntities.ForEach((Action<AttrValueCalcEntity>) (entity =>
    {
      FixedFuncValueConfig valueConfig = entity.ValueConfigs[0] as FixedFuncValueConfig;
      if (valueConfig.FuncType != ConfigFormat.FixedFuncType.fftCurDate)
        return;
      string format;
      switch (valueConfig.ValueDataType)
      {
        case ConfigFormat.AttrValueDataType.avdtDate:
          format = "yyyy-MM-dd";
          break;
        case ConfigFormat.AttrValueDataType.avdtDateTime:
          format = "yyyy-MM-dd\\THH:mm:ss";
          break;
        case ConfigFormat.AttrValueDataType.avdtTime:
          format = "HH:mm:ss";
          break;
        default:
          format = "yyyy-MM-dd\\THH:mm:ss";
          break;
      }
      this.SetObjAttrValue(valueConfig.SurroundSymbol + DateTime.Now.ToString(format) + valueConfig.SurroundSymbol, entity, (ValueConfig) valueConfig, filledValues);
    }));
  }

  private void CalcIMParamsEntities(
    List<AttrValueCalcEntity> IMParamsEntities,
    IDictionary<string, string> filledValues)
  {
    IMParamsEntities.ForEach((Action<AttrValueCalcEntity>) (entity =>
    {
      IMSAttributeBasedValueConfig valueConfig = entity.ValueConfigs[0] as IMSAttributeBasedValueConfig;
      string empty = string.Empty;
      IImAttribute attr = (IImAttribute) null;
      List<string> values = new List<string>();
      if (valueConfig.sourceIMAttrType != null)
      {
        string dictAttrKey = ImAttributeType.GetDictAttrKey(valueConfig.sourceIMAttrType.AttributeID.ToString());
        if (string.IsNullOrEmpty(dictAttrKey))
          return;
        switch (valueConfig.ValueType)
        {
          case ConfigFormat.AttrValueType.avtIMObjectAttr:
            if (entity.SourceIMObject != null && entity.SourceIMObject.Attributes.ContainsKey(dictAttrKey))
            {
              attr = entity.SourceIMObject.Attributes[dictAttrKey] as IImAttribute;
              break;
            }
            break;
          case ConfigFormat.AttrValueType.avtIMRelationAttr:
            if (entity.SourceIMRelation != null && entity.SourceIMRelation.Attributes.ContainsKey(dictAttrKey))
            {
              attr = entity.SourceIMRelation.Attributes[dictAttrKey] as IImAttribute;
              break;
            }
            break;
        }
        IPSToLocmanService.GetAttrValues(attr, valueConfig, values);
      }
      else
      {
        string str = valueConfig.Value;
        if (string.IsNullOrEmpty(str))
          return;
        switch (valueConfig.ValueType)
        {
          case ConfigFormat.AttrValueType.avtIMObjectAttr:
            if (entity.SourceIMObject != null && entity.SourceIMObject.Attributes.ContainsKey(str))
            {
              if (entity.SourceIMObject.Attributes[str] is IImAttribute attribute3)
              {
                IPSToLocmanService.GetAttrValues(attribute3, valueConfig, values);
                break;
              }
              values.Add(entity.SourceIMObject.Attributes[str].ToString());
              break;
            }
            IImAttributeType attrType1 = this._parser.GetAttrType(str);
            if (attrType1 != null)
            {
              string dictAttrKey = ImAttributeType.GetDictAttrKey(attrType1.F_ATTRIBUTE_ID.ToString());
              if (entity.SourceIMObject != null && entity.SourceIMObject.Attributes.ContainsKey(dictAttrKey))
              {
                IPSToLocmanService.GetAttrValues(entity.SourceIMObject.Attributes[dictAttrKey] as IImAttribute, valueConfig, values);
                break;
              }
              break;
            }
            break;
          case ConfigFormat.AttrValueType.avtIMRelationAttr:
            if (entity.SourceIMRelation != null && entity.SourceIMRelation.Attributes.ContainsKey(str))
            {
              if (entity.SourceIMRelation.Attributes[str] is IImAttribute attribute4)
              {
                IPSToLocmanService.GetAttrValues(attribute4, valueConfig, values);
                break;
              }
              values.Add(entity.SourceIMRelation.Attributes[str].ToString());
              break;
            }
            IImAttributeType attrType2 = this._parser.GetAttrType(str);
            if (attrType2 != null)
            {
              string dictAttrKey = ImAttributeType.GetDictAttrKey(attrType2.F_ATTRIBUTE_ID.ToString());
              if (entity.SourceIMRelation != null && entity.SourceIMRelation.Attributes.ContainsKey(dictAttrKey))
              {
                IPSToLocmanService.GetAttrValues(entity.SourceIMRelation.Attributes[dictAttrKey] as IImAttribute, valueConfig, values);
                break;
              }
              break;
            }
            break;
        }
      }
      values.ForEach((Action<string>) (value =>
      {
        string str = this.FormatValue(this.GetConvertedValue(value, (ValueConfig) valueConfig), (ValueConfig) valueConfig);
        if (string.IsNullOrEmpty(str))
          return;
        this.SetObjAttrValue(valueConfig.SurroundSymbol + str + valueConfig.SurroundSymbol, entity, (ValueConfig) valueConfig, filledValues);
      }));
    }));
  }

  private static void GetAttrValues(
    IImAttribute attr,
    IMSAttributeBasedValueConfig valueConfig,
    List<string> values)
  {
    if (attr != null)
    {
      IDictionary<int, IDictionary<string, object>> dictionary1 = attr.DeNormalize();
      if (dictionary1.Count > 1)
      {
        foreach (IDictionary<string, object> dictionary2 in (IEnumerable<IDictionary<string, object>>) dictionary1.Values)
        {
          object obj;
          if (dictionary2.TryGetValue(valueConfig.AttrInternalFieldName, out obj))
            values.Add((string) obj);
          else
            values.Add(string.Empty);
        }
      }
      else
        values.Add(attr.GetAsString(valueConfig.AttrInternalFieldName, string.Empty));
    }
    else
      values.Add(string.Empty);
  }

  private void CalcLocalEntities(
    List<AttrValueCalcEntity> localEntities,
    IDictionary<string, string> filledValues)
  {
    localEntities.ForEach((Action<AttrValueCalcEntity>) (entity =>
    {
      LocalValueConfig valueConfig = entity.ValueConfigs[0] as LocalValueConfig;
      string originValue;
      if (!filledValues.TryGetValue(valueConfig.LocalAttrName, out originValue))
        return;
      string str = this.FormatValue(this.GetConvertedValue(originValue, (ValueConfig) valueConfig), (ValueConfig) valueConfig);
      if (string.IsNullOrEmpty(str))
        return;
      this.SetObjAttrValue(valueConfig.SurroundSymbol + str + valueConfig.SurroundSymbol, entity, (ValueConfig) valueConfig, filledValues);
    }));
  }

  private void CalcGrouppedLocalEntities(
    List<AttrValueCalcEntity> grouppedLocalEntities,
    IDictionary<string, string> filledValues)
  {
    grouppedLocalEntities.ForEach((Action<AttrValueCalcEntity>) (entity =>
    {
      if (entity.ValueConfigs.Count == 0)
        return;
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < entity.ValueConfigs.Count; ++index)
      {
        LocalValueConfig valueConfig = entity.ValueConfigs[index] as LocalValueConfig;
        string originValue;
        if (filledValues.TryGetValue(valueConfig.LocalAttrName, out originValue))
        {
          originValue = this.FormatValue(this.GetConvertedValue(originValue, (ValueConfig) valueConfig), (ValueConfig) valueConfig);
          if (!string.IsNullOrEmpty(originValue))
          {
            stringBuilder.Append(valueConfig.SurroundSymbol).Append(originValue).Append(valueConfig.SurroundSymbol);
            if (valueConfig.GroupCond != ConfigFormat.GroupCondType.gctOR)
            {
              if (index < entity.ValueConfigs.Count - 1)
                stringBuilder.Append(valueConfig.Delimiter);
            }
            else
              break;
          }
        }
      }
      this.SetObjAttrValue(stringBuilder.ToString(), entity, entity.ValueConfigs[0], filledValues);
    }));
  }

  private void SetObjAttrValue(
    string value,
    AttrValueCalcEntity entity,
    ValueConfig valueConfig,
    IDictionary<string, string> filledValues)
  {
    if (valueConfig.Base64Encode)
      value = this.Base64Encode(value);
    string key = string.IsNullOrEmpty(valueConfig.AttrName) ? entity.TargetAttrConfig.Name : $"{entity.TargetAttrConfig.Name}.{valueConfig.AttrName}";
    filledValues[key] = value;
    entity.AttrValue.Add(value);
  }

  private string Base64Encode(string source)
  {
    return System.Convert.ToBase64String(Encoding.UTF8.GetBytes(source));
  }

  private string GetConvertedValue(string originValue, ValueConfig valueConfig)
  {
    ValueConverter valueConverter = this._config.ValueConverters[valueConfig.ConvertValueLink.Name];
    return valueConverter == null ? originValue : valueConverter.Convert(originValue, valueConfig.ConvertValueLink.Context);
  }

  private string FormatValue(string originValue, ValueConfig valueConfig)
  {
    switch (valueConfig.ValueDataType)
    {
      case ConfigFormat.AttrValueDataType.avdtDate:
        DateTime result1;
        if (DateTime.TryParse(originValue, out result1))
          return result1.ToString("yyyy-MM-dd");
        break;
      case ConfigFormat.AttrValueDataType.avdtDateTime:
        DateTime result2;
        if (DateTime.TryParse(originValue, out result2))
          return result2.ToString("yyyy-MM-dd\\THH:mm:ss");
        break;
      case ConfigFormat.AttrValueDataType.avdtTime:
        DateTime result3;
        if (DateTime.TryParse(originValue, out result3))
          return result3.ToString("HH:mm:ss");
        break;
    }
    return originValue;
  }

  private string GetUniqueID(IImObject sourceIMObj, IImRelation sourceIMRelation = null)
  {
    string str = sourceIMObj.UniqueID + "_";
    return sourceIMRelation == null ? str : str + sourceIMRelation.UniqueID;
  }

  private string GetObjDesign(IImObject obj)
  {
    string dictAttrKey = ImAttributeType.GetDictAttrKey(this.AT_DESIGNATION.AttributeID.ToString());
    return !obj.Attributes.ContainsKey(dictAttrKey) ? string.Empty : obj.Attributes[dictAttrKey].ToString();
  }

  private string GetMainArtDesign(IImObject obj)
  {
    if (!obj.IsIsp())
      return string.Empty;
    foreach (IXmlRelation objChildRelation in (IEnumerable<IXmlRelation>) this._parser.GetObjChildRelations(obj as IXmlObject))
    {
      if (this._parser.GetRelType(objChildRelation as IImRelation).F_GUID.ToString().Equals("cad00154-306c-11d8-b4e9-00304f19f545", StringComparison.OrdinalIgnoreCase))
      {
        IImObject relChildObj = this._parser.GetRelChildObj(objChildRelation) as IImObject;
        if (obj.IsSBArticle() && relChildObj.IsSpecification() || obj.IsPart() && (relChildObj.IsPartsDrawings() || obj.IsPartWithoutDrawing() && relChildObj.IsModel()))
          return this.GetObjDesign(relChildObj);
      }
    }
    return string.Empty;
  }
}
