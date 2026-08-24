// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Load.Loaders.ParamConfigLoader
// Assembly: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 675F82E7-A3E4-4C10-BC83-A1D6F7097D09
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.dll

using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.ParamValueConfig;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Format;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Load.Attributes;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Load.LoadService;
using Intermech.XmlExchange.IpsXml.Interfaces.Logger;
using System.Xml.Linq;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Load.Loaders;

[ConfigLoader(NodeType.ParamConfig)]
internal class ParamConfigLoader(ConfigLoadService loadersService, IpsXmlLogger logger) : 
  BaseConvertableEntityConfigLoader<ParamConfig>(loadersService, logger)
{
  protected override void OnLoadAddParams(ParamConfig target, XElement source)
  {
    this.Logger.Info(LocalizationHolder.rm.GetString("msg_load_param_config"));
    base.OnLoadAddParams(target, source);
    target.ConfigType = this.GetAttrValue(source, AttrType.Type).ParseParamConfigType();
    target.ParamParentType = this.GetAttrValue(source, AttrType.ParentType).ParseParamType();
    target.ParentParamId = this.GetAttrValue(source, AttrType.ParentParamId);
    target.ParamSubType = this.GetAttrValue(source, AttrType.SubType).ParseParamSubType();
    XAttribute xattribute = source.Attribute((XName) AttrType.Export.ToXMLTag());
    target.Export = xattribute == null || xattribute.Value == "1";
    XElement source1 = source.Element((XName) NodeType.ValueConfigs.ToXMLTag());
    if (source1 != null)
      target.ValueConfigs = this.LoadersService.LoadConfig(source1) as ValueConfigs;
    if (target.ValueConfigs == null)
      target.ValueConfigs = new ValueConfigs();
    this.Logger.Info(LocalizationHolder.rm.GetString("msg_load_param_config_complete"));
  }
}
