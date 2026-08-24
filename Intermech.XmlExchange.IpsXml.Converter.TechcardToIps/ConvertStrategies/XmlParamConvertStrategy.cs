// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConvertStrategies.XmlParamConvertStrategy
// Assembly: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E8F758AE-29ED-44FC-8EF9-3A977263FAEF
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.dll

using Intermech.IpsXmlViewer.Interfaces;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Attributes;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.ParamValueConfig;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Format;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.StrategyParams;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.StrategyProcess;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Utils;
using Intermech.XmlExchange.IpsXml.Interfaces;
using Intermech.XmlExchange.IpsXml.Provider.Ips.Serializer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using XmlReaderAPI.Common;
using XmlReaderAPI.Data;
using XmlReaderAPI.MetaData;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConvertStrategies;

[DefaultConvertStrategyForType(typeof (IXmlParam))]
public class XmlParamConvertStrategy : XmlEntityConvertStrategy
{
  private const string CACHE_PARAM_ID_FORMAT = "{0}.{1}";
  private const string BLOB_DIR = "BLOB";

  public override XmlStrategyConvertResultType Convert()
  {
    if (this.StrategyParams == null)
      return XmlStrategyConvertResultType.MinorError;
    object obj;
    if (!this.StrategyParams.TryGetValue(AddStrategyParamType.ConvertedParamOwner, out obj) || !(obj is ImDataElement))
    {
      this.Logger.Error(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_no_converted_params_owner_provided"));
      return XmlStrategyConvertResultType.MinorError;
    }
    ImDataElement imDataElement = obj as ImDataElement;
    if (!this.StrategyParams.TryGetValue(AddStrategyParamType.ParamOwner, out obj) || !(obj is IXmlEntity))
    {
      this.Logger.Error(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_no_params_owner_provided"));
      return XmlStrategyConvertResultType.MinorError;
    }
    IXmlEntity paramsOwner = obj as IXmlEntity;
    if (!this.StrategyParams.TryGetValue(AddStrategyParamType.ConvertedOwnerParamsCache, out obj) || !(obj is ParamsCache))
    {
      this.Logger.Error(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_no_params_cache_provided"));
      return XmlStrategyConvertResultType.MinorError;
    }
    ParamsCache paramsCache = obj as ParamsCache;
    ParamConfig targetConfig = this.TargetConfig as ParamConfig;
    if (targetConfig.ConfigType == ParamConfigType.Const)
      return this.ConvertConstParam(targetConfig, imDataElement, paramsCache);
    if (targetConfig.ConfigType == ParamConfigType.Simple)
      return targetConfig.ParamParentType == ParamType.Object ? this.ConvertSimpleParam(this.Target as IXmlParam, targetConfig, imDataElement, paramsCache) : this.ConvertLinkedParam(targetConfig, paramsOwner, imDataElement, paramsCache);
    if (targetConfig.ConfigType == ParamConfigType.File)
      return this.ConvertFileParam(this.Target as IXmlParam, targetConfig, imDataElement, paramsCache);
    return targetConfig.ConfigType == ParamConfigType.Calculated ? this.ConvertCalculatedParam(targetConfig, imDataElement, paramsCache) : XmlStrategyConvertResultType.WrongStrategyChoise;
  }

  private XmlStrategyConvertResultType ConvertConstParam(
    ParamConfig paramConfig,
    ImDataElement paramOwner,
    ParamsCache paramsCache)
  {
    XmlStrategyConvertResultType convertResultType = XmlStrategyConvertResultType.Converted;
    IpsDataSerializer service1 = this.GlobalServices.GetService<IpsDataSerializer>();
    ValueConverter service2 = this.GlobalServices.GetService<ValueConverter>();
    foreach (string id in paramConfig.ValueConfigs.Ids)
    {
      ValueConfig valueConfig = paramConfig.ValueConfigs[id];
      if (string.IsNullOrEmpty(valueConfig.DestFieldName))
      {
        this.Logger.Error(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_error_empty_value_dest_fieldname"), (object) valueConfig.Id));
        convertResultType = XmlStrategyConvertResultType.MinorError;
      }
      else
      {
        string str = string.IsNullOrEmpty(valueConfig.ConverterReference.ConverterId) ? valueConfig.Value : service2.Convert(valueConfig.Value, valueConfig.ConverterReference.ConverterId, valueConfig.ConverterReference.Context);
        if (valueConfig.Destination == ValueDestType.ImAttribute)
        {
          ImAttribute attr = (ImAttribute) null;
          object obj;
          if (paramOwner.Attributes.TryGetValue(ImAttributeType.GetDictAttrKey(valueConfig.AttrId), out obj))
          {
            attr = obj as ImAttribute;
          }
          else
          {
            if (paramConfig.Export && valueConfig.Export)
            {
              attr = new ImAttribute();
              attr.SetAsString("F_ATTRIBUTE_ID", valueConfig.AttrId);
              service1.AddAttribute((IImDataElement) paramOwner, attr);
            }
            paramsCache[this.GetCacheParamId(paramConfig.Id, valueConfig.Id)] = str;
          }
          attr?.SetAsString(valueConfig.DestFieldName, str);
          this.Logger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_attr_value_set"), (object) valueConfig.AttrId, (object) valueConfig.DestFieldName, (object) str));
        }
        else if (valueConfig.Destination == ValueDestType.InternalField)
        {
          if (paramConfig.Export && valueConfig.Export)
          {
            if (!this.IsProtectedInternalField(valueConfig.DestFieldName))
              paramOwner.SetAsString(valueConfig.DestFieldName, str);
            else
              this.Logger.Warn(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msgCantWriteToProtectedField"), (object) valueConfig.DestFieldName, (object) str, (object) valueConfig.Id));
          }
          paramsCache[this.GetCacheParamId(paramConfig.Id, valueConfig.Id)] = str;
          this.Logger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_ent_value_set"), (object) valueConfig.DestFieldName, (object) str));
        }
        else
        {
          this.Logger.Error(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_error_unsupported_value_destination"), (object) valueConfig.Destination.ToXMLTag()));
          convertResultType = XmlStrategyConvertResultType.MinorError;
        }
      }
    }
    return convertResultType;
  }

  private XmlStrategyConvertResultType ConvertSimpleParam(
    IXmlParam sourceParam,
    ParamConfig paramConfig,
    ImDataElement paramOwner,
    ParamsCache paramsCache)
  {
    XmlStrategyConvertResultType convertResultType = XmlStrategyConvertResultType.Converted;
    IpsDataSerializer service1 = this.GlobalServices.GetService<IpsDataSerializer>();
    ValueConverter service2 = this.GlobalServices.GetService<ValueConverter>();
    foreach (string id in paramConfig.ValueConfigs.Ids)
    {
      ValueConfig valueConfig = paramConfig.ValueConfigs[id];
      if (string.IsNullOrEmpty(valueConfig.DestFieldName))
      {
        this.Logger.Error(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_error_empty_value_dest_fieldname"), (object) valueConfig.Id));
        convertResultType = XmlStrategyConvertResultType.MinorError;
      }
      else
      {
        string str = string.IsNullOrEmpty(valueConfig.ConverterReference.ConverterId) ? sourceParam.Value : service2.Convert(sourceParam.Value, valueConfig.ConverterReference.ConverterId, valueConfig.ConverterReference.Context);
        if (valueConfig.Destination == ValueDestType.ImAttribute)
        {
          ImAttribute attr = (ImAttribute) null;
          object obj;
          if (paramOwner.Attributes.TryGetValue(ImAttributeType.GetDictAttrKey(valueConfig.AttrId), out obj))
            attr = obj as ImAttribute;
          else if (paramConfig.Export && valueConfig.Export)
          {
            attr = new ImAttribute();
            attr.SetAsString("F_ATTRIBUTE_ID", valueConfig.AttrId);
            service1.AddAttribute((IImDataElement) paramOwner, attr);
          }
          attr?.SetAsString(valueConfig.DestFieldName, str);
          string cacheParamId = this.GetCacheParamId(sourceParam.Id, valueConfig.Id);
          if (!paramsCache.ContainsKey(cacheParamId))
            paramsCache[cacheParamId] = str;
          this.Logger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_attr_value_set"), (object) valueConfig.AttrId, (object) valueConfig.DestFieldName, (object) str));
        }
        else if (valueConfig.Destination == ValueDestType.InternalField)
        {
          if (!this.IsProtectedInternalField(valueConfig.DestFieldName))
            paramOwner.SetAsString(valueConfig.DestFieldName, str);
          else
            this.Logger.Warn(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msgCantWriteToProtectedField"), (object) valueConfig.DestFieldName, (object) str, (object) valueConfig.Id));
          paramsCache[this.GetCacheParamId(sourceParam.Id, valueConfig.Id)] = str;
          this.Logger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_ent_value_set"), (object) valueConfig.DestFieldName, (object) str));
        }
        else
        {
          this.Logger.Error(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_error_unsupported_value_destination"), (object) valueConfig.Destination.ToXMLTag()));
          convertResultType = XmlStrategyConvertResultType.MinorError;
        }
      }
    }
    return convertResultType;
  }

  private XmlStrategyConvertResultType ConvertFileParam(
    IXmlParam sourceParam,
    ParamConfig paramConfig,
    ImDataElement paramOwner,
    ParamsCache paramsCache)
  {
    XmlStrategyConvertResultType convertResultType = XmlStrategyConvertResultType.Converted;
    IpsDataSerializer service1 = this.GlobalServices.GetService<IpsDataSerializer>();
    this.GlobalServices.GetService<ValueConverter>();
    if (paramConfig.ValueConfigs.Count == 0)
    {
      this.Logger.Error(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_error_empty_values_config"), (object) paramConfig.Id));
      return XmlStrategyConvertResultType.MinorError;
    }
    ValueConfig valueConfig = paramConfig.ValueConfigs[paramConfig.ValueConfigs.Ids.First<string>()];
    if (string.IsNullOrEmpty(valueConfig.DestFieldName))
    {
      this.Logger.Error(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_error_empty_value_dest_fieldname"), (object) valueConfig.Id));
      return XmlStrategyConvertResultType.MinorError;
    }
    if (valueConfig.Destination == ValueDestType.ImAttribute)
    {
      ImAttribute imAttribute1 = (ImAttribute) null;
      string arrayParamName = string.Empty;
      int paramIndex = -1;
      bool flag = ParamConfig.IsArrayParam(sourceParam.Name, out arrayParamName, out paramIndex);
      ImAttribute imAttribute2 = (ImAttribute) null;
      object obj;
      if (paramOwner.Attributes.TryGetValue(ImAttributeType.GetDictAttrKey(valueConfig.AttrId), out obj))
      {
        if (flag)
        {
          imAttribute1 = new ImAttribute();
          imAttribute1.SetAsString("F_ATTRIBUTE_ID", valueConfig.AttrId);
          imAttribute1.SetAsString("F_INLIST_ID", System.Convert.ToString(paramIndex + 1));
          imAttribute2 = obj as ImAttribute;
          imAttribute2.SetAsString("F_INLIST_ID", "0");
          imAttribute2.Normalize();
        }
        else
          imAttribute1 = obj as ImAttribute;
      }
      else if (paramConfig.Export && valueConfig.Export)
      {
        imAttribute1 = new ImAttribute();
        imAttribute1.SetAsString("F_ATTRIBUTE_ID", valueConfig.AttrId);
        if (flag)
          imAttribute1.SetAsString("F_INLIST_ID", System.Convert.ToString(paramIndex + 1));
      }
      if (imAttribute1 != null)
      {
        ConvertSessionInfo service2 = this.GlobalServices.GetService<ConvertSessionInfo>();
        string str1 = Path.Combine(Path.GetDirectoryName(service2.InputDataFile), sourceParam.Value);
        if (!File.Exists(str1))
        {
          this.Logger.Error(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_error_file_not_found"), (object) str1));
          return XmlStrategyConvertResultType.MinorError;
        }
        string str2 = Path.Combine(service2.WorkDir, "BLOB");
        if (!Directory.Exists(str2))
          Directory.CreateDirectory(str2);
        string str3 = Path.Combine(str2, Path.GetFileName(sourceParam.Value));
        Path.Combine("BLOB", Path.GetFileName(sourceParam.Value));
        string str4;
        if (File.Exists(str3))
        {
          this.Logger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_dublicate_file_found"), (object) sourceParam.Value));
          string fileName = Path.GetFileName(Path.GetDirectoryName(sourceParam.Value));
          str4 = Path.Combine("BLOB", fileName, Path.GetFileName(sourceParam.Value));
          string str5 = Path.Combine(str2, fileName);
          if (!Directory.Exists(str5))
            Directory.CreateDirectory(str5);
          str3 = Path.Combine(str5, Path.GetFileName(sourceParam.Value));
          File.Copy(str1, str3, true);
          this.Logger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_dublicate_file_copied"), (object) str3));
        }
        else
        {
          File.Copy(str1, str3, true);
          str4 = Path.Combine("BLOB", Path.GetFileName(sourceParam.Value));
        }
        if (File.Exists(str3))
        {
          imAttribute1.SetAsString("F_PATH2FILE", str4);
          imAttribute1.SetAsString("F_VALUE", str4);
          imAttribute1.SetAsString("F_STRING_VALUE", str4);
          imAttribute1.Normalize();
          if (flag && imAttribute2 != null && imAttribute2.CanMergeWith((IImAttribute) imAttribute1))
            imAttribute2.MergeWith((IImAttribute) imAttribute1);
          else
            service1.AddAttribute((IImDataElement) paramOwner, imAttribute1);
        }
      }
      string cacheParamId = this.GetCacheParamId(sourceParam.Id, valueConfig.Id);
      if (!paramsCache.ContainsKey(cacheParamId))
        paramsCache[cacheParamId] = sourceParam.Value;
      this.Logger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_attr_value_set"), (object) valueConfig.AttrId, (object) valueConfig.DestFieldName, (object) sourceParam.Value));
      return convertResultType;
    }
    this.Logger.Error(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_error_unsupported_value_destination"), (object) valueConfig.Destination.ToXMLTag()));
    return XmlStrategyConvertResultType.MinorError;
  }

  private XmlStrategyConvertResultType ConvertLinkedParam(
    ParamConfig paramConfig,
    IXmlEntity paramsOwner,
    ImDataElement convertedParamsOwner,
    ParamsCache paramsCache)
  {
    if (!(paramsOwner is IXmlRelation))
      return XmlStrategyConvertResultType.MinorError;
    IXmlDataProvider service1 = this.GlobalServices.GetService<IXmlDataProvider>();
    IXmlObject target;
    if (paramConfig.ParamParentType == ParamType.ParentObject)
      target = service1.GetRelParentObj(paramsOwner as IXmlRelation);
    else if (paramConfig.ParamParentType == ParamType.ChildObject)
    {
      target = service1.GetRelChildObj(paramsOwner as IXmlRelation);
    }
    else
    {
      this.Logger.Error(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msgParamParentTypeNotSupported"), (object) paramConfig.ParamParentType.ToXMLTag()));
      return XmlStrategyConvertResultType.MinorError;
    }
    ConvertedData convertedData = ConvertUtils.FindConvertedData((IXmlEntity) target, this.GlobalServices);
    if (convertedData == null)
      return XmlStrategyConvertResultType.MinorError;
    IpsDataSerializer service2 = this.GlobalServices.GetService<IpsDataSerializer>();
    ValueConverter service3 = this.GlobalServices.GetService<ValueConverter>();
    XmlStrategyConvertResultType convertResultType = XmlStrategyConvertResultType.Converted;
    foreach (string id in paramConfig.ValueConfigs.Ids)
    {
      ValueConfig valueConfig = paramConfig.ValueConfigs[id];
      if (string.IsNullOrEmpty(valueConfig.DestFieldName))
      {
        this.Logger.Error(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_error_empty_value_dest_fieldname"), (object) valueConfig.Id));
        convertResultType = XmlStrategyConvertResultType.MinorError;
      }
      else
      {
        string cacheParamId = this.GetCacheParamId(paramConfig.ParentParamId, valueConfig.Id);
        string originValue;
        if (!convertedData.ConvertedEntityParams.TryGetValue(cacheParamId, out originValue))
        {
          this.Logger.Error(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_error_no_param_value_in_cache"), (object) paramConfig.Id, (object) valueConfig.Id));
          convertResultType = XmlStrategyConvertResultType.MinorError;
        }
        else
        {
          if (!string.IsNullOrEmpty(valueConfig.ConverterReference.ConverterId))
            originValue = service3.Convert(originValue, valueConfig.ConverterReference.ConverterId, valueConfig.ConverterReference.Context);
          if (valueConfig.Destination == ValueDestType.ImAttribute)
          {
            ImAttribute attr = (ImAttribute) null;
            object obj;
            if (convertedParamsOwner.Attributes.TryGetValue(ImAttributeType.GetDictAttrKey(valueConfig.AttrId), out obj))
            {
              attr = obj as ImAttribute;
            }
            else
            {
              if (paramConfig.Export && valueConfig.Export)
              {
                attr = new ImAttribute();
                attr.SetAsString("F_ATTRIBUTE_ID", valueConfig.AttrId);
                service2.AddAttribute((IImDataElement) convertedParamsOwner, attr);
              }
              paramsCache[this.GetCacheParamId(paramConfig.Id, valueConfig.Id)] = originValue;
            }
            attr?.SetAsString(valueConfig.DestFieldName, originValue);
            this.Logger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_attr_value_set"), (object) valueConfig.AttrId, (object) valueConfig.DestFieldName, (object) originValue));
          }
          else if (valueConfig.Destination == ValueDestType.InternalField)
          {
            if (!this.IsProtectedInternalField(valueConfig.DestFieldName))
              convertedParamsOwner.SetAsString(valueConfig.DestFieldName, originValue);
            else
              this.Logger.Warn(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msgCantWriteToProtectedField"), (object) valueConfig.DestFieldName, (object) originValue, (object) valueConfig.Id));
            paramsCache[this.GetCacheParamId(paramConfig.Id, valueConfig.Id)] = originValue;
            this.Logger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_ent_value_set"), (object) valueConfig.DestFieldName, (object) originValue));
          }
          else
          {
            this.Logger.Error(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_error_unsupported_value_destination"), (object) valueConfig.Destination.ToXMLTag()));
            convertResultType = XmlStrategyConvertResultType.MinorError;
          }
        }
      }
    }
    return convertResultType;
  }

  private XmlStrategyConvertResultType ConvertCalculatedParam(
    ParamConfig paramConfig,
    ImDataElement paramOwner,
    ParamsCache paramsCache)
  {
    Dictionary<string, List<ValueConfig>> dictionary = new Dictionary<string, List<ValueConfig>>();
    List<ValueConfig> valueConfigList1 = new List<ValueConfig>();
    foreach (string id in paramConfig.ValueConfigs.Ids)
    {
      ValueConfig valueConfig = paramConfig.ValueConfigs[id];
      string groupId = valueConfig.GroupId;
      if (string.IsNullOrEmpty(groupId))
      {
        valueConfigList1.Add(valueConfig);
      }
      else
      {
        List<ValueConfig> valueConfigList2;
        if (!dictionary.TryGetValue(groupId, out valueConfigList2))
        {
          valueConfigList2 = new List<ValueConfig>();
          dictionary.Add(groupId, valueConfigList2);
        }
        valueConfigList2.Add(valueConfig);
      }
    }
    IpsDataSerializer service1 = this.GlobalServices.GetService<IpsDataSerializer>();
    ValueConverter service2 = this.GlobalServices.GetService<ValueConverter>();
    XmlStrategyConvertResultType convertResultType = XmlStrategyConvertResultType.Converted;
    foreach (List<ValueConfig> valueConfigList3 in dictionary.Values)
    {
      valueConfigList3.Sort((Comparison<ValueConfig>) ((left, right) => left.Order - right.Order));
      string id = valueConfigList3[0].Id;
      string destFieldName = valueConfigList3[0].DestFieldName;
      ValueDestType destination = valueConfigList3[0].Destination;
      bool export = valueConfigList3[0].Export;
      string attrId = valueConfigList3[0].AttrId;
      ConditionType groupCond = valueConfigList3[0].GroupCond;
      if (string.IsNullOrEmpty(destFieldName))
      {
        this.Logger.Error(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_error_empty_value_dest_fieldname"), (object) valueConfigList3[0].Id));
        convertResultType = XmlStrategyConvertResultType.MinorError;
      }
      else
      {
        string str = string.Empty;
        foreach (ValueConfig valueConfig in valueConfigList3)
        {
          string originValue;
          if (paramsCache.TryGetValue(this.GetCacheParamId(valueConfig.Id, valueConfig.LinkedValueId), out originValue))
          {
            if (!string.IsNullOrEmpty(valueConfig.ConverterReference.ConverterId))
              originValue = service2.Convert(originValue, valueConfig.ConverterReference.ConverterId, valueConfig.ConverterReference.Context);
            if (!string.IsNullOrEmpty(str))
              str += valueConfig.Delimiter;
            str = str + valueConfig.SurrSymbol + originValue + valueConfig.SurrSymbol;
            if (groupCond == ConditionType.Or)
              break;
          }
          else
            this.Logger.Warn(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msgEmptyValueInGroup"), (object) valueConfig.Id));
        }
        switch (destination)
        {
          case ValueDestType.InternalField:
            if (!this.IsProtectedInternalField(destFieldName))
              paramOwner.SetAsString(destFieldName, str);
            else
              this.Logger.Warn(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msgCantWriteToProtectedField"), (object) destFieldName, (object) str, (object) id));
            paramsCache[this.GetCacheParamId(paramConfig.Id, id)] = str;
            this.Logger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_ent_value_set"), (object) destFieldName, (object) str));
            continue;
          case ValueDestType.ImAttribute:
            ImAttribute attr = (ImAttribute) null;
            object obj;
            if (paramOwner.Attributes.TryGetValue(ImAttributeType.GetDictAttrKey(attrId), out obj))
            {
              attr = obj as ImAttribute;
            }
            else
            {
              if (paramConfig.Export & export)
              {
                attr = new ImAttribute();
                attr.SetAsString("F_ATTRIBUTE_ID", attrId);
                service1.AddAttribute((IImDataElement) paramOwner, attr);
              }
              paramsCache[this.GetCacheParamId(paramConfig.Id, id)] = str;
            }
            attr?.SetAsString(destFieldName, str);
            this.Logger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_attr_value_set"), (object) attrId, (object) destFieldName, (object) str));
            continue;
          default:
            this.Logger.Error(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_error_unsupported_value_destination"), (object) destination.ToXMLTag()));
            convertResultType = XmlStrategyConvertResultType.MinorError;
            continue;
        }
      }
    }
    foreach (ValueConfig valueConfig in valueConfigList1)
    {
      if (string.IsNullOrEmpty(valueConfig.DestFieldName))
      {
        this.Logger.Error(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_error_empty_value_dest_fieldname"), (object) valueConfig.Id));
        convertResultType = XmlStrategyConvertResultType.MinorError;
      }
      else
      {
        string originValue;
        if (!paramsCache.TryGetValue(this.GetCacheParamId(valueConfig.Id, valueConfig.LinkedValueId), out originValue))
        {
          this.Logger.Warn(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msgEmptyValueInCalcParam"), (object) valueConfig.Id));
          convertResultType = XmlStrategyConvertResultType.MinorError;
        }
        else
        {
          if (!string.IsNullOrEmpty(valueConfig.ConverterReference.ConverterId))
            originValue = service2.Convert(originValue, valueConfig.ConverterReference.ConverterId, valueConfig.ConverterReference.Context);
          if (valueConfig.Destination == ValueDestType.ImAttribute)
          {
            ImAttribute attr = (ImAttribute) null;
            object obj;
            if (paramOwner.Attributes.TryGetValue(ImAttributeType.GetDictAttrKey(valueConfig.AttrId), out obj))
            {
              attr = obj as ImAttribute;
            }
            else
            {
              if (paramConfig.Export && valueConfig.Export)
              {
                attr = new ImAttribute();
                attr.SetAsString("F_ATTRIBUTE_ID", valueConfig.AttrId);
                service1.AddAttribute((IImDataElement) paramOwner, attr);
              }
              paramsCache[this.GetCacheParamId(paramConfig.Id, valueConfig.Id)] = originValue;
            }
            attr?.SetAsString(valueConfig.DestFieldName, originValue);
            this.Logger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_attr_value_set"), (object) valueConfig.AttrId, (object) valueConfig.DestFieldName, (object) originValue));
          }
          else if (valueConfig.Destination == ValueDestType.InternalField)
          {
            if (!this.IsProtectedInternalField(valueConfig.DestFieldName))
              paramOwner.SetAsString(valueConfig.DestFieldName, originValue);
            else
              this.Logger.Warn(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msgCantWriteToProtectedField"), (object) valueConfig.DestFieldName, (object) originValue, (object) valueConfig.Id));
            paramsCache[this.GetCacheParamId(paramConfig.Id, valueConfig.Id)] = originValue;
            this.Logger.Info(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_ent_value_set"), (object) valueConfig.DestFieldName, (object) originValue));
          }
          else
          {
            this.Logger.Error(string.Format(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.LocalizationHolder.rm.GetString("msg_error_unsupported_value_destination"), (object) valueConfig.Destination.ToXMLTag()));
            convertResultType = XmlStrategyConvertResultType.MinorError;
          }
        }
      }
    }
    return convertResultType;
  }

  protected string GetCacheParamId(string paramConfigId, string valueConfigId)
  {
    return $"{paramConfigId}.{valueConfigId}";
  }

  private bool IsProtectedInternalField(string fieldName) => fieldName == "F_OBJECT_ID";
}
