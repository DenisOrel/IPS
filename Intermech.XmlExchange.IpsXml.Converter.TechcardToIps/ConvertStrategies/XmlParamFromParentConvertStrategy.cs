// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConvertStrategies.XmlParamFromParentConvertStrategy
// Assembly: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E8F758AE-29ED-44FC-8EF9-3A977263FAEF
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.dll

using Intermech.Interfaces;
using Intermech.IpsXmlViewer.Interfaces;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.StrategyParams;
using Intermech.XmlExchange.IpsXml.Interfaces;
using Intermech.XmlExchange.IpsXml.Provider.Ips.Serializer;
using Intermech.XmlExchange.IpsXml.Provider.Techcard.Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using XmlReaderAPI.Common;
using XmlReaderAPI.Data;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConvertStrategies;

internal class XmlParamFromParentConvertStrategy : XmlParamConvertStrategy
{
  public override XmlStrategyConvertResultType Convert()
  {
    if (!Debugger.IsAttached)
      Debugger.Launch();
    Debugger.Break();
    if (this.StrategyParams == null)
      return XmlStrategyConvertResultType.MinorError;
    object obj;
    if (!this.StrategyParams.TryGetValue(AddStrategyParamType.ConvertedParamOwner, out obj) || !(obj is ImDataElement))
    {
      this.Logger.Error("Не указана конвертированная сущность-владелец параметра");
      return XmlStrategyConvertResultType.MinorError;
    }
    ImDataElement target = obj as ImDataElement;
    if (!this.StrategyParams.TryGetValue(AddStrategyParamType.ParamOwner, out obj) || !(obj is IXmlEntity))
    {
      this.Logger.Error("Не указана сущность-владелец параметра");
      return XmlStrategyConvertResultType.MinorError;
    }
    IXmlObject StartFrom = obj as IXmlObject;
    if (!this.StrategyParams.TryGetValue(AddStrategyParamType.ConvertedOwnerParamsCache, out obj) || !(obj is ParamsCache))
    {
      this.Logger.Error("Не указан кэш конвертированных параметров");
      return XmlStrategyConvertResultType.MinorError;
    }
    ParamsCache paramsCache = obj as ParamsCache;
    ParamConfig targetConfig = this.TargetConfig as ParamConfig;
    string str;
    if (this.FindSourceParamValue(StartFrom, Intermech.XmlExchange.IpsXml.Provider.Techcard.Format.NodeType.Art, "Обозначение", out str))
    {
      ImAttribute attr = new ImAttribute();
      attr.SetAsString("F_ATTRIBUTE_ID", System.Convert.ToString(MetaDataHelper.GetAttributeTypeID("cad0001f-306c-11d8-b4e9-00304f19f545")));
      attr.SetAsString("F_VALUE", str);
      attr.SetAsString("F_STRING_VALUE", str);
      this.GlobalServices.GetService<IpsDataSerializer>().AddAttribute((IImDataElement) target, attr);
      paramsCache[this.GetCacheParamId(targetConfig.Id, string.Empty)] = str;
    }
    return XmlStrategyConvertResultType.Converted;
  }

  private bool FindSourceParamValue(
    IXmlObject StartFrom,
    Intermech.XmlExchange.IpsXml.Provider.Techcard.Format.NodeType ParentType,
    string ParamName,
    out string Value)
  {
    Value = string.Empty;
    IXmlDataProvider service = this.GlobalServices.GetService<IXmlDataProvider>();
    IReadOnlyCollection<IXmlRelation> objParentRelations = service.GetObjParentRelations(StartFrom);
    if (objParentRelations == null || objParentRelations.Count == 0)
      return false;
    foreach (IXmlRelation rel in (IEnumerable<IXmlRelation>) objParentRelations)
    {
      IXmlObject relParentObj = service.GetRelParentObj(rel);
      if ((relParentObj as TechXmlObject).NodeType == ParentType)
      {
        Value = relParentObj.XmlParams.Where<IXmlParam>((Func<IXmlParam, bool>) (param => param.Name == ParamName)).Select<IXmlParam, string>((Func<IXmlParam, string>) (param => param.Value)).FirstOrDefault<string>() ?? string.Empty;
        return true;
      }
      if (this.FindSourceParamValue(relParentObj, ParentType, ParamName, out Value))
        return true;
    }
    return false;
  }
}
