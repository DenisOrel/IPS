// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.To1C.IpsTo1CService
// Assembly: Intermech.XmlExchange.IpsXml.Converter.To1C, Version=1.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 449F0722-988D-4220-8C90-DEA703EA2A9B
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.To1C.dll

using Intermech.Interfaces;
using Intermech.Interfaces.TechCard;
using Intermech.IpsXmlViewer.Interfaces;
using Intermech.XmlExchange.IpsXml.Converter.To1C.Resources;
using Intermech.XmlExchange.IpsXml.Interfaces;
using Intermech.XmlExchange.IpsXml.Interfaces.Converter;
using Intermech.XmlExchange.IpsXml.Interfaces.Converter.ConfigTypes;
using Intermech.XmlExchange.IpsXml.Interfaces.Logger;
using Intermech.XmlExchange.IpsXml.Interfaces.Logger.Configuration;
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
using XmlReaderAPI.Data;
using XmlReaderAPI.MetaData;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.To1C;

public class IpsTo1CService
{
  private PluginConfig _config;
  private IpsXmlDataProvider _parser;
  private IpsXmlLogger _logger;
  private IMSAttributeType AT_SUBSTITUTE_GROUP_NUMBER = MetaDataHelper.GetAttributeType(new Guid("cad001c0-306c-11d8-b4e9-00304f19f545"));
  private IMSAttributeType AT_SUBSTITUTE_NUMBER_IN_GROUP = MetaDataHelper.GetAttributeType(new Guid("cad001c1-306c-11d8-b4e9-00304f19f545"));
  private IMSAttributeType AT_PROC_ROUTE_DEFAULT = MetaDataHelper.GetAttributeType(TechCardConsts.AttributeTypes.ProcRouteDefaultAttrGuid);
  private IMSAttributeType AT_MEMBER_OF_SBORKA_OBJECT_ATTRGUID = MetaDataHelper.GetAttributeType(TechCardConsts.AttributeTypes.MemberOfSborkaObjectAttrGUID);
  private IDictionary<string, string> _substitutes = (IDictionary<string, string>) new Dictionary<string, string>();
  private IDictionary<string, IDictionary<string, string>> _allFilledValues = (IDictionary<string, IDictionary<string, string>>) new Dictionary<string, IDictionary<string, string>>();
  private List<List<List<AttrValueCalcEntity>>> _objsPreparedParams = new List<List<List<AttrValueCalcEntity>>>();
  private const string ECO_ATTRIBUTE_ID = "17918";

  public IpsTo1CService(PluginConfig config) => this._config = config;

