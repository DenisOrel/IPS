// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Load.Loaders.ConvertationRulesConfigLoader
// Assembly: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 675F82E7-A3E4-4C10-BC83-A1D6F7097D09
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.dll

using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Format;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Load.Attributes;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Load.LoadService;
using Intermech.XmlExchange.IpsXml.Interfaces.Logger;
using System.Xml.Linq;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Load.Loaders;

[ConfigLoader(NodeType.ConvertationRulesConfig)]
internal class ConvertationRulesConfigLoader(ConfigLoadService loadersService, IpsXmlLogger logger) : 
  BaseConfigLoader<ConvertationRulesConfig>(loadersService, logger)
{
  protected override void OnLoadAddParams(ConvertationRulesConfig target, XElement source)
  {
    this.Logger.Info(LocalizationHolder.rm.GetString("msg_load_object_convert_rules"));
    if (this.GetAttrValue(source, AttrType.Object) == "1")
      target.Rules |= ConvertationRules.Object;
    else
      target.Rules ^= ConvertationRules.Object;
    if (this.GetAttrValue(source, AttrType.ObjectParams) == "1")
      target.Rules |= ConvertationRules.ObjectParams;
    else
      target.Rules ^= ConvertationRules.ObjectParams;
    if (this.GetAttrValue(source, AttrType.Relation) == "1")
      target.Rules |= ConvertationRules.Relation;
    else
      target.Rules ^= ConvertationRules.Relation;
    if (this.GetAttrValue(source, AttrType.RelationParams) == "1")
      target.Rules |= ConvertationRules.RelationParams;
    else
      target.Rules ^= ConvertationRules.RelationParams;
    this.Logger.Info(LocalizationHolder.rm.GetString("msg_load_object_convert_rules_complete"));
  }
}
