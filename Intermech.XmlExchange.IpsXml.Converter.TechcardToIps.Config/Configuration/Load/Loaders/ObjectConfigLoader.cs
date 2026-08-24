// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Load.Loaders.ObjectConfigLoader
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

[ConfigLoader(NodeType.ObjectConfig)]
internal class ObjectConfigLoader(ConfigLoadService loadersService, IpsXmlLogger logger) : 
  BaseConvertableEntityWithParamsConfigLoader<ObjectConfig>(loadersService, logger)
{
  protected override void OnLoadAddParams(ObjectConfig target, XElement source)
  {
    this.Logger.Info(LocalizationHolder.rm.GetString("msg_load_obj_config"));
    base.OnLoadAddParams(target, source);
    this.LoadRelationsConfigs(target, source);
    this.Logger.Info(LocalizationHolder.rm.GetString("msg_load_obj_config_complete"));
  }

  private void LoadRelationsConfigs(ObjectConfig target, XElement source)
  {
    XElement source1 = source.Element((XName) NodeType.RelationConfigs.ToXMLTag());
    if (source1 != null)
    {
      BaseConfig baseConfig = this.LoadersService.LoadConfig(source1);
      if (baseConfig is RelationConfigs)
        target.RelationConfigs = baseConfig as RelationConfigs;
    }
    if (target.RelationConfigs != null)
      return;
    target.RelationConfigs = new RelationConfigs();
    this.Logger.Warn(string.Format(LocalizationHolder.rm.GetString("msg_empty_relation_configs"), (object) target.Id));
  }
}
