// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Load.Loaders.IdPartLoader
// Assembly: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 675F82E7-A3E4-4C10-BC83-A1D6F7097D09
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.dll

using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.IdConfig;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Format;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Load.Attributes;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Load.LoadService;
using Intermech.XmlExchange.IpsXml.Interfaces.Logger;
using System.Xml.Linq;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Load.Loaders;

[ConfigLoader(NodeType.IdPart)]
internal class IdPartLoader(ConfigLoadService loadersService, IpsXmlLogger logger) : 
  BaseConfigLoader<IdPart>(loadersService, logger)
{
  protected override void OnLoadAddParams(IdPart target, XElement source)
  {
    this.Logger.Info(LocalizationHolder.rm.GetString("msg_load_id_config"));
    target.ParamType = this.GetAttrValue(source, AttrType.ParentType).ParseParamType();
    target.ParamSubType = this.GetAttrValue(source, AttrType.SubType).ParseParamSubType();
    this.Logger.Info(LocalizationHolder.rm.GetString("msg_load_id_config_complete"));
  }
}
