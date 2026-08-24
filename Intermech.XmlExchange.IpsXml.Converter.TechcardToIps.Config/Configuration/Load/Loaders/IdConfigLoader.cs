// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Load.Loaders.IdConfigLoader
// Assembly: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 675F82E7-A3E4-4C10-BC83-A1D6F7097D09
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.dll

using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.IdConfig;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Format;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Load.Attributes;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Load.LoadService;
using Intermech.XmlExchange.IpsXml.Interfaces.Logger;
using System.Linq;
using System.Xml.Linq;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Load.Loaders;

[ConfigLoader(NodeType.IdConfig)]
internal class IdConfigLoader(ConfigLoadService loadersService, IpsXmlLogger logger) : 
  BaseConfigLoader<Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.IdConfig.IdConfig>(loadersService, logger)
{
  protected override void OnLoadAddParams(Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.IdConfig.IdConfig target, XElement source)
  {
    this.Logger.Info(LocalizationHolder.rm.GetString("msg_load_config_id"));
    if (source.HasElements)
    {
      if (source.Elements().Count<XElement>() > 1)
        this.Logger.Warn(string.Format(LocalizationHolder.rm.GetString("msg_warn_group_id_only_one_child_allowed"), (object) NodeType.IdPart, (object) NodeType.IdPartGroup, (object) source.Name, (object) target.Id, (object) target.Name));
      foreach (XElement element in source.Elements())
      {
        BaseConfig baseConfig = this.LoadersService.LoadConfig(element);
        if (baseConfig is BaseIdPart)
        {
          target.Content = baseConfig as BaseIdPart;
          break;
        }
      }
      if (target.Content == null)
      {
        target.Content = (BaseIdPart) new IdPartGroup();
        this.Logger.Warn(string.Format(LocalizationHolder.rm.GetString("msg_empty_id_config_found"), (object) target.Id));
      }
    }
    target.Type = this.GetAttrValue(source, AttrType.Type).ParseIdConfigType();
    target.CalcResultType = this.GetAttrValue(source, AttrType.ResultType).ParseIdConfigCalcResultType();
    this.Logger.Info(LocalizationHolder.rm.GetString("msg_load_config_id_complete"));
  }
}