  public string Convert(string[] fileNames)
  {
    this._allFilledValues.Clear();
    this._substitutes.Clear();
    this._objsPreparedParams.Clear();
    string directoryName = Path.GetDirectoryName(this._config.OutPutFileInfo.Name);
    if (string.IsNullOrEmpty(directoryName))
      directoryName = Path.GetDirectoryName(Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().CodeBase).Path));
    if (string.IsNullOrEmpty(directoryName))
      return string.Empty;
    this._logger = new IpsXmlLogger(Path.Combine(directoryName, "convert.log"));
    this._logger.LoggerConfig.MessageTypes = this._config.LoggerConfig.Infos ? this._logger.LoggerConfig.MessageTypes | LogMessageTypes.Info : this._logger.LoggerConfig.MessageTypes;
    this._logger.LoggerConfig.MessageTypes = this._config.LoggerConfig.Warnings ? this._logger.LoggerConfig.MessageTypes | LogMessageTypes.Warn : this._logger.LoggerConfig.MessageTypes;
    this._logger.LoggerConfig.MessageTypes = this._config.LoggerConfig.Errors ? this._logger.LoggerConfig.MessageTypes | LogMessageTypes.Error : this._logger.LoggerConfig.MessageTypes;
    this._logger.Info(LocalizationHolder.rm.GetString("msgLoadInputData"));
    this._parser = new IpsXmlDataFactory().GetIpsXMlDataProvider(fileNames);
    this._logger.Info($"{LocalizationHolder.rm.GetString("msgOperationComplete")} {LocalizationHolder.rm.GetString("msgLoadInputData")}");
    List<ImObject> list = this._parser.RootObjects.Where<IXmlObject>((Func<IXmlObject, bool>) (headObj => headObj is ImObject targetObj && targetObj.IsArticle())).Select<IXmlObject, ImObject>((Func<IXmlObject, ImObject>) (headObj => headObj as ImObject)).ToList<ImObject>();
    this._logger.Info($"{LocalizationHolder.rm.GetString("msgHeadArtsCount")} ${list.Count}");
    Dictionary<string, IImObject> allSubAssemblies = new Dictionary<string, IImObject>();
    XDocument xdocument = new XDocument();
    this._logger.Info(LocalizationHolder.rm.GetString("msgExportNode") + " reference_list");
    XElement xelement1 = new XElement((XName) "reference_list");
    this.ExportAttrs(xelement1);
    this._logger.Info($"{LocalizationHolder.rm.GetString("msgOperationComplete")} {LocalizationHolder.rm.GetString("msgExportNode")} reference_list");
    this._logger.Info(LocalizationHolder.rm.GetString("msgExportNode") + " title");
    XElement xelement2 = new XElement((XName) "title");
    this.ExportAttrs(xelement2, (IImObject) list.FirstOrDefault<ImObject>());
    xelement1.Add((object) xelement2);
    this._logger.Info($"{LocalizationHolder.rm.GetString("msgOperationComplete")} {LocalizationHolder.rm.GetString("msgExportNode")} title");
    this._logger.Info(LocalizationHolder.rm.GetString("msgExportNode") + " assembly_list");
    XElement xelement3 = new XElement((XName) "assembly_list");
    xelement1.Add((object) xelement3);
    this.ExportAttrs(xelement3);
    this._logger.Info($"{LocalizationHolder.rm.GetString("msgOperationComplete")} {LocalizationHolder.rm.GetString("msgExportNode")} assembly_list");
    this._logger.Info(LocalizationHolder.rm.GetString("msgExportAssemblies"));
    foreach (ImObject assembly in list)
    {
      IReadOnlyCollection<IXmlRelation> objParentRelations = this._parser.GetObjParentRelations((IXmlObject) assembly);
      IImRelation linkToAssembly = (IImRelation) null;
      if (objParentRelations != null)
        linkToAssembly = objParentRelations.First<IXmlRelation>() as IImRelation;
      this.ExportAssembly(xelement3, (XElement) null, (IImXmlObject) assembly, linkToAssembly, (IDictionary<string, IImObject>) allSubAssemblies);
    }
    this._logger.Info($"{LocalizationHolder.rm.GetString("msgOperationComplete")} {LocalizationHolder.rm.GetString("msgExportAssemblies")}");
    this._logger.Info(LocalizationHolder.rm.GetString("msgExportNode") + " item_list");
    XElement xelement4 = new XElement((XName) "item_list");
    this.ExportAttrs(xelement4);
    xelement1.Add((object) xelement4);
    this.ExportItems(xelement4, (IDictionary<string, IImObject>) allSubAssemblies);
    this._logger.Info($"{LocalizationHolder.rm.GetString("msgOperationComplete")} {LocalizationHolder.rm.GetString("msgExportNode")} item_list");
    this._logger.Info(LocalizationHolder.rm.GetString("msgExportNode") + " labor_standards");
    XElement xelement5 = new XElement((XName) "labor_standards");
    this.ExportAttrs(xelement5);
    xelement1.Add((object) xelement5);
    this.ExportLaborStandarts(xelement5);
    xdocument.Add((object) xelement1);
    this._logger.Info($"{LocalizationHolder.rm.GetString("msgOperationComplete")} {LocalizationHolder.rm.GetString("msgExportNode")} labor_standards");
    this._logger.Info(LocalizationHolder.rm.GetString("msgExportPreparedParams"));
    this.ExportPreparedParams();
    this._logger.Info($"{LocalizationHolder.rm.GetString("msgOperationComplete")} {LocalizationHolder.rm.GetString("msgExportPreparedParams")}");
    this._logger.Info(LocalizationHolder.rm.GetString("msgRemovingEmptyItems") + " item initial_item");
    List<XElement> container = new List<XElement>();
    this.CollectAllNodes(xelement1, container);
    container.ForEach((Action<XElement>) (node =>
    {
      if (node.HasElements || !(node.Name == (XName) "initial_item") && !(node.Name == (XName) "item"))
        return;
      node.Remove();
    }));
    this._logger.Info($"{LocalizationHolder.rm.GetString("msgOperationComplete")} {LocalizationHolder.rm.GetString("msgRemovingEmptyItems")} item initial_item");
    string str = this._config.OutPutFileInfo.FileName;
    if (string.IsNullOrEmpty(Path.GetFileName(this._config.OutPutFileInfo.Name)))
      str = Path.Combine(this._config.OutPutFileInfo.Name ?? string.Empty, "XML_IM_TO_1C.xml");
    if (string.IsNullOrEmpty(Path.GetDirectoryName(this._config.OutPutFileInfo.Name)))
      str = Path.Combine(directoryName, str);
    this._logger.Info($"{LocalizationHolder.rm.GetString("msgSaveResultFile")} {str}");
    Encoding encoding;
    try
    {
      encoding = Encoding.GetEncoding(this._config.OutPutFileInfo.Encoding);
    }
    catch (ArgumentException ex)
    {
      encoding = Encoding.UTF8;
    }
    xdocument.Declaration = new XDeclaration(this._config.OutPutFileInfo.Version, encoding.BodyName, (string) null);
    using (XmlTextWriter writer = new XmlTextWriter(str, encoding))
    {
      writer.Formatting = Formatting.Indented;
      xdocument.Save((XmlWriter) writer);
    }
    this._logger.Info($"{LocalizationHolder.rm.GetString("msgOperationComplete")} {LocalizationHolder.rm.GetString("msgSaveResultFile")} {str}");
    this._logger.Close();
    return str;
  }

  private void CollectAllNodes(XElement parent, List<XElement> container)
  {
    container.Add(parent);
    foreach (XElement element in parent.Elements())
      this.CollectAllNodes(element, container);
  }

  private void ExportAssembly(
    XElement parentAssemblyListNode,
    XElement parentAssemblyNode,
    IImXmlObject assembly,
    IImRelation linkToAssembly,
    IDictionary<string, IImObject> allSubAssemblies)
  {
    this._logger.Info($"{LocalizationHolder.rm.GetString("msgExportNode")} assembly: {assembly.F_OBJECT_ID}");
    if (allSubAssemblies.ContainsKey(assembly.F_OBJECT_ID))
    {
      this._logger.Info($"{LocalizationHolder.rm.GetString("msgAlreadyExportedObject")} F_OBJECT_ID: {assembly.F_OBJECT_ID}");
    }
    else
    {
      XElement initialItemListNode = (XElement) null;
      XElement assemblyNode = (XElement) null;
      if (!assembly.IsProcessRoute())
      {
        allSubAssemblies.Add(assembly.F_OBJECT_ID, (IImObject) assembly);
        assemblyNode = new XElement((XName) nameof (assembly));
        this.ExportAttrs(assemblyNode, (IImObject) assembly, linkToAssembly);
        parentAssemblyListNode.Add((object) assemblyNode);
        XElement xelement1 = new XElement((XName) "product_list");
        this.ExportAttrs(xelement1, (IImObject) assembly, linkToAssembly);
        assemblyNode.Add((object) xelement1);
        XElement xelement2 = new XElement((XName) "product");
        this.ExportAttrs(xelement2, (IImObject) assembly, linkToAssembly);
        this.ExportRoutes(xelement2, (IImObject) assembly, linkToAssembly);
        xelement1.Add((object) xelement2);
        initialItemListNode = new XElement((XName) "initial_item_list");
        this.ExportAttrs(initialItemListNode, (IImObject) assembly, linkToAssembly);
        assemblyNode.Add((object) initialItemListNode);
        IImObject ecoII = (IImObject) null;
        if (this.FindEco((IImObject) assembly, out ecoII))
        {
          this._logger.Info($"{LocalizationHolder.rm.GetString("msgExportNode")} change_notice: {ecoII.F_OBJECT_ID}");
          XElement xelement3 = new XElement((XName) "change_notice");
          this.ExportAttrs(xelement3, ecoII, linkToAssembly);
          assemblyNode.Add((object) xelement3);
          XElement xelement4 = new XElement((XName) "plans_list");
          this.ExportAttrs(xelement4, ecoII, linkToAssembly);
          xelement3.Add((object) xelement4);
          this._logger.Info($"{LocalizationHolder.rm.GetString("msgOperationComplete")} {LocalizationHolder.rm.GetString("msgExportNode")}  change_notice: {ecoII.F_OBJECT_ID}");
        }
      }
      else if (parentAssemblyNode != null && assembly.IsProcessRoute())
      {
        assemblyNode = parentAssemblyNode;
        initialItemListNode = parentAssemblyNode.Element((XName) "initial_item_list");
        allSubAssemblies.Add(assembly.F_OBJECT_ID, (IImObject) assembly);
      }
      if (initialItemListNode == null)
        return;
      Dictionary<int, string> mainArtsByGroup = new Dictionary<int, string>();
      Dictionary<int, List<string>> substArtsByGroup = new Dictionary<int, List<string>>();
      string mainZag = string.Empty;
      List<string> substZag = new List<string>();
      this._parser.Traverse((IXmlObject) assembly, (OnFilterObject) ((IXmlObject parentObj, IXmlObject childObj, IXmlRelation relation, ref bool traverse) =>
      {
        if (!(childObj is IImObject targetObj2))
          return false;
        return targetObj2.IsArticle() || targetObj2.IsMaterial() || targetObj2.IsWorkpiece();
      }), (OnVisitObject) ((IXmlObject parentObj, IXmlObject childObj, IXmlRelation relation, ref bool traverse) =>
      {
        if (!(childObj is IImObject imObject2) || !(relation is IImRelation sourceIMRelation2))
          return;
        if (imObject2.IsArticle() || imObject2.IsMaterial())
        {
          if (sourceIMRelation2 == null)
            return;
          int attributeId = this.AT_SUBSTITUTE_GROUP_NUMBER.AttributeID;
          string dictAttrKey1 = ImAttributeType.GetDictAttrKey(attributeId.ToString());
          attributeId = this.AT_SUBSTITUTE_NUMBER_IN_GROUP.AttributeID;
          string dictAttrKey2 = ImAttributeType.GetDictAttrKey(attributeId.ToString());
          if (!sourceIMRelation2.Attributes.ContainsKey(dictAttrKey1) || !sourceIMRelation2.Attributes.ContainsKey(dictAttrKey2))
            return;
          IImAttribute attribute1 = sourceIMRelation2.Attributes[dictAttrKey1] as IImAttribute;
          IImAttribute attribute2 = sourceIMRelation2.Attributes[dictAttrKey2] as IImAttribute;
          int asInt32 = attribute1.GetAsInt32("F_VALUE", 0);
          if (attribute2.GetAsInt32("F_VALUE", 0) == 0)
          {
            mainArtsByGroup[asInt32] = this.GetUniqueID(imObject2, sourceIMRelation2);
          }
          else
          {
            List<string> stringList;
            if (!substArtsByGroup.TryGetValue(asInt32, out stringList))
            {
              stringList = new List<string>();
              substArtsByGroup.Add(asInt32, stringList);
            }
            stringList.Add(this.GetUniqueID(imObject2, sourceIMRelation2));
          }
        }
        else
        {
          if (!imObject2.IsWorkpiece())
            return;
          string dictAttrKey = ImAttributeType.GetDictAttrKey(this.AT_PROC_ROUTE_DEFAULT.AttributeID.ToString());
          if (!imObject2.Attributes.ContainsKey(dictAttrKey))
            substZag.Add(this.GetUniqueID(imObject2, sourceIMRelation2));
          else if (!string.IsNullOrEmpty((imObject2.Attributes[dictAttrKey] as IImAttribute).GetAsString("F_VALUE", string.Empty)))
            mainZag = this.GetUniqueID(imObject2, sourceIMRelation2);
          else
            substZag.Add(this.GetUniqueID(imObject2, sourceIMRelation2));
        }
      }), false);
      foreach (KeyValuePair<int, List<string>> keyValuePair in substArtsByGroup)
      {
        string mainArt;
        if (mainArtsByGroup.TryGetValue(keyValuePair.Key, out mainArt))
          keyValuePair.Value.ForEach((Action<string>) (substitute => this._substitutes[substitute] = mainArt));
      }
      if (!string.IsNullOrEmpty(mainZag))
        substZag.ForEach((Action<string>) (substitute => this._substitutes[substitute] = mainZag));
      List<(IImObject, IImRelation)> procRouteInfos = new List<(IImObject, IImRelation)>();
      List<(IImObject, IImRelation)> subAssemblyInfos = new List<(IImObject, IImRelation)>();
      this._parser.Traverse((IXmlObject) assembly, (OnFilterObject) ((IXmlObject parentObj, IXmlObject childObj, IXmlRelation relation, ref bool traverse) => parentObj != null || childObj != assembly), (OnVisitObject) ((IXmlObject parentObj, IXmlObject childObj, IXmlRelation relation, ref bool traverse) =>
      {
        if (!(childObj is ImObject imObject4) || !(relation is ImRelation sourceIMRelation4))
          return;
        if (this.CanUseObjInAssembly((IImObject) imObject4))
        {
          XElement xelement = new XElement((XName) "initial_item");
          this.ExportAttrs(xelement, (IImObject) imObject4, (IImRelation) sourceIMRelation4);
          initialItemListNode.Add((object) xelement);
        }
        if (imObject4.IsArticle())
        {
          if (allSubAssemblies.ContainsKey(imObject4.F_OBJECT_ID))
            return;
          subAssemblyInfos.Add(((IImObject) imObject4, (IImRelation) sourceIMRelation4));
        }
        else
        {
          if (!imObject4.IsProcessRoute())
            return;
          procRouteInfos.Add(((IImObject) imObject4, (IImRelation) sourceIMRelation4));
        }
      }), false);
      if (procRouteInfos.Count > 0)
      {
        (IImObject ProcRoute, IImRelation LinkToProcRoute) tuple = this.FilterProcRoute((IEnumerable<(IImObject, IImRelation)>) procRouteInfos, linkToAssembly);
        if (tuple.ProcRoute != null)
          subAssemblyInfos.Add(tuple);
      }
      subAssemblyInfos.ForEach((Action<(IImObject, IImRelation)>) (subAssemblyInfo => this.ExportAssembly(parentAssemblyListNode, assemblyNode, subAssemblyInfo.SubAssembly as IImXmlObject, subAssemblyInfo.LinkToSubAssembly, allSubAssemblies)));
      this._logger.Info($"{LocalizationHolder.rm.GetString("msgOperationComplete")} {LocalizationHolder.rm.GetString("msgExportNode")}  assembly: {assembly.F_OBJECT_ID}");
    }
  }

  private bool CanUseObjInAssembly(IImObject obj)
  {
    return !obj.IsRoute() && !obj.IsOper() && !obj.IsProcessRoute() && !obj.IsProcessRouteEntry();
  }

  private bool CanUseObjInItemList(IImObject obj) => this.CanUseObjInAssembly(obj);

  private bool FindEco(IImObject source, out IImObject ecoII)
  {
    ecoII = (IImObject) null;
    string dictAttrKey = ImAttributeType.GetDictAttrKey("17918");
    object obj = (object) null;
    if (!source.Attributes.TryGetValue(dictAttrKey, out obj))
      return false;
    string asString = (obj as IImAttribute).GetAsString("F_INTEGER_VALUE", string.Empty);
    if (string.IsNullOrEmpty(asString))
      return false;
    ecoII = (IImObject) this._parser.FindObjectById(asString);
    return asString != null;
  }

  private void ExportRoutes(XElement productNode, IImObject assembly, IImRelation linkToAssembly)
  {
    NodeConfig nodeConfig = this._config.NodeConfigs["route_table"];
    if (nodeConfig == null || !nodeConfig.Export)
      return;
    (IImObject Route, IImRelation LinkToRoute) routeByVhod = this.FindRouteByVhod(assembly, linkToAssembly);
    if (routeByVhod.Route == null)
      return;
    List<XElement> routeElemsList = new List<XElement>();
    IReadOnlyCollection<IXmlRelation> objChildRelations = this._parser.GetObjChildRelations(routeByVhod.Route as IXmlObject);
    if (objChildRelations == null)
      return;
    objChildRelations.Where<IXmlRelation>((Func<IXmlRelation, bool>) (child => this._parser.GetRelChildObj(child) is IImObject relChildObj && relChildObj.IsRouteElem())).ToList<IXmlRelation>().ForEach((Action<IXmlRelation>) (linkToRouteElem =>
    {
      XElement targetNode = new XElement((XName) "route_element");
      this.ExportAttrs(targetNode, this._parser.GetRelChildObj(linkToRouteElem) as IImObject, linkToRouteElem as IImRelation);
      routeElemsList.Add(targetNode);
    }));
    if (routeElemsList.Count <= 0)
      return;
    XElement xelement = new XElement((XName) "route_table");
    this.ExportAttrs(xelement, routeByVhod.Route, routeByVhod.LinkToRoute);
    foreach (XElement content in routeElemsList)
      xelement.Add((object) content);
    productNode.Add((object) xelement);
  }

  private (IImObject ProcRoute, IImRelation LinkToProcRoute) FilterProcRoute(
    IEnumerable<(IImObject ProcRoute, IImRelation LinkToProcRoute)> procRoutesToFilter,
    IImRelation linkToParentAssembly)
  {
    this._logger.Info(LocalizationHolder.rm.GetString("msgProcessRoutesFiltering"));
    if (linkToParentAssembly == null)
    {
      List<(IImObject, IImRelation)> list = procRoutesToFilter.ToList<(IImObject, IImRelation)>();
      (IImObject, IImRelation) valueTuple = list.Count == 1 ? list.First<(IImObject, IImRelation)>() : list.Where<(IImObject, IImRelation)>((Func<(IImObject, IImRelation), bool>) (procRoute => procRoute.ProcRoute.IsDefaultProcRoute())).FirstOrDefault<(IImObject, IImRelation)>();
      if (valueTuple.Item1 == null)
        valueTuple = list.First<(IImObject, IImRelation)>();
      if (valueTuple.Item1 != null)
        this._logger.Info($"{LocalizationHolder.rm.GetString("msgProcessHeadRouteFound")} F_OBJECT_ID = {valueTuple.Item1.F_OBJECT_ID}");
      else
        this._logger.Info(LocalizationHolder.rm.GetString("msgProcessHeadRouteNotFound"));
      return valueTuple;
    }
    if (!(this._parser.GetRelParentObj(linkToParentAssembly as IXmlRelation) is IImObject relParentObj))
    {
      this._logger.Info($"{LocalizationHolder.rm.GetString("msgParentNotFoundForLink")} {"F_GUID"} = {linkToParentAssembly.F_GUID}");
      return ((IImObject) null, (IImRelation) null);
    }
    if (relParentObj.IsEcoII())
    {
      List<(IImObject, IImRelation)> list = procRoutesToFilter.ToList<(IImObject, IImRelation)>();
      (IImObject, IImRelation) valueTuple = list.Count == 1 ? list.First<(IImObject, IImRelation)>() : list.Where<(IImObject, IImRelation)>((Func<(IImObject, IImRelation), bool>) (procRoute => procRoute.ProcRoute.IsDefaultProcRoute())).FirstOrDefault<(IImObject, IImRelation)>();
      if (valueTuple.Item1 == null)
        valueTuple = list.First<(IImObject, IImRelation)>();
      if (valueTuple.Item1 != null)
        this._logger.Info($"{LocalizationHolder.rm.GetString("msgProcessHeadRouteFound")} F_OBJECT_ID = {valueTuple.Item1.F_OBJECT_ID}");
      else
        this._logger.Info(LocalizationHolder.rm.GetString("msgProcessHeadRouteNotFound"));
      return valueTuple;
    }
    List<(IImObject, IImRelation)> source1 = new List<(IImObject, IImRelation)>();
    List<(IImObject, IImRelation)> source2 = new List<(IImObject, IImRelation)>();
    foreach ((IImObject ProcRoute, IImRelation LinkToProcRoute) tuple in procRoutesToFilter)
    {
      switch (this.IsProcRouteFilteredByVhod(tuple.ProcRoute, relParentObj.F_OBJECT_ID))
      {
        case FilterByVhodResult.FoundVhod:
          source1.Add(tuple);
          continue;
        case FilterByVhodResult.NoVhodSpecified:
          source2.Add(tuple);
          continue;
        default:
          continue;
      }
    }
    this._logger.Info(LocalizationHolder.rm.GetString("msgFindDefaultPROnVhod"));
    (IImObject, IImRelation) valueTuple1 = source1.Count == 1 ? source1.First<(IImObject, IImRelation)>() : source1.Where<(IImObject, IImRelation)>((Func<(IImObject, IImRelation), bool>) (procRouteInfo => procRouteInfo.ProcRoute.IsDefaultProcRoute())).FirstOrDefault<(IImObject, IImRelation)>();
    if (valueTuple1.Item1 == null && source1.Count > 0)
    {
      this._logger.Info(LocalizationHolder.rm.GetString("msgFindFirstPROnVhod"));
      valueTuple1 = source1.FirstOrDefault<(IImObject, IImRelation)>();
    }
    if (valueTuple1.Item1 == null)
    {
      this._logger.Info(LocalizationHolder.rm.GetString("msgFindDefalutPRWithoutVhod"));
      valueTuple1 = source2.Count == 1 ? source2.First<(IImObject, IImRelation)>() : source2.Where<(IImObject, IImRelation)>((Func<(IImObject, IImRelation), bool>) (procRouteInfo => procRouteInfo.ProcRoute.IsDefaultProcRoute())).FirstOrDefault<(IImObject, IImRelation)>();
      if (valueTuple1.Item1 == null && source2.Count > 0)
      {
        this._logger.Info(LocalizationHolder.rm.GetString("msgFindFirstPRWithoutVhod"));
        valueTuple1 = source2.FirstOrDefault<(IImObject, IImRelation)>();
      }
    }
    this._logger.Info($"{LocalizationHolder.rm.GetString("msgOperationComplete")} {LocalizationHolder.rm.GetString("msgProcessRoutesFiltering")}");
    if (valueTuple1.Item1 != null)
      this._logger.Info($"{LocalizationHolder.rm.GetString("msgProcessRouteFound")} F_OBJECT_ID = {valueTuple1.Item1.F_OBJECT_ID}");
    else
      this._logger.Info(LocalizationHolder.rm.GetString("msgProcessRouteNotFound") ?? "");
    return valueTuple1;
  }

  private (IImObject Route, IImRelation LinkToRoute) FindRouteByVhod(
    IImObject assembly,
    IImRelation linkToAssembly)
  {
    this._logger.Info(LocalizationHolder.rm.GetString("msgRoutesSearching"));
    IReadOnlyCollection<IXmlRelation> objChildRelations1 = this._parser.GetObjChildRelations(assembly as IXmlObject);
    if (objChildRelations1 == null)
      return ((IImObject) null, (IImRelation) null);
    IEnumerable<IXmlObject> source1 = objChildRelations1.Where<IXmlRelation>((Func<IXmlRelation, bool>) (child => this._parser.GetRelChildObj(child) is IImObject relChildObj1 && relChildObj1.IsProcessRoute())).Select<IXmlRelation, IXmlObject>((Func<IXmlRelation, IXmlObject>) (child => this._parser.GetRelChildObj(child)));
    string parentId = string.Empty;
    if (linkToAssembly != null)
      parentId = this._parser.GetRelParentObj(linkToAssembly as IXmlRelation) is IImObject relParentObj ? relParentObj.F_OBJECT_ID : (string) null;
    IImObject imObject1;
    if (string.IsNullOrEmpty(parentId) || assembly.IsHead())
    {
      List<IXmlObject> list = source1.ToList<IXmlObject>();
      imObject1 = list.Count == 1 ? list.First<IXmlObject>() as IImObject : (list.Where<IXmlObject>((Func<IXmlObject, bool>) (procRoute => procRoute is IImObject targetObj && targetObj.IsDefaultProcRoute())).FirstOrDefault<IXmlObject>() is IImObject imObject2 ? imObject2 : list.FirstOrDefault<IXmlObject>() as IImObject);
      if (imObject1 != null)
        this._logger.Info($"{LocalizationHolder.rm.GetString("msgProcessHeadRouteFound")} F_OBJECT_ID = {imObject1.F_OBJECT_ID}");
      else
        this._logger.Info(LocalizationHolder.rm.GetString("msgProcessHeadRouteNotFound"));
    }
    else
    {
      List<IImObject> source2 = new List<IImObject>();
      List<IImObject> source3 = new List<IImObject>();
      foreach (IXmlObject procRoute in source1)
      {
        switch (this.IsProcRouteFilteredByVhod(procRoute as IImObject, parentId))
        {
          case FilterByVhodResult.FoundVhod:
            source2.Add(procRoute as IImObject);
            continue;
          case FilterByVhodResult.NoVhodSpecified:
            source3.Add(procRoute as IImObject);
            continue;
          default:
            continue;
        }
      }
      this._logger.Info(LocalizationHolder.rm.GetString("msgFindDefaultPROnVhod"));
      imObject1 = source2.Count == 1 ? source2.First<IImObject>() : source2.Where<IImObject>((Func<IImObject, bool>) (procRoute => procRoute.IsDefaultProcRoute())).FirstOrDefault<IImObject>() ?? source2.FirstOrDefault<IImObject>();
      if (imObject1 == null)
      {
        this._logger.Info(LocalizationHolder.rm.GetString("msgFindDefalutPRWithoutVhod"));
        imObject1 = source3.Count == 1 ? source3.First<IImObject>() : source3.Where<IImObject>((Func<IImObject, bool>) (procRoute => procRoute.IsDefaultProcRoute())).FirstOrDefault<IImObject>() ?? source3.FirstOrDefault<IImObject>();
      }
    }
    if (imObject1 == null)
    {
      this._logger.Info(LocalizationHolder.rm.GetString("msgPRNotFound"));
      return ((IImObject) null, (IImRelation) null);
    }
    IReadOnlyCollection<IXmlRelation> objChildRelations2 = this._parser.GetObjChildRelations(imObject1 as IXmlObject);
    if (objChildRelations2 == null)
    {
      this._logger.Info(LocalizationHolder.rm.GetString("msgEmptyPR"));
      return ((IImObject) null, (IImRelation) null);
    }
    IXmlRelation rel = objChildRelations2.Where<IXmlRelation>((Func<IXmlRelation, bool>) (link => this._parser.GetRelChildObj(link) is IImObject relChildObj2 && relChildObj2.IsRoute())).FirstOrDefault<IXmlRelation>();
    this._logger.Info($"{LocalizationHolder.rm.GetString("msgOperationComplete")} {LocalizationHolder.rm.GetString("msgRoutesSearching")}");
    if (rel != null)
    {
      IImObject relChildObj3 = this._parser.GetRelChildObj(rel) as IImObject;
      this._logger.Info($"{LocalizationHolder.rm.GetString("msgRouteFound")} F_OBJECT_ID = {relChildObj3.F_OBJECT_ID}");
      return (relChildObj3, rel as IImRelation);
    }
    this._logger.Info(LocalizationHolder.rm.GetString("msgRouteNotFound"));
    return ((IImObject) null, (IImRelation) null);
  }

  private FilterByVhodResult IsProcRouteFilteredByVhod(IImObject procRoute, string parentId)
  {
    IReadOnlyCollection<IXmlRelation> objChildRelations = this._parser.GetObjChildRelations(procRoute as IXmlObject);
    if (objChildRelations == null)
      return FilterByVhodResult.NoVhodSpecified;
    List<IImObject> list = objChildRelations.Select(linkToChild => new
    {
      linkToChild = linkToChild,
      childObj = this._parser.GetRelChildObj(linkToChild) as IImObject
    }).Where(_param1 => _param1.childObj != null && _param1.childObj.IsProcessRouteEntry()).Select(_param1 => _param1.childObj).ToList<IImObject>();
    if (list.Count == 0)
      return FilterByVhodResult.NoVhodSpecified;
    foreach (IImObject imObject in list)
    {
      string dictAttrKey = ImAttributeType.GetDictAttrKey(this.AT_MEMBER_OF_SBORKA_OBJECT_ATTRGUID.AttributeID.ToString());
      if (imObject.Attributes.ContainsKey(dictAttrKey))
      {
        foreach (IDictionary<string, object> dictionary in (IEnumerable<IDictionary<string, object>>) (procRoute.Attributes[dictAttrKey] as IImAttribute).DeNormalize().Values)
        {
          object obj;
          if (dictionary.TryGetValue("F_VALUE", out obj) && parentId == (string) obj)
            return FilterByVhodResult.FoundVhod;
        }
      }
    }
    return FilterByVhodResult.NotFoundVhod;
  }

  private void ExportItems(
    XElement parentItemListNode,
    IDictionary<string, IImObject> allSubAssemblies)
  {
    HashSet<string> allExportedItems = new HashSet<string>();
    foreach (IImObject startFromObj in (IEnumerable<IImObject>) allSubAssemblies.Values)
      this._parser.Traverse(startFromObj as IXmlObject, (OnFilterObject) ((IXmlObject parentObj, IXmlObject childObj, IXmlRelation relation, ref bool traverse) => true), (OnVisitObject) ((IXmlObject parentObj, IXmlObject childObj, IXmlRelation relation, ref bool traverse) =>
      {
        if (!(childObj is IImObject sourceIMObj2) || allExportedItems.Contains(sourceIMObj2.F_OBJECT_ID) || !this.CanUseObjInItemList(sourceIMObj2))
          return;
        XElement xelement = new XElement((XName) "item");
        this.ExportAttrs(xelement, sourceIMObj2, relation as IImRelation);
        parentItemListNode.Add((object) xelement);
        allExportedItems.Add(sourceIMObj2.F_OBJECT_ID);
      }), false);
  }

  private void ExportLaborStandarts(XElement laborStandartsNode)
  {
    foreach (IXmlObject allObject in (IEnumerable<IXmlObject>) this._parser.GetAllObjects())
    {
      if (allObject is IImObject imObject && imObject.IsOper())
      {
        IReadOnlyCollection<IXmlRelation> objChildRelations = this._parser.GetObjChildRelations(imObject as IXmlObject);
        if (objChildRelations == null)
        {
          XElement xelement = new XElement((XName) "labor_standard");
          laborStandartsNode.Add((object) xelement);
          this.ExportAttrs(xelement, imObject);
        }
        else
        {
          List<(IImObject, IImRelation)> list = objChildRelations.Select(linkToChild => new
          {
            linkToChild = linkToChild,
            child = this._parser.GetRelChildObj(linkToChild) as IImObject
          }).Where(_param1 => _param1.child.IsPersonal()).Select(_param1 => (_param1.child, _param1.linkToChild as IImRelation)).ToList<(IImObject, IImRelation)>();
          if (list.Count == 0)
          {
            XElement xelement = new XElement((XName) "labor_standard");
            laborStandartsNode.Add((object) xelement);
            this.ExportAttrs(xelement, imObject);
          }
          else
          {
            foreach ((IImObject sourceIMObj, IImRelation sourceIMRelation) in list)
            {
              XElement xelement = new XElement((XName) "labor_standard");
              laborStandartsNode.Add((object) xelement);
              this.ExportAttrs(xelement, imObject);
              this.ExportAttrs(xelement, sourceIMObj, sourceIMRelation);
            }
          }
        }
      }
    }
  }

  private void ExportAttrs(
    XElement targetNode,
    IImObject sourceIMObj = null,
    IImRelation sourceIMRelation = null)
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
    string context = sourceIMObj != null ? this.FindCompatibleConfigContextForObject(sourceIMObj, nodeConfig.AttrConfigs.contexts) : string.Empty;
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
            if (attrConfig.Context == MetaDataHelper.GetObjectTypeName(TechCardConsts.ObjectTypes.CehRouteGUID))
            {
              (IImObject Route, IImRelation LinkToRoute) routeByVhod = this.FindRouteByVhod(sourceIMObj, sourceIMRelation);
              if (routeByVhod.Route == null)
                return;
              attrValueCalcEntity = new AttrValueCalcEntity(attrConfig, targetNode, routeByVhod.Route, routeByVhod.LinkToRoute);
              attrValueCalcEntity.ValueConfigs.Add(valueConfig);
              IMParamsEntities.Add(attrValueCalcEntity);
              break;
            }
            attrValueCalcEntity = new AttrValueCalcEntity(attrConfig, targetNode, sourceIMObj, sourceIMRelation);
            attrValueCalcEntity.ValueConfigs.Add(valueConfig);
            IMParamsEntities.Add(attrValueCalcEntity);
            break;
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
    }), context, (Predicate<AttrConfig>) (childConfig => string.IsNullOrEmpty(context) || childConfig.Context.Equals(context) || childConfig.Context.Equals(MetaDataHelper.GetObjectTypeName(TechCardConsts.ObjectTypes.CehRouteGUID))));
    this.CalcSimpleEntities(simpleEntities, filledValues);
    this.CalcFixedFuncEntities(fixedFuncEntities, filledValues);
    this.CalcIMParamsEntities(IMParamsEntities, filledValues);
    Dictionary<ValueConfig, AttrValueCalcEntity> dictionary = new Dictionary<ValueConfig, AttrValueCalcEntity>();
    this.CalcLocalEntities(localEntities, filledValues, dictionary);
    this.CalcGrouppedLocalEntities(grouppedLocalEntities, filledValues, dictionary);
    this.CalcLastEntities(dictionary, filledValues);
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
      XElement curChildAttr = (XElement) null;
      Dictionary<string, AttrValueCalcEntity> comparisonValues = new Dictionary<string, AttrValueCalcEntity>();
      XElement attrOwnerNode = (XElement) null;
      objParams.ForEach((Action<List<AttrValueCalcEntity>>) (attrParams =>
      {
        attrParams.Sort((Comparison<AttrValueCalcEntity>) ((left, right) => left.ValueConfigs[0].Order - right.ValueConfigs[0].Order));
        attrParams.ForEach((Action<AttrValueCalcEntity>) (preparedParam =>
        {
          if (attrOwnerNode == null)
            attrOwnerNode = preparedParam.AttrOwnerNode;
          IDictionary<string, string> filledValues;
          if (preparedParam.AttrValue.Count == 0 && preparedParam.ValueConfigs[0].ValueType == ConfigFormat.AttrValueType.avtSubstitute && this._allFilledValues.TryGetValue(this.GetUniqueID(preparedParam.SourceIMObject, preparedParam.SourceIMRelation), out filledValues))
            this.CalcSubstituteEntitity(preparedParam, filledValues);
          preparedParam.AttrValue.ForEach((Action<string>) (attrValue =>
          {
            if (preparedParam.TargetAttrConfig.ForComparison)
            {
              comparisonValues[preparedParam.TargetAttrConfig.Name] = preparedParam;
            }
            else
            {
              if (!preparedParam.TargetAttrConfig.Export || string.IsNullOrEmpty(attrValue))
                return;
              XElement content = (XElement) null;
              if (preparedParam.AttrValue.Count == 1)
                content = attrOwnerNode.Element((XName) preparedParam.TargetAttrConfig.Name);
              if (content == null)
              {
                content = new XElement((XName) preparedParam.TargetAttrConfig.Name);
                if (curChildAttr == null)
                {
                  attrOwnerNode.AddFirst((object) content);
                  curChildAttr = content;
                }
                else
                {
                  curChildAttr.AddAfterSelf((object) content);
                  curChildAttr = content;
                }
              }
              if (string.IsNullOrEmpty(preparedParam.ValueConfigs[0].AttrName))
              {
                content.Value = attrValue;
              }
              else
              {
                XAttribute xattribute = content.Attribute((XName) preparedParam.ValueConfigs[0].AttrName);
                if (xattribute == null)
                  content.Add((object) new XAttribute((XName) preparedParam.ValueConfigs[0].AttrName, (object) attrValue));
                else
                  xattribute.Value = attrValue;
              }
            }
          }));
        }));
      }));
      if (comparisonValues.Count <= 0 || attrOwnerNode == null)
        return;
      List<AttrValueCalcEntity> list = comparisonValues.Values.ToList<AttrValueCalcEntity>();
      list.Sort((Comparison<AttrValueCalcEntity>) ((left, right) => left.ValueConfigs[0].Order - right.ValueConfigs[0].Order));
      this.ExportComparisonValues(attrOwnerNode, list);
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
    if (objectType1 == null)
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
      switch (valueConfig.FuncType)
      {
        case ConfigFormat.FixedFuncType.fftCurDate:
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
          break;
        case ConfigFormat.FixedFuncType.fftAssemblyInfo:
          this.SetObjAttrValue(Assembly.GetExecutingAssembly().GetName().Version.ToString(), entity, (ValueConfig) valueConfig, filledValues);
          break;
      }
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
        IpsTo1CService.GetAttrValues(attr, valueConfig, values);
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
                IpsTo1CService.GetAttrValues(attribute3, valueConfig, values);
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
                IpsTo1CService.GetAttrValues(entity.SourceIMObject.Attributes[dictAttrKey] as IImAttribute, valueConfig, values);
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
                IpsTo1CService.GetAttrValues(attribute4, valueConfig, values);
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
                IpsTo1CService.GetAttrValues(entity.SourceIMRelation.Attributes[dictAttrKey] as IImAttribute, valueConfig, values);
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

  private void CalcSubstituteEntitity(
    AttrValueCalcEntity entity,
    IDictionary<string, string> filledValues)
  {
    SubstituteValueConfig valueConfig = entity.ValueConfigs[0] as SubstituteValueConfig;
    string key;
    IDictionary<string, string> dictionary;
    string originValue;
    if (!this._substitutes.TryGetValue(this.GetUniqueID(entity.SourceIMObject, entity.SourceIMRelation), out key) || !this._allFilledValues.TryGetValue(key, out dictionary) || !dictionary.TryGetValue(valueConfig.LocalSourceAttrName, out originValue))
      return;
    string str = this.FormatValue(this.GetConvertedValue(originValue, (ValueConfig) valueConfig), (ValueConfig) valueConfig);
    if (string.IsNullOrEmpty(str))
      return;
    this.SetObjAttrValue(valueConfig.SurroundSymbol + str + valueConfig.SurroundSymbol, entity, (ValueConfig) valueConfig, filledValues);
  }

  private void CalcLocalEntities(
    List<AttrValueCalcEntity> localEntities,
    IDictionary<string, string> filledValues,
    Dictionary<ValueConfig, AttrValueCalcEntity> calcLastEntities)
  {
    localEntities.ForEach((Action<AttrValueCalcEntity>) (entity =>
    {
      LocalValueConfig valueConfig = entity.ValueConfigs[0] as LocalValueConfig;
      string originValue;
      if (!filledValues.TryGetValue(valueConfig.LocalAttrName, out originValue))
      {
        if (calcLastEntities.ContainsKey((ValueConfig) valueConfig))
          return;
        calcLastEntities.Add((ValueConfig) valueConfig, entity);
      }
      else
      {
        string str = this.FormatValue(this.GetConvertedValue(originValue, (ValueConfig) valueConfig), (ValueConfig) valueConfig);
        if (string.IsNullOrEmpty(str))
          return;
        this.SetObjAttrValue(valueConfig.SurroundSymbol + str + valueConfig.SurroundSymbol, entity, (ValueConfig) valueConfig, filledValues);
      }
    }));
  }

  private void CalcGrouppedLocalEntities(
    List<AttrValueCalcEntity> grouppedLocalEntities,
    IDictionary<string, string> filledValues,
    Dictionary<ValueConfig, AttrValueCalcEntity> calcLastEntities)
  {
    grouppedLocalEntities.ForEach((Action<AttrValueCalcEntity>) (entity =>
    {
      if (entity.ValueConfigs.Count == 0)
        return;
      bool flag = true;
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < entity.ValueConfigs.Count; ++index)
      {
        LocalValueConfig valueConfig = entity.ValueConfigs[index] as LocalValueConfig;
        string originValue;
        if (!filledValues.TryGetValue(valueConfig.LocalAttrName, out originValue))
        {
          flag = false;
        }
        else
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
      if (entity.AttrValue.Count != 0 || flag || calcLastEntities.ContainsKey(entity.ValueConfigs[0]))
        return;
      calcLastEntities.Add(entity.ValueConfigs[0], entity);
    }));
  }

  private bool CalcLastEntities(
    Dictionary<ValueConfig, AttrValueCalcEntity> calcEntities,
    IDictionary<string, string> filledValues)
  {
    Stack<string> calcStack = new Stack<string>();
    List<AttrValueCalcEntity> list = calcEntities.Values.ToList<AttrValueCalcEntity>();
    for (int index = list.Count - 1; index >= 0; --index)
    {
      calcStack.Clear();
      if (this.CalcLastEntity(list[index], filledValues, calcStack, list))
      {
        calcEntities.Remove(list[index].ValueConfigs[0]);
        list.RemoveAt(index);
      }
    }
    return false;
  }

  private bool CalcLastEntity(
    AttrValueCalcEntity entity,
    IDictionary<string, string> filledValues,
    Stack<string> calcStack,
    List<AttrValueCalcEntity> calcList)
  {
    if (calcStack.Contains(entity.ValueConfigs[0].Name))
      return false;
    calcStack.Push(entity.ValueConfigs[0].Name);
    try
    {
      bool flag = true;
      foreach (ValueConfig valueConfig in (IEnumerable<ValueConfig>) entity.ValueConfigs)
      {
        LocalValueConfig localValueConfig;
        if ((localValueConfig = valueConfig as LocalValueConfig) != null)
        {
          string originValue;
          if (!filledValues.TryGetValue(localValueConfig.LocalAttrName, out originValue))
          {
            AttrValueCalcEntity entity1 = calcList.Where<AttrValueCalcEntity>((Func<AttrValueCalcEntity, bool>) (calEnt => calEnt.TargetAttrConfig.Name == localValueConfig.LocalAttrName)).FirstOrDefault<AttrValueCalcEntity>();
            if (entity1 == null)
            {
              flag = false;
              continue;
            }
            this.CalcLastEntity(entity1, filledValues, calcStack, calcList);
            if (!filledValues.TryGetValue(localValueConfig.LocalAttrName, out originValue))
            {
              flag = false;
              return false;
            }
          }
          originValue = this.FormatValue(this.GetConvertedValue(originValue, (ValueConfig) localValueConfig), (ValueConfig) localValueConfig);
          if (!string.IsNullOrEmpty(originValue))
          {
            entity.AttrValue.Clear();
            originValue = localValueConfig.SurroundSymbol + originValue + localValueConfig.SurroundSymbol;
            this.SetObjAttrValue(originValue, entity, valueConfig, filledValues);
          }
        }
      }
      return flag;
    }
    finally
    {
      calcStack.Pop();
    }
  }

  private void ExportComparisonValues(
    XElement targetNode,
    List<AttrValueCalcEntity> comparisonValues)
  {
    if (comparisonValues.Count == 0)
      return;
    XElement comparisonListNode = new XElement((XName) "comparison_list");
    targetNode.Add((object) comparisonListNode);
    comparisonValues.ForEach((Action<AttrValueCalcEntity>) (entity => entity.AttrValue.ForEach((Action<string>) (value =>
    {
      XElement content = new XElement((XName) "comparison");
      content.Add((object) new XAttribute((XName) "destination", (object) entity.TargetAttrConfig.Name));
      content.Value = value;
      comparisonListNode.Add((object) content);
    }))));
  }

  private void SetObjAttrValue(
    string value,
    AttrValueCalcEntity entity,
    ValueConfig valueConfig,
    IDictionary<string, string> filledValues)
  {
    entity.AttrValue.Add(value);
    string key = string.IsNullOrEmpty(valueConfig.AttrName) ? entity.TargetAttrConfig.Name : $"{entity.TargetAttrConfig.Name}.{valueConfig.AttrName}";
    if (entity.AttrValue.Count > 1)
    {
      if (entity.AttrValue.Count == 2)
        filledValues[key + ".0"] = filledValues[key];
      key = $"{key}.{(entity.AttrValue.Count - 1).ToString()}";
    }
    filledValues[key] = value;
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
}
